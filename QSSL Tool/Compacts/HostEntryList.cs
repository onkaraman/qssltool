using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSSLTool.Compacts
{
    public class HostEntryList
    {
        private List<HostEntry> _entries;
        public List<HostEntry> List { get { return _entries; } }
        public int Count { get { return _entries.Count; } }

        public HostEntryList()
        {
            _entries = new List<HostEntry>();
        }

        public void Add(HostEntry host)
        {
            _entries.Add(host);
        }
    }
}
