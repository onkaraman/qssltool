using QSSLTool.Compacts;
using SSLLabsApiWrapper;
using SSLLabsApiWrapper.Models.Response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSSLTool.Queries
{
    public class SSLAnalyzer
    {
        private int _done;
        private List<DataNode> _nodes;
        private SSLLabsApiService _service;

        public SSLAnalyzer(List<DataNode> nodes, SSLLabsApiService service)
        {
            _service = service;
            _nodes = nodes;
        }

        public void Start()
        {

        }

        private void analyze()
        {
            Analyze a = _service.AutomaticAnalyze("https://www.google.de", SSLLabsApiService.Publish.Off, SSLLabsApiService.StartNew.On,
                SSLLabsApiService.FromCache.Off, 1, SSLLabsApiService.All.On, SSLLabsApiService.IgnoreMismatch.Off, 200, 3);
            Debug.WriteLine(1);
        }
    }
}
