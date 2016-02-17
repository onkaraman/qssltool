namespace QSSLTool.Compacts
{
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
            MD5
        }

        private AttributeType _attribute;
        public AttributeType Attribute
        {
            get { return _attribute; }
        }
        private string _content;
        public string Content
        {
            get { return _content; }
        }

        public HostEntryAttribute(AttributeType attribute,
            string content)
        {
            _attribute = attribute;
            _content = content;
            if (_content == null) _content = "?";
        }

        public override bool Equals(object obj)
        {
            HostEntryAttribute hea = (HostEntryAttribute)obj;
            if (hea.Content.Equals(_content)) return true;
            return false;
        }

        public override string ToString()
        {
            return _content;
        }
    }
}
