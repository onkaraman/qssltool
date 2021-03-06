﻿using System;
using System.Collections.Generic;
using QSSLTool.FileParsers;
using QSSLTool.Gateways;
using SSLLabsApiWrapper.Models.Response.EndpointSubModels;

namespace QSSLTool.Compacts
{
    /// <summary>
    /// This class encapsulates a host entry in a mass query.
    /// </summary>
    public class HostEntry
    {
        #region Fields
        private bool _warningExpired;
        public bool WarningExpired { get { return _warningExpired; } }
        public bool Expired
        {
            get
            {
                if (_expiration == null) return false;
                else return (DateTime.Parse(_expiration.ToString()) >= DateTime.Now);
            }
        }
        private string _assessmentFailedMessage;

        #region HostEntryAttributes
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
        private HostEntryAttribute _warningExpiration;
        public HostEntryAttribute WarningExpiration { get { return _warningExpiration; } }
        private HostEntryAttribute _protocolVersions;
        public HostEntryAttribute ProtocolVersions { get { return _protocolVersions; } }
        private HostEntryAttribute _RC4;
        public HostEntryAttribute RC4 { get { return _RC4; } }
        private HostEntryAttribute _beast;
        public HostEntryAttribute BeastVulnerable { get { return _beast; } }
        private HostEntryAttribute _forwardSecrecy;
        public HostEntryAttribute ForwardSecrecy { get { return _forwardSecrecy; } }
        private HostEntryAttribute _heartbleed;
        public HostEntryAttribute Heartbleed { get { return _heartbleed; } }
        private HostEntryAttribute _signatureAlgorithm;
        public HostEntryAttribute SignatureAlgorithm { get { return _signatureAlgorithm; } }
        private HostEntryAttribute _poodleVulnarable;
        public HostEntryAttribute PoodleVulnerable { get { return _poodleVulnarable; } }
        private HostEntryAttribute _extendedValidation;
        public HostEntryAttribute ExtendedValidation { get { return _extendedValidation; } }
        private HostEntryAttribute _openSSLCCSVulnerable;
        public HostEntryAttribute OpenSSLCCSVulnerable { get { return _openSSLCCSVulnerable; } }
        private HostEntryAttribute _httpServerSignature;
        public HostEntryAttribute HTTPServerSignature { get { return _httpServerSignature; } }
        private HostEntryAttribute _serverHostname;
        public HostEntryAttribute ServerHostname { get { return _serverHostname; } }
        private HostEntryAttribute __3DES;
        public HostEntryAttribute _3DES { get { return __3DES; } }
        private HostEntryAttribute _bleichenbacher;
        public HostEntryAttribute Bleichenbacher { get { return _bleichenbacher; } }

        private List<HostEntryAttribute> _customAttributes;
        public List<HostEntryAttribute> CustomAttributes { get { return _customAttributes; } }
        private List<AnalyzeDifference> _differences;
        public List<AnalyzeDifference> Differences { get { return _differences; } }
        #endregion
        #endregion

        /// <summary>
        /// Will construct a new HostEntry object.
        /// </summary>
        /// <param name="url">The URL of the host.</param>
        /// <param name="protocol">The URL protocol of the host.</param>
        /// <param name="fillEmpty">True if attributes such as IP should be filled
        /// with empty strings.</param>
        public HostEntry(string url, string protocol)
        {
            //if (protocol == null) protocol = "https";
            //_protocol = new HostEntryAttribute(HostEntryAttribute.Type.Protocol, protocol);
            _protocol = new HostEntryAttribute(HostEntryAttribute.Type.Protocol, "https");

            _URL = new HostEntryAttribute(HostEntryAttribute.Type.URL, url.Trim());
            _customAttributes = new List<HostEntryAttribute>();
            _differences = new List<AnalyzeDifference>();
            _assessmentFailedMessage = "Assessment failed";
        }

        #region Attribute Setters
        /// <summary>
        /// Will set the IP address for this host entry.
        /// </summary>
        public void SetIP(string value)
        {
            if (value == null) value = _assessmentFailedMessage;
            _IP = new HostEntryAttribute(HostEntryAttribute.Type.IP, value);
        }

        /// <summary>
        /// Will set the ranking for this host entry.
        /// </summary>
        public void SetRanking(string value)
        {
            if (value == null) value = _assessmentFailedMessage;
            _ranking = new HostEntryAttribute(HostEntryAttribute.Type.Ranking, value);
        }

        /// <summary>
        /// Will set the fingerprint certificate for this
        /// host entry.
        /// </summary>
        public void SetFingerPrintCert(string value)
        {
            if (value == null) value = _assessmentFailedMessage;
            _FingerPrintCert = new HostEntryAttribute(HostEntryAttribute.Type.Fingerprint, value);
        }

        /// <summary>
        /// Will set the expiration date for this host entry.
        /// </summary>
        public void SetExpirationDate(long value)
        {
            if (value == 0) value = 0;
            DateTime dt = DataFormatter.Static.UnixToDateTime(value);
            DateTime warningDate = dt.Subtract(TimeSpan.FromDays(Settings.Static.AnalyzerSettings.WarningDays));

            _warningExpired = DateTime.Now <= DateTime.Today.Subtract(TimeSpan.FromDays(Settings.Static.AnalyzerSettings.WarningDays));

            _expiration = new HostEntryAttribute(HostEntryAttribute.Type.Expiration, 
                dt.ToString("dd.MM.yyyy"));

            _warningExpiration = new HostEntryAttribute(HostEntryAttribute.Type.WarningExpiration,
                warningDate.ToString("dd.MM.yyyy"));
        }

        /// <summary>
        /// Will set the expiration date for this host entry.
        /// </summary>
        public void SetExpirationDate(string value)
        {
            if (value == null) value = DateTime.Now.ToString("dd.MM.yyyy");
            DateTime warningDate = 
                DateTime.Parse(value).Subtract(TimeSpan.FromSeconds(Settings.Static.AnalyzerSettings.WarningDays));
            _warningExpired = 
                warningDate <= DateTime.Today.Subtract(TimeSpan.FromDays(Settings.Static.AnalyzerSettings.WarningDays));

            _expiration = new HostEntryAttribute(HostEntryAttribute.Type.Expiration, value);
            _warningExpiration = new HostEntryAttribute(HostEntryAttribute.Type.WarningExpiration,
                warningDate.ToString("dd.MM.yyyy"));
        }

        /// <summary>
        /// Will set the TLS versions for this host entry.
        /// </summary>
        /// <param name="value"></param>
        public void SetProtocolVersions(List<Protocol> protocols)
        {
            if (protocols == null) return;
            string str = DataFormatter.Static.ProtocolVersionsToString(protocols);
            _protocolVersions = new HostEntryAttribute(HostEntryAttribute.Type.ProtocolVersions, str);
        }

        /// <summary>
        /// Will set the TLS versions for this host entry.
        /// </summary>
        public void SetProtocolVersions(string value)
        {
            if (value == null) value = _assessmentFailedMessage;
            _protocolVersions = new HostEntryAttribute(HostEntryAttribute.Type.ProtocolVersions, value);
        }

        /// <summary>
        /// Will set the RC4 support for this host entry.
        /// </summary>
        /// <param name="value"></param>
        public void SetRC4(string value)
        {
            if (value == null) value = _assessmentFailedMessage;
            _RC4 = new HostEntryAttribute(HostEntryAttribute.Type.RC4, value);
        }

        /// <summary>
        /// Sets the beast vulnerability of this host entry.
        /// </summary>
        public void SetBeastVulnerarbility(bool value)
        {
            string str = "No";
            if (value) str = "Yes";

            _beast = new HostEntryAttribute(HostEntryAttribute.Type.BeastVulnerability, str);
        }

        /// <summary>
        /// Sets the beast vulnerability of this host entry.
        /// </summary>
        public void SetBeastVulnerarbility(string value)
        {
            if (value == null) value = _assessmentFailedMessage;
            _beast = new HostEntryAttribute(HostEntryAttribute.Type.BeastVulnerability, value);
        }

        /// <summary>
        /// Sets the forward secrecy attribute of this host entry.
        /// </summary>
        public void SetForwardSecrecy(string value)
        {
            if (value == null) value = _assessmentFailedMessage;
            _forwardSecrecy = new HostEntryAttribute(HostEntryAttribute.Type.ForwardSecrecy, value);
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
            _forwardSecrecy = new HostEntryAttribute(HostEntryAttribute.Type.ForwardSecrecy
                ,str);
        }

        /// <summary>
        /// Sets the heartbleed vulnerability for this host entry.
        /// </summary>
        public void SetHeartbleedVulnerability(string value)
        {
            if (value == null) value = _assessmentFailedMessage;
            _heartbleed = new HostEntryAttribute(HostEntryAttribute.Type.Heartbleed
                ,value);
        }

        /// <summary>
        /// Sets the beast vulnerability of this host entry.
        /// </summary>
        public void SetHeartbleedVulnerability(bool value)
        {
            string str = "No";
            if (value) str = "Yes";

            _heartbleed = new HostEntryAttribute(HostEntryAttribute.Type.Heartbleed, str);
        }

        /// <summary>
        /// Will set the signature algorithm of this host entry.
        /// </summary>
        public void SetSignatureAlgorithm(string value)
        {
            if (value == null) value = _assessmentFailedMessage;
            _signatureAlgorithm = new HostEntryAttribute(HostEntryAttribute.Type.SignatureAlgorithm, value);
        }

        /// <summary>
        /// Will set whether this host entry is vulnerable to Poddle.
        /// </summary>
        public void SetPoodleVulnerability(bool poodleSSL, int poodleTLS)
        {
            string str = DataFormatter.Static.PoodleToString(poodleSSL, poodleTLS);
            _poodleVulnarable = new HostEntryAttribute(HostEntryAttribute.Type.PoodleVulnerable, str);
        }

        /// <summary>
        /// Will set whether this host entry is vulnerable to Poddle.
        /// </summary>
        public void SetPoodleVulnerability(string value)
        {
            if (value == null) value = _assessmentFailedMessage;
            _poodleVulnarable = new HostEntryAttribute(HostEntryAttribute.Type.PoodleVulnerable, value);
        }

        /// <summary>
        /// Will set the extended validation of this host entry.
        /// </summary>
        public void SetExtendedValidation(string value)
        {
            string str = "Uknown";
            if (value != null) str = DataFormatter.Static.ExtendedValidationToString(value);
            _extendedValidation = new HostEntryAttribute(HostEntryAttribute.Type.ExtendedValidation, value);
        }

        /// <summary>
        /// Will set the OpenSSL CCS vulnerability of this host entry.
        /// </summary>
        public void SetOpenSSLCCSVulnerable(int value)
        {
            string str = DataFormatter.Static.OpenSSLCCSToString(value);
            _openSSLCCSVulnerable = new HostEntryAttribute(HostEntryAttribute.Type.OpenSSLCCSVulnerable, str);
        }

        /// <summary>
        /// Will set the OpenSSL CCS vulnerability of this host entry.
        /// </summary>
        public void SetOpenSSLCCSVulnerable(string value)
        {
            if (value == null) value = _assessmentFailedMessage;
            _openSSLCCSVulnerable = new HostEntryAttribute(HostEntryAttribute.Type.OpenSSLCCSVulnerable, value);
        }

        /// <summary>
        /// Will set the HTTP Server signature of this host entry.
        /// </summary>
        public void SetHTTPServerSignature(string value)
        {
            if (value == null) value = _assessmentFailedMessage;
            _httpServerSignature = new HostEntryAttribute(HostEntryAttribute.Type.HTTPServerSignature, value);
        }

        /// <summary>
        /// Will set the server host name of this host entry.
        /// </summary>
        public void SetServerHostName(string value)
        {
            if (value == null) value = _assessmentFailedMessage;
            _serverHostname = new HostEntryAttribute(HostEntryAttribute.Type.ServerHostName, value);
        }

        /// <summary>
        /// Will set the TLS_DHE_RSA_WITH_3DES_EDE_CBC_SHA cipher presence. 
        /// </summary>
        public void Set3DESPresence(string value)
        {
            if (value == null) value = "False";
            else
            {
                value = value.ToLower();
                value = value[0].ToString().ToUpper() + value.Substring(1);
            }

            if (!value.Equals("True") && !value.Equals("False"))
                throw new Exception("3DES Cipher presence must either be 'True' or 'False'.");
            __3DES = new HostEntryAttribute(HostEntryAttribute.Type._3DES, value);
        }
        
        public void SetBleichenBacher(string value)
        {
            if (value == null) value = _assessmentFailedMessage;
            else if (value == "-1") value = "test failed";
            else if (value == "0") value = "unknown";
            else if (value == "1") value = "not vulnerable";
            else if (value == "2") value = "vulnerable (weak oracle)";
            else if (value == "3") value = "vulnerable (strong oracle)";
            else if (value == "4") value = " inconsistent results";

            _bleichenbacher = new HostEntryAttribute(HostEntryAttribute.Type.Bleichenbacher, value);
        }
        #endregion


        public void AddCustomAttribute(string name, string value)
        {
            var attr = new HostEntryAttribute(HostEntryAttribute.Type.CustomAttribute, value, name);
            _customAttributes.Add(attr);
        }

        public void AddCustomAttribute(List<HostEntryAttribute> range)
        {
            _customAttributes.AddRange(range);
        }

        /// <summary>
        /// Checks if a host entry is empty by looking at the IP address and URL.
        /// If those are empty, the whole object will be treated as empty.
        /// </summary>
        public bool IsEmpty()
        {
            if (_IP.ToString().Equals(_assessmentFailedMessage) 
                && _URL.ToString().Equals(_assessmentFailedMessage)) return true;
            else if (_IP.ToString().Length < 3 && _URL.ToString().Length <= 1) return true;
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
                _differences.Add(new AnalyzeDifference("URL", getSummary(_URL, other.URL)));
                _differences.Add(new AnalyzeDifference("IP address", getSummary(_IP, other.IP)));
                _differences.Add(new AnalyzeDifference("Ranking", getSummary(_ranking, other.Ranking)));
                _differences.Add(new AnalyzeDifference("Fingerprint cert.", getSummary(_FingerPrintCert, other.FingerPrintCert)));
                _differences.Add(new AnalyzeDifference("Expiration", getSummary(_expiration, other.Expiration)));
                _differences.Add(new AnalyzeDifference("Warning expiration", getSummary(_warningExpiration, other.WarningExpiration)));
                _differences.Add(new AnalyzeDifference("RC4 support", getSummary(_RC4, other.RC4)));
                _differences.Add(new AnalyzeDifference("Protocol versions", getSummary(_protocolVersions, other.ProtocolVersions)));
                _differences.Add(new AnalyzeDifference("Beast vulnerability", getSummary(_beast, other.BeastVulnerable)));
                _differences.Add(new AnalyzeDifference("Forward secrecy", getSummary(_forwardSecrecy, other.ForwardSecrecy)));
                _differences.Add(new AnalyzeDifference("Heartbleed vulnerability", getSummary(_heartbleed, other.Heartbleed)));
                _differences.Add(new AnalyzeDifference("Signature algorithm", getSummary(_signatureAlgorithm, other.SignatureAlgorithm)));
                _differences.Add(new AnalyzeDifference("Poodle vulnerability", getSummary(_poodleVulnarable, other.PoodleVulnerable)));
                _differences.Add(new AnalyzeDifference("Extended validation", getSummary(_extendedValidation, other.ExtendedValidation)));
                _differences.Add(new AnalyzeDifference("OpenSSL CCS vulnerability", getSummary(_openSSLCCSVulnerable, other.OpenSSLCCSVulnerable)));
                _differences.Add(new AnalyzeDifference("HTTP Server signature", getSummary(_httpServerSignature, other.HTTPServerSignature)));
                _differences.Add(new AnalyzeDifference("Server host name", getSummary(_serverHostname, other.ServerHostname)));
                _differences.Add(new AnalyzeDifference("3DES cipher presence", getSummary(__3DES, other._3DES)));
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
            if ((before == null || before.ToString().Length <= 1)
                && now.ToString().Length >= 1) return string.Format("Discovered as {0}", now);
            else if (before.ToString().Length >= 1 && now.ToString().Length >= 1
                && !before.Equals(now)) return string.Format("Changed from {0} to {1}", before, now);
            else if (before.Equals(now)) return string.Format("Unchanged: {0}", now);
            return "Assessment failed";
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
            if (ExportFilter.Static.AlreadyExpired && Expired) return false;
            else if (ExportFilter.Static.WarningExpired && _warningExpired) return false;

            if (!ExportFilter.Static.RankingFilter.Equals("*"))
            {
                if (_ranking.ToString().Length <= 0) return false;
                if (!_ranking.ToString().StartsWith(ExportFilter.Static.RankingFilter)
                    || _ranking.ToString().Contains("failed")) return false;
            }
            return true;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", _URL, _ranking);
        }
    }
}
