﻿using QSSLTool.Compacts;
using QSSLTool.FileParsers;
using SSLLabsApiWrapper;
using SSLLabsApiWrapper.Models.Response;
using System;
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
                return _entries.List[_done-1];
            }
        }
        private HostEntryList _entries;
        private HostEntryList _analyzedEntries;
        public HostEntryList AnalyzedEntries { get { return _analyzedEntries; } }
        private SSLLabsApiService _service;
        public event Action OnAnalyzeProgressed;
        public event Action OnAnalyzeComplete;
        #endregion

        /// <summary>
        /// Constructs the SSLAnalyzer using a parsed list of HostEntries and a connected API service object.
        /// </summary>
        public SSLAnalyzer(HostEntryList entries, SSLLabsApiService service)
        {
            _service = service;
            _entries = entries;
            _estRuntime = 60;
            _waitInterval = 3;
            _analyzedEntries = new HostEntryList();
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
                _current = _entries.List[i];
                string url = string.Format("{0}://{1}", 
                    _current.Protocol.Content.ToLower(), _current.URL);

                Analyze a = _service.AutomaticAnalyze(url, 
                    SSLLabsApiService.Publish.Off, SSLLabsApiService.StartNew.On,
                    SSLLabsApiService.FromCache.Off, 1, SSLLabsApiService.All.On, 
                    SSLLabsApiService.IgnoreMismatch.Off, 200, _waitInterval);

                HostEntry fresh = extractInfoFromAnalysis(a, _current);
                _current.CheckDifferences(fresh);
                _current = addMetaNotes(a, _current);
                _analyzedEntries.Add(fresh);

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
            try
            {
                return new HostEntry(a.endpoints[0].ipAddress, 
                    he.URL.Content, 
                    he.Protocol.Content,
                    a.endpoints[0].grade,
                    a.endpoints[0].Details.cert.sigAlg,
                    DataFormatter.Static.UnixToDateTime(a.endpoints[0].Details.cert.notAfter),
                    DataFormatter.Static.TLSListToString(a.endpoints[0].Details.protocols),
                    a.endpoints[0].Details.supportsRc4.ToString());
            }
            catch (Exception)
            {
                return he;
            }
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
            int seconds = dt.Second + (dt.Minute * 60);
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
