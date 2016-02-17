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
        private string _FingerPrintCert;
        public string FingerPrintCert { get { return _FingerPrintCert; } }
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
            string tls, string rc4)
        {
            _IP = ip;
            _url = url;
            _protocol = protocol;
            if (_protocol == null) _protocol = "http";

            _ranking = ranking;
            _FingerPrintCert = fingerprint;

            _expiration = expiration;
            _TLS = tls;

            _rc4 = rc4;

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
                _differences.Add(new AnalyzeDifference("IP address", getSummary(_IP, he.IP)));
            }
            if (!_ranking.Equals(he.Ranking))
            {
                _differences.Add(new AnalyzeDifference("Ranking", getSummary(_ranking, he.Ranking)));
            }
            if (!_FingerPrintCert.Equals(he.FingerPrintCert))
            {
                _differences.Add(new AnalyzeDifference("Fingerprint cert.", getSummary(_FingerPrintCert, he.FingerPrintCert)));
            }
            if (!_expiration.ToString("dd.MM.yyyy").Equals(he.Expiration.ToString("dd.MM.yyyy")))
            {
                _differences.Add(new AnalyzeDifference("Expiration", getSummary(_expiration.ToString("dd.MM.yyyy"),
                    he.Expiration.ToString("dd.MM.yyyy"))));
            }
            if (!_rc4.Equals(he.RC4))
            {
                _differences.Add(new AnalyzeDifference("RC4 support", getSummary(_rc4, he.RC4)));
            }
        }

        private string getSummary(string before, string now)
        {
            if (before.Length > 0) return string.Format("Changed from {0} to {1}", before, now);
            else return string.Format("Discovered as {0}", now);
        }

        public void AddDifference(string name, string key)
        {
            if (name == null || key == null || key.Length <= 0) return;
            _differences.Add(new AnalyzeDifference(name, key));
        }
    }
}
