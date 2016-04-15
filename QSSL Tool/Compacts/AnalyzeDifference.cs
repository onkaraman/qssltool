namespace QSSLTool.Compacts
{
    /// <summary>
    /// This object represents a list item for the UI.
    /// </summary>
    public class AnalyzeDifference
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public AnalyzeDifference(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, Value);
        }
    }
}
