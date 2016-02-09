using QSSLTool.Compacts;
using QSSLTool.FileParsers;
using SSLLabsApiWrapper;
using SSLLabsApiWrapper.Models.Response;
using System;
using System.Threading;

namespace QSSLTool.Queries
{
    public class SSLAnalyzer
    {
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
        public event Action OnAnalyzeComplete;

        public SSLAnalyzer(HostEntryList entries, SSLLabsApiService service)
        {
            _service = service;
            _entries = entries;
            _estRuntime = 60;
            _waitInterval = 3;
            _analyzedEntries = new HostEntryList();
        }

        public void Start()
        {
            ThreadPool.QueueUserWorkItem(o => analyze());
        }

        private void analyze()
        {
            foreach(HostEntry host in _entries.List)
            {
                _current = host;
                string url = string.Format("{0}://{1}", 
                    host.Protocol.ToLower(), host.URL);

                Analyze a = _service.AutomaticAnalyze(url, 
                    SSLLabsApiService.Publish.Off, SSLLabsApiService.StartNew.On,
                    SSLLabsApiService.FromCache.Off, 1, SSLLabsApiService.All.On, 
                    SSLLabsApiService.IgnoreMismatch.Off, 200, _waitInterval);

                HostEntry fresh = extractInfoFromAnalysis(a, host);
                _analyzedEntries.Add(fresh);

                host.CheckDifferences(extractInfoFromAnalysis(a, host));
                host.AddDifference("General message", a.endpoints[0].statusMessage);
                host.AddDifference("Detailed message", a.endpoints[0].statusDetailsMessage);

                if (_stopSignal)
                {
                    _stopSignal = false;
                    break;
                }
                notify();
            }
        }

        private void notify()
        {
            _done += 1;
            _updateEstimate = true;
            if (OnAnalyzeComplete != null) OnAnalyzeComplete();
        }

        private HostEntry extractInfoFromAnalysis(Analyze a, HostEntry he)
        {
            string ip = a.endpoints[0].ipAddress;
            string ranking = a.endpoints[0].grade;
            string tls = DataFormatter.Static.TLSListToString(a.endpoints[0].Details.protocols);
            DateTime d = DataFormatter.Static.UnixToDateTime(a.endpoints[0].Details.cert.notAfter);
            string fingerprint = a.endpoints[0].Details.cert.sigAlg;

            return new HostEntry(ip, he.URL, he.Protocol, ranking, fingerprint, d, tls);
        }

        public int EstimateRuntime(DateTime dt)
        {
            int seconds = dt.Second + (dt.Minute * 60);
            if (_done > 2 && _updateEstimate)
            {
                _estRuntime = seconds / _done;
                _updateEstimate = false;
            }
            return (_waitInterval + _estRuntime) * _entries.Count;
        }

        public void Stop()
        {
            _stopSignal = true;
        }
    }
}
