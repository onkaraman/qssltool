using System.Collections.Generic;

namespace QSSLTool.Compacts
{
    public class DataNodeList
    {
        private List<DataNode> _list;
        
        public DataNodeList()
        {
            _list = new List<DataNode>();
        }

        /// <summary>
        /// Adds a DataNode to the internal list.
        /// </summary>
        public void Add(DataNode node)
        {
            _list.Add(node);
        }

        /// <summary>
        /// Adds a DataNode as a child of the DataNode
        /// on the passed index.
        /// </summary>
        public void Add(DataNode node, int index)
        {
            _list[index].Subrows.Add(node);
        }
    }
}
