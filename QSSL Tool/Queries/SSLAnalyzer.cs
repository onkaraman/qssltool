using QSSLTool.Compacts;
using QSSLTool.FileParsers;
using QSSLTool.Gateways;
using SSLLabsApiWrapper;
using SSLLabsApiWrapper.Models.Response;
using SSLLabsApiWrapper.Models.Response.EndpointSubModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace QSSLTool.Queries
{
    /// <summary>
    /// Uses the SSLLabsAPIWrapper to make threaded queries possible.
    /// </summary>
    public class SSLAnalyzer
    {
        #region Fields
        private bool _stopSignal;
        private bool _updateEstimate;
        private int _estRuntime;
        private int _waitInterval;
        private int _done;
        public int Done { get { return _done; } }
        private HostEntry _current;
        public HostEntry Current { get { return _current; } }
        public HostEntry RecentlyAnalyzed
        {
            get
            {
                return _entries[_done-1];
            }
        }
        private List<HostEntry> _entries;
        private List<HostEntry> _analyzedEntries;
        public List<HostEntry> AnalyzedEntries { get { return _analyzedEntries; } }
        private SSLLabsApiService _service;
        public event Action OnAnalyzeProgressed;
        public event Action OnAnalyzeComplete;
        #endregion

        /// <summary>
        /// Constructs the SSLAnalyzer using a parsed list of HostEntries and a connected API service object.
        /// </summary>
        public SSLAnalyzer(List<HostEntry> entries, SSLLabsApiService service)
        {
            _service = service;
            _entries = entries;
            _estRuntime = 120;
            _waitInterval = 3;
            _analyzedEntries = new List<HostEntry>();
        }

        /// <summary>
        /// Starts the mass query in a new thread.
        /// </summary>
        public void Start()
        {
            ThreadPool.QueueUserWorkItem(o => analyze());
        }

        /// <summary>
        /// Iterates through the whole list of parsed HostEntries and starts an analysis on each item.
        /// The current iteration will be saved into the _current object to make a callable outside this SSLAnalzer.
        /// After a single analysis is complete, a fresh HostEntry will be created with the information of the analysis.
        /// The differences between _current on the fresh HostEntry will be added to _current.
        /// If a stop signal is set from the outside, the analysis will be stopped.
        /// </summary>
        private void analyze()
        {
            for (int i=0; i < _entries.Count; i+=1)
            {
                _current = _entries[i];
                string url = string.Format("{0}://{1}", 
                    _current.Protocol.ToString().ToLower(), _current.URL);

                Analyze analyzed = _service.AutomaticAnalyze(url, 
                    Settings.Static.AnalyzerSettings.Publish, 
                    SSLLabsApiService.StartNew.Ignore,
                    Settings.Static.AnalyzerSettings.FromCache, 
                    24, 
                    SSLLabsApiService.All.On,
                    Settings.Static.AnalyzerSettings.IgnoreMismatch, 
                    200, _waitInterval);

                HostEntry fresh = extractInfoFromAnalysis(analyzed, _current);
                if (fresh != null)
                {
                    _current.CheckDifferences(fresh);
                    _current = addMetaNotes(analyzed, _current);
                    fresh.AddCustomAttribute(_current.CustomAttributes);
                    _analyzedEntries.Add(fresh);
                }

                if (_stopSignal)
                {
                    _stopSignal = false;
                    break;
                }

                notify();
            }

            if (OnAnalyzeComplete != null) OnAnalyzeComplete();
        }

        /// <summary>
        /// This method adds further notes for the 'Recent outcome' list on the UI.
        /// </summary>
        private HostEntry addMetaNotes(Analyze a, HostEntry host)
        {
            try
            {
                host.AddDifference("General message", a.endpoints[0].statusMessage);
                host.AddDifference("Detailed message", a.endpoints[0].statusDetailsMessage);
            }
            catch (Exception)
            {
                host.AddDifference("Error", a.Errors[0].message);
                host.AddDifference("App", "This entry will be treated as unchanged.");
            }
            return host;
        }

        /// <summary>
        /// Takes the result of the analysis and extracts the information to a new HostEntry.
        /// If the extraction fails, the same HostEntry as passed will be returned.
        /// Otherwise the fresh HostEntry gets returned.
        /// </summary>
        private HostEntry extractInfoFromAnalysis(Analyze a, HostEntry he)
        {
            HostEntry extracted = new HostEntry(he.URL.ToString(), he.Protocol.ToString());
            try
            {
                extracted.SetIP(a.endpoints[0].ipAddress);
                extracted.SetRanking(a.endpoints[0].grade);
                extracted.SetFingerPrintCert(a.endpoints[0].details.cert.sigAlg);
                extracted.SetExpirationDate(a.endpoints[0].details.cert.notAfter);
                extracted.SetProtocolVersions(a.endpoints[0].details.protocols);
                extracted.SetRC4(a.endpoints[0].details.supportsRc4.ToString());
                extracted.SetBeastVulnerarbility(a.endpoints[0].details.vulnBeast);
                extracted.SetForwardSecrecy(a.endpoints[0].details.forwardSecrecy);
                extracted.SetHeartbleedVulnerability(a.endpoints[0].details.heartbleed);
                extracted.SetSignatureAlgorithm(a.endpoints[0].details.cert.sigAlg);
                extracted.SetPoodleVulnerability(a.endpoints[0].details.poodle, a.endpoints[0].details.poodleTls);
                extracted.SetExtendedValidation(a.endpoints[0].details.cert.validationType);
                extracted.SetOpenSSLCCSVulnerable(a.endpoints[0].details.openSslCcs);
                extracted.SetHTTPServerSignature(a.endpoints[0].details.serverSignature);
                extracted.SetServerHostName(a.endpoints[0].serverName);
                //extracted.Set3DESPresence(check3DESCipherPresence(a.endpoints[0].details.suites));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return extracted;
        }

        /// <summary>
        /// Will text if a 3DES Cipher suite is present.
        /// </summary>
        /// <param name="suites"></param>
        /// <returns></returns>
        private string check3DESCipherPresence(Suites suites)
        {
            foreach (List cipher in suites.list)
            {
                if (cipher.name.Equals("TLS_DHE_RSA_WITH_3DES_EDE_CBC_SHA")) return "True";
            }
            return "False";
        }

        /// <summary>
        /// Notifies the subscribed classes that the current iteration of the analysis has been complete.
        /// </summary>
        private void notify()
        {
            _done += 1;
            // This will be set to true in order to make sure that the estimation
            // gets only calculated at the right moments.
            _updateEstimate = true;
            if (OnAnalyzeProgressed != null) OnAnalyzeProgressed();
        }

        /// <summary>
        /// Estimates the runtime for the analysis only when enough analyses have been done.
        /// </summary>
        public int EstimateRuntime(DateTime dt)
        {
            int seconds = dt.Second + (dt.Minute * 60) + (3600 * dt.Hour);
            if (_done > 2 && _updateEstimate)
            {
                // Estimate runtime for a single analysis.
                _estRuntime = seconds / _done;
                _updateEstimate = false;
            }
            return (_waitInterval + _estRuntime) * _entries.Count;
        }

        /// <summary>
        /// Sends the stop signal to the analysis.
        /// The analysis will be stopped after the current analysis has been done.
        /// </summary>
        public void Stop()
        {
            _stopSignal = true;
        }
    }
}
