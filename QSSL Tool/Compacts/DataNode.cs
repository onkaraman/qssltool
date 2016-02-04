using System.Collections.Generic;

namespace QSSLTool.Compacts
{
    public class DataNode
    {
        public string Key { get; }
        public string Value { get; }
        public List<DataNode> Subrows;

        public DataNode(string key, string value)
        {
            Key = key;
            Value = value;
            Subrows = new List<DataNode>();
        }

        public DataNode(string key) : this (key, null) {}
    }
}
