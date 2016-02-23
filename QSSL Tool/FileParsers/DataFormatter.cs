using QSSLTool.Patterns;
using System;
using System.Collections.Generic;
using SSLLabsApiWrapper.Models.Response.EndpointSubModels;
using System.Windows.Media;
using QSSLTool.Compacts;

namespace QSSLTool.FileParsers
{
    /// <summary>
    /// This static class offers conversion options for API string data.
    /// </summary>
    public class DataFormatter : LazyStatic<DataFormatter>
    {
        public DataFormatter() { }

        /// <summary>
        /// Takes a date represented in miliseconds and converts it
        /// to a DateTime object.
        /// </summary>
        public DateTime UnixToDateTime(long seconds)
        {
            DateTime d = new DateTime(1970, 1, 1, 0, 0, 0);
            return d.AddMilliseconds(seconds);
        }

        /// <summary>
        /// Takes a list of TLS protocols and converts it to a single string.
        /// </summary>
        public string TLSListToString(List<Protocol> protocols)
        {
            string s = "";
            foreach(Protocol p in protocols)
            {
                s += string.Format("{0} {1}, ", p.name, p.version);
            }
            return s;
        }

        public Color ColorHolderToColor(ColorHolder c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }
    }
}
