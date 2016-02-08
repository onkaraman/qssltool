using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSSLTool.Compacts
{
    public class HostEntry
    {
        private string _IP;
        public string IP { get { return _IP; } }
        private string _url;
        public string URL { get { return _url; } }
        private string _protocol;
        public string Protocol { get { return _protocol; } }

        private string _ranking;
        public string Ranking { get { return _ranking; } }
        private string _fingerPrintCert;
        public string FingerPrintCert { get { return _fingerPrintCert; } }
        private string _expiration;
        public string Expiration { get { return _fingerPrintCert; } }
        private string _TLS;
        public string TLS { get { return _TLS; } }

        public HostEntry(string ip, string url, string protocol,
            string ranking, string fingerprint, string expiration,
            string tls)
        {
            _IP = ip;
            _url = url;
            _protocol = protocol;
            if (_protocol == null) _protocol = "http";
            _ranking = ranking;
            _fingerPrintCert = fingerprint;
            _expiration = expiration;
            _TLS = tls;
        }

        public bool isEmpty()
        {
            if (_IP == null && _url == null) return true;
            return false;
        }
    }
}
