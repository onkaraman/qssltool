using QSSLTool.Compacts;
using SSLLabsApiWrapper;
using SSLLabsApiWrapper.Models.Response;
using System;
using System.Threading;

namespace QSSLTool.Queries
{
    public class SSLAnalyzer
    {
        private bool _updateEstimate;
        private int _done;
        public int Done { get { return _done; } }
        private HostEntry _current;
        public HostEntry Current { get { return _current; } }
        private HostEntryList _entries;
        private SSLLabsApiService _service;
        private int _estRuntime;
        private int _waitInterval;
        public event Action OnAnalyzeComplete;

        public SSLAnalyzer(HostEntryList entries, SSLLabsApiService service)
        {
            _service = service;
            _entries = entries;
            _estRuntime = 60;
            _waitInterval = 3;
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

                _done += 1;
                _updateEstimate = true;
                if (OnAnalyzeComplete != null) OnAnalyzeComplete();
            }
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
    }
}
