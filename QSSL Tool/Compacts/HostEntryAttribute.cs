namespace QSSLTool.Compacts
{
    /// <summary>
    /// Encapsuates an attribute of a HostEntry.
    /// </summary>
    public class HostEntryAttribute
    {
        public enum Type
        {
            IP,
            URL,
            Protocol,
            Ranking,
            Fingerprint,
            Expiration,
            ProtocolVersions,
            RC4,
            MD5,
            BeastVulnerability,
            ForwardSecrecy,
            Heartbleed,
            SignatureAlgorithm,
            PoodleVulnerable,
            ExtendedValidation,
            OpenSSLCCSVulnerable,
            LongHandshakeIntolerance,
            TLSVersionIntolerance,
            HTTPServerSignature,
            PublicKeyPinning,
            TLSCompression,
            ServerHostName
        }

        private Type _attribute;
        public Type Attribute
        {
            get { return _attribute; }
        }
        private string _content;

        /// <summary>
        /// Constructs a HostEntryAttribute. If the content is null, 
        /// a '?' will be applied for it.
        /// </summary>
        public HostEntryAttribute(Type attribute,
            string content)
        {
            _attribute = attribute;
            _content = content;
            if (_content == null) _content = "?";
        }

        /// <summary>
        /// Checks if two attributes are equal by their values.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            HostEntryAttribute hea = (HostEntryAttribute)obj;
            if (hea.ToString().Equals(_content)) return true;
            return false;
        }

        public override string ToString()
        {
            return _content;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
