using QSSLTool.Gateways;
using System;
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
        private HostEntryAttribute _protocolVersions;
        public HostEntryAttribute ProtocolVersions { get { return _protocolVersions; } }
        private HostEntryAttribute _RC4;
        public HostEntryAttribute RC4 { get { return _RC4; } }
        private HostEntryAttribute _md5;
        public HostEntryAttribute MD5 { get { return _md5; } }
        private HostEntryAttribute _beast;
        public HostEntryAttribute BeastVuln { get { return _beast; } }
        private HostEntryAttribute _forwardSecrecy;
        public HostEntryAttribute ForwardSecrecy { get { return _forwardSecrecy; } }
        private HostEntryAttribute _heartbleed;
        public HostEntryAttribute Heartbleed { get { return _heartbleed; } }

        private List<AnalyzeDifference> _differences;
        public List<AnalyzeDifference> Differences { get { return _differences; } }
        #endregion

        /// <summary>
        /// Will construct a new HostEntry object.
        /// </summary>
        /// <param name="url">The URL of the host.</param>
        /// <param name="protocol">The URL protocol of the host.</param>
        /// <param name="fillEmpty">True if attributes such as IP should be filled
        /// with empty strings.</param>
        public HostEntry(string url, string protocol, bool fillEmpty = true)
        {
            _URL = new HostEntryAttribute(HostEntryAttribute.AttributeType.URL, url);
            _protocol = new HostEntryAttribute(HostEntryAttribute.AttributeType.Protocol, protocol);
            _differences = new List<AnalyzeDifference>();

            if (fillEmpty)
            {
                SetIP("");
                SetRanking("");
                SetFingerPrintCert("");
                SetExpirationDate(DateTime.Now);
                SetProtocolVersions("");
                SetRC4("");
                SetMD5("");
                SetBeastVulnerarbility(false);
                SetHeartbleed(false);
                SetForwardSecrecy("");
            }
        }

        /// <summary>
        /// Will set the IP address for this host entry.
        /// </summary>
        public void SetIP(string value)
        {
            if (value == null) return;
            _IP = new HostEntryAttribute(HostEntryAttribute.AttributeType.IP, value);
        }

        /// <summary>
        /// Will set the ranking for this host entry.
        /// </summary>
        public void SetRanking(string value)
        {
            if (value == null) return;
            _ranking = new HostEntryAttribute(HostEntryAttribute.AttributeType.Ranking, value);
        }

        /// <summary>
        /// Will set the fingerprint certificate for this
        /// host entry.
        /// </summary>
        public void SetFingerPrintCert(string value)
        {
            if (value == null) return;
            _FingerPrintCert = new HostEntryAttribute(HostEntryAttribute.AttributeType.Fingerprint, value);
        }

        /// <summary>
        /// Will set the expiration date for this host entry.
        /// </summary>
        public void SetExpirationDate(DateTime value)
        {
            if (value == null) return;
            _expiration = new HostEntryAttribute(HostEntryAttribute.AttributeType.Expiration, value.ToString("dd.MM.yyyy"));
        }

        /// <summary>
        /// Will set the expiration date for this host entry.
        /// </summary>
        public void SetExpirationDate(string value)
        {
            if (value == null) return;
            _expiration = new HostEntryAttribute(HostEntryAttribute.AttributeType.Expiration, value);
        }

        /// <summary>
        /// Will set the TLS versions for this host entry.
        /// </summary>
        /// <param name="value"></param>
        public void SetProtocolVersions(string value)
        {
            if (value == null) return;
            _protocolVersions = new HostEntryAttribute(HostEntryAttribute.AttributeType.ProtocolVersions, value);
        }

        /// <summary>
        /// Will set the RC4 support for this host entry.
        /// </summary>
        /// <param name="value"></param>
        public void SetRC4(string value)
        {
            if (value == null) return;
            _RC4 = new HostEntryAttribute(HostEntryAttribute.AttributeType.RC4, value);
        }

        /// <summary>
        /// Will set the MD5 availability for this host entry.
        /// </summary>
        /// <param name="value"></param>
        public void SetMD5(string value)
        {
            if (value == null) return;
            _md5 = new HostEntryAttribute(HostEntryAttribute.AttributeType.MD5, "?");
        }

        /// <summary>
        /// Sets the beast vulnerability of this host entry.
        /// </summary>
        public void SetBeastVulnerarbility(bool value)
        {
            string str = "No";
            if (value) str = "Yes";

            _beast = new HostEntryAttribute(HostEntryAttribute.AttributeType.Beast, str);
        }

        /// <summary>
        /// Sets the beast vulnerability of this host entry.
        /// </summary>
        public void SetBeastVulnerarbility(string value)
        {
            if (value == null) return;
            _beast = new HostEntryAttribute(HostEntryAttribute.AttributeType.Beast, value);
        }

        /// <summary>
        /// Sets the forward secrecy attribute of this host entry.
        /// </summary>
        public void SetForwardSecrecy(string value)
        {
            if (value == null) return;
            _forwardSecrecy = new HostEntryAttribute(HostEntryAttribute.AttributeType.ForwardSecrecy
                , value);
        }

        /// <summary>
        /// Sets the forward secrecy attribute of this host entry.
        /// </summary>
        public void SetForwardSecrecy(int value)
        {
            string str = "No";
            if (value == 1) str = "(0) For at least one browser from simulator.";
            else if (value == 2) str = "(1) ECDHE suites, but not DHE.";
            else if (value == 4) str = "(2) Robust: ECDHE + DHE.";
            _forwardSecrecy = new HostEntryAttribute(HostEntryAttribute.AttributeType.ForwardSecrecy
                ,str);
        }

        /// <summary>
        /// Sets the heartbleed vulnerability for this host entry.
        /// </summary>
        public void SetHeartbleed(string value)
        {
            if (value == null) return;
            _heartbleed = new HostEntryAttribute(HostEntryAttribute.AttributeType.Heartbleed
                , value);
        }

        /// <summary>
        /// Sets the beast vulnerability of this host entry.
        /// </summary>
        public void SetHeartbleed(bool value)
        {
            string str = "No";
            if (value) str = "Yes";

            _heartbleed = new HostEntryAttribute(HostEntryAttribute.AttributeType.Heartbleed, str);
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
            try
            {
                if (!_URL.ToString().ToLower().Equals(other.URL.ToString().ToLower()))
                    _differences.Add(new AnalyzeDifference("URL", getSummary(_URL, other.URL)));

                if (_IP != null)
                {
                    if (!_IP.Equals(other.IP))
                        _differences.Add(new AnalyzeDifference("IP address",
                            getSummary(_IP, other.IP)));
                }

                if (_ranking != null)
                {
                    if (!_ranking.Equals(other.Ranking))
                        _differences.Add(new AnalyzeDifference("Ranking", 
                            getSummary(_ranking, other.Ranking)));
                }

                if (_FingerPrintCert != null)
                {
                    if (!_FingerPrintCert.Equals(other.FingerPrintCert))
                        _differences.Add(new AnalyzeDifference("Fingerprint cert.",
                            getSummary(_FingerPrintCert, other.FingerPrintCert)));
                }

                if (_expiration != null)
                {
                    if (!_expiration.Equals(other.Expiration))
                        _differences.Add(new AnalyzeDifference("Expiration",
                            getSummary(_expiration, other.Expiration)));
                }

                if (_RC4 != null)
                {
                    if (!_RC4.Equals(other.RC4))
                        _differences.Add(new AnalyzeDifference("RC4 support",
                            getSummary(_RC4, other.RC4)));
                }

                if (_protocolVersions != null)
                {
                    if (!_protocolVersions.Equals(other.ProtocolVersions))
                        _differences.Add(new AnalyzeDifference("Protocol versions",
                            getSummary(_protocolVersions, other.ProtocolVersions)));
                }

                if (_beast != null)
                {
                    if (!_beast.Equals(other.BeastVuln))
                        _differences.Add(new AnalyzeDifference("Beast vuln.",
                            getSummary(_beast, other.BeastVuln)));
                }

                if (_heartbleed != null)
                {
                    if (!_heartbleed.Equals(other.Heartbleed))
                        _differences.Add(new AnalyzeDifference("Heartbleed vuln.",
                            getSummary(_heartbleed, other.Heartbleed)));
                }

                if (_forwardSecrecy != null)
                {
                    if (!_forwardSecrecy.Equals(other.ForwardSecrecy))
                        _differences.Add(new AnalyzeDifference("Forward secrecy",
                            getSummary(_forwardSecrecy, other.ForwardSecrecy)));
                }

            }
            catch (Exception)
            {
                _differences.Add(new AnalyzeDifference("Error", "Try analyzing without cache (settings)."));
            }
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

        /// <summary>
        /// Returns true if this host entry is comforming with the filter settings.
        /// </summary>
        /// <returns></returns>
        public bool AppliesToFilters()
        {
            if (ExportFilter.Static.AlreadyExpired)
            {
                if (DateTime.Parse(_expiration.ToString()) >= DateTime.Now)
                    return false; 
            }
            else if (ExportFilter.Static.ExpireTimeFrame > 0)
            {
                DateTime now = DateTime.Now;
                if (ExportFilter.Static.ExpireTimeUnit.ToLower().Equals("days"))
                    now = now.AddDays(ExportFilter.Static.ExpireTimeFrame);
                if (ExportFilter.Static.ExpireTimeUnit.ToLower().Equals("weeks"))
                    now = now.AddDays(ExportFilter.Static.ExpireTimeFrame*7);
                if (ExportFilter.Static.ExpireTimeUnit.ToLower().Equals("months"))
                    now = now.AddMonths(ExportFilter.Static.ExpireTimeFrame);
                if (ExportFilter.Static.ExpireTimeUnit.ToLower().Equals("years"))
                    now = now.AddYears(ExportFilter.Static.ExpireTimeFrame);

                if (DateTime.Parse(_expiration.ToString()) <= now) return false;
            }

            if (!ExportFilter.Static.RankingFilter.Equals("*"))
            {
                if (!ExportFilter.Static.RankingFilter.StartsWith(_ranking.ToString())) return false;
            }
            return true;
        }
    }
}
