using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSSLTool.Compacts
{
    public class HostEntry
    {
        private HostEntryAttribute _IP;
        public HostEntryAttribute IP { get { return _IP; } }
        private HostEntryAttribute _url;
        public HostEntryAttribute URL { get { return _url; } }
        private HostEntryAttribute _protocol;
        public HostEntryAttribute Protocol { get { return _protocol; } }

        private HostEntryAttribute _ranking;
        public HostEntryAttribute Ranking { get { return _ranking; } }
        private HostEntryAttribute _FingerPrintCert;
        public HostEntryAttribute FingerPrintCert { get { return _FingerPrintCert; } }
        private HostEntryAttribute _expiration;
        public HostEntryAttribute Expiration { get { return _expiration; } }
        private HostEntryAttribute _TLS;
        public HostEntryAttribute TLS { get { return _TLS; } }

        private HostEntryAttribute _RC4;
        public HostEntryAttribute RC4 { get { return _RC4; } }
        private HostEntryAttribute _md5;
        public HostEntryAttribute MD5 { get { return _md5; } }

        private List<AnalyzeDifference> _differences;
        public List<AnalyzeDifference> Differences { get { return _differences; } }

        public HostEntry(string ip, string url, string protocol,
            string ranking, string fingerprint, DateTime expiration,
            string TLS, string RC4)
        {
            _IP = new HostEntryAttribute(HostEntryAttribute.AttributeType.IP, ip);
            _url = new HostEntryAttribute(HostEntryAttribute.AttributeType.URL, url);
            _protocol = new HostEntryAttribute(HostEntryAttribute.AttributeType.Protocol, protocol);
            _ranking = new HostEntryAttribute(HostEntryAttribute.AttributeType.Ranking, ranking);
            _FingerPrintCert = new HostEntryAttribute(HostEntryAttribute.AttributeType.Fingerprint, fingerprint);
            _expiration = new HostEntryAttribute(HostEntryAttribute.AttributeType.Expiration, expiration.ToString("dd.MM.yyyy"));
            _TLS = new HostEntryAttribute(HostEntryAttribute.AttributeType.TLS, TLS);
            _RC4 = new HostEntryAttribute(HostEntryAttribute.AttributeType.RC4, RC4);
            _md5 = new HostEntryAttribute(HostEntryAttribute.AttributeType.MD5, "?");
            _differences = new List<AnalyzeDifference>();
        }

        public bool isEmpty()
        {
            if (_IP.Content.Length < 3 && _url.Content.Length < 3) return true;
            return false;
        }

        public int CheckDifferences(HostEntry he)
        {
            int diff = 0;
            if (!_IP.Equals(he.IP))
            {
                _differences.Add(new AnalyzeDifference("IP address", getSummary(_IP, he.IP)));
                diff += 1;
            }
            if (!_ranking.Equals(he.Ranking))
            {
                _differences.Add(new AnalyzeDifference("Ranking", getSummary(_ranking, he.Ranking)));
                diff += 1;
            }
            if (!_FingerPrintCert.Equals(he.FingerPrintCert))
            {
                _differences.Add(new AnalyzeDifference("Fingerprint cert.", getSummary(_FingerPrintCert, he.FingerPrintCert)));
                diff += 1;
            }
            if (!_expiration.Equals(he.Expiration))
            {
                _differences.Add(new AnalyzeDifference("Expiration", getSummary(_expiration, he.Expiration)));
                diff += 1;
            }
            if (!_RC4.Equals(he.RC4))
            {
                _differences.Add(new AnalyzeDifference("RC4 support", getSummary(_RC4, he.RC4)));
                diff += 1;
            }
            return diff;
        }

        private string getSummary(HostEntryAttribute before, HostEntryAttribute now)
        {
            if (before.Content.Length > 1) return string.Format("Changed from {0} to {1}", before, now);
            else return string.Format("Discovered as {0}", now);
        }

        public void AddDifference(string name, string key)
        {
            if (name == null || key == null || key.Length <= 0) return;
            _differences.Add(new AnalyzeDifference(name, key));
        }
    }
}
