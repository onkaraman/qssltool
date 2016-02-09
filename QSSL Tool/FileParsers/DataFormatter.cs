using QSSLTool.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSLLabsApiWrapper.Models.Response.EndpointSubModels;

namespace QSSLTool.FileParsers
{
    public class DataFormatter : LazyStatic<DataFormatter>
    {
        public DataFormatter() { }

        public DateTime UnixToDateTime(long seconds)
        {
            DateTime d = new DateTime(1970, 1, 1, 0, 0, 0);
            return d.AddMilliseconds(seconds);
        }

        public string TLSListToString(List<Protocol> protocols)
        {
            string s = "";
            foreach(Protocol p in protocols)
            {
                s += string.Format("{0} {1}, ", p.name, p.version);
            }
            return s;
        }
    }
}
