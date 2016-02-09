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
        private DateTime _expiration;
        public DateTime Expiration { get { return _expiration; } }
        private string _TLS;
        public string TLS { get { return _TLS; } }

        private string _rc4;
        public string RC4 { get { return _rc4; } }
        private string _md5;
        public string MD5 { get { return _md5; } }

        private List<AnalyzeDifference> _differences;
        public List<AnalyzeDifference> Differences { get { return _differences; } }

        public HostEntry(string ip, string url, string protocol,
            string ranking, string fingerprint, DateTime expiration,
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

            _differences = new List<AnalyzeDifference>();
        }

        public bool isEmpty()
        {
            if (_IP == null && _url == null) return true;
            return false;
        }

        public void CheckDifferences(HostEntry he)
        {
            if (!_IP.Equals(he.IP))
            {
                _differences.Add(new AnalyzeDifference("IP address", 
                    string.Format("Changed from {0} to {1}", _IP, he.IP)));
            }
            if (!_ranking.Equals(he.Ranking))
            {
                _differences.Add(new AnalyzeDifference("Ranking",
                    string.Format("Changed from {0} to {1}", _ranking, he.Ranking)));
            }
            if (!_fingerPrintCert.Equals(he.FingerPrintCert))
            {
                _differences.Add(new AnalyzeDifference("Fingerprint certificate",
                    string.Format("Changed from {0} to {1}", _fingerPrintCert, he.FingerPrintCert)));
            }
            if (!_expiration.ToString("dd.MM.yyyy").Equals(he.Expiration.ToString("dd.MM.yyyy")))
            {
                _differences.Add(new AnalyzeDifference("Expiration",
                    string.Format("Changed from {0} to {1}", 
                    _expiration.ToString("dd.MM.yyyy"), 
                    he.Expiration.ToString("dd.MM.yyyy"))));
            }
        }

        public void AddDifference(string name, string key)
        {
            if (name == null || key == null || key.Length <= 0) return;
            _differences.Add(new AnalyzeDifference(name, key));
        }
    }
}
