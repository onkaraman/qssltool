namespace QSSLTool.Compacts
{
    /// <summary>
    /// Encapsuates an attribute of a HostEntry.
    /// </summary>
    public class HostEntryAttribute
    {
        public enum AttributeType
        {
            IP,
            URL,
            Protocol,
            Ranking,
            Fingerprint,
            Expiration,
            TLS,
            RC4,
            MD5,
            SSLVersions,
            Beast,
            PFS,
            Heartbleed
        }

        private AttributeType _attribute;
        public AttributeType Attribute
        {
            get { return _attribute; }
        }
        private string _content;

        /// <summary>
        /// Constructs a HostEntryAttribute. If the content is null, 
        /// a '?' will be applied for it.
        /// </summary>
        public HostEntryAttribute(AttributeType attribute,
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
