﻿using System;
using System.Collections.Generic;

namespace QSSLTool.Compacts
{
    /// <summary>
    /// This class encapsulates a host entry in a mass query.
    /// </summary>
    public class HostEntry
    {
        #region Fields
        private HostEntryAttribute _IP;
        public HostEntryAttribute IP { get { return _IP; } }
        private HostEntryAttribute _URL;
        public HostEntryAttribute URL { get { return _URL; } }
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
        #endregion

        public HostEntry(string ip, string url, string protocol,
            string ranking, string fingerprint, DateTime expiration,
            string TLS, string RC4)
        {
            _IP = new HostEntryAttribute(HostEntryAttribute.AttributeType.IP, ip);
            _URL = new HostEntryAttribute(HostEntryAttribute.AttributeType.URL, url);
            _protocol = new HostEntryAttribute(HostEntryAttribute.AttributeType.Protocol, protocol);
            _ranking = new HostEntryAttribute(HostEntryAttribute.AttributeType.Ranking, ranking);
            _FingerPrintCert = new HostEntryAttribute(HostEntryAttribute.AttributeType.Fingerprint, fingerprint);
            _expiration = new HostEntryAttribute(HostEntryAttribute.AttributeType.Expiration, expiration.ToString("dd.MM.yyyy"));
            _TLS = new HostEntryAttribute(HostEntryAttribute.AttributeType.TLS, TLS);
            _RC4 = new HostEntryAttribute(HostEntryAttribute.AttributeType.RC4, RC4);
            _md5 = new HostEntryAttribute(HostEntryAttribute.AttributeType.MD5, "?");
            _differences = new List<AnalyzeDifference>();
        }

        /// <summary>
        /// Checks if a host entry is empty by looking at the IP address and URL.
        /// If those are empty, the whole object will be treated as empty.
        /// </summary>
        public bool IsEmpty()
        {
            if (_IP.ToString().Length < 3 && _URL.ToString().Length < 3) return true;
            return false;
        }
        
        /// <summary>
        /// Checks whether there are differences between the object and the passed object.
        /// If there are any, those will be added to the difference list of this object.
        /// </summary>
        public void CheckDifferences(HostEntry other)
        {
            if (!_IP.Equals(other.IP))
                _differences.Add(new AnalyzeDifference("IP address", getSummary(_IP, other.IP)));
            if (!_URL.ToString().ToLower().Equals(other.URL.ToString().ToLower()))
                _differences.Add(new AnalyzeDifference("URL", getSummary(_URL, other.URL)));
            if (!_ranking.Equals(other.Ranking))
                _differences.Add(new AnalyzeDifference("Ranking", getSummary(_ranking, other.Ranking)));
            if (!_FingerPrintCert.Equals(other.FingerPrintCert))
                _differences.Add(new AnalyzeDifference("Fingerprint cert.", getSummary(_FingerPrintCert, other.FingerPrintCert)));
            if (!_expiration.Equals(other.Expiration))
                _differences.Add(new AnalyzeDifference("Expiration", getSummary(_expiration, other.Expiration)));
            if (!_RC4.Equals(other.RC4))
                _differences.Add(new AnalyzeDifference("RC4 support", getSummary(_RC4, other.RC4)));
            if (!_TLS.Equals(other.TLS))
                _differences.Add(new AnalyzeDifference("TLS", getSummary(_RC4, other.RC4)));
        }

        /// <summary>
        /// Checks whether this object has a difference by the passed keyword.
        /// </summary>
        public bool HasDifference(string keyword)
        {
            foreach(AnalyzeDifference d in _differences)
            {
                if (d.Name.ToLower().Contains(keyword.ToLower()))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Formulates the difference of two HostEntryAttributes. 
        /// </summary>
        private string getSummary(HostEntryAttribute before, HostEntryAttribute now)
        {
            if (before.ToString().Length > 1) return string.Format("Changed from {0} to {1}", before, now);
            else return string.Format("Discovered as {0}", now);
        }

        /// <summary>
        /// Adds a difference to the internal list of this object.
        /// If the name or value is empty, the difference will not be added.
        /// </summary>
        public void AddDifference(string name, string value)
        {
            if (name == null || value == null || value.Length <= 0) return;
            _differences.Add(new AnalyzeDifference(name, value));
        }
    }
}
