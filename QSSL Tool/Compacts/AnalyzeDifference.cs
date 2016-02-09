namespace QSSLTool.Compacts
{
    public class AnalyzeDifference
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public AnalyzeDifference(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
