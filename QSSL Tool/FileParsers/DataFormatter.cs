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
        /// Takes a list of protocols and converts it to a single string.
        /// </summary>
        public string ProtocolVersionsToString(List<Protocol> protocols)
        {
            string s = "";
            foreach(Protocol p in protocols)
            {
                s += string.Format("{0} {1}, ", p.name, p.version);
            }
            return s;
        }

        public string PoodleToString(bool poodleSSL, int poodleTLS)
        {
            string ssl = "Yes";
            if (!poodleSSL) ssl = "No";

            string tls = "Yes";
            if (poodleTLS == -3) tls = "Timed out";
            if (poodleTLS == -2) tls = "TLS not supported";
            if (poodleTLS == -1) tls = "Test failed";
            if (poodleTLS == 0) tls = "Unknown";
            if (poodleTLS == 1) tls = "Not vulnerable";
            if (poodleTLS == 2) tls = "Vulnerable"; 

            return string.Format("SSL: {0}, TLS: {1}", ssl, tls);
        }

        public string ExtendedValidationToString(string value)
        {
            string ret = "No";
            if (value.Equals("E")) ret = "Yes";
            return ret;
        }

        public string OpenSSLCCSToString(int value)
        {
            string ret = "";
            if (value == -1) ret = "Test failed";
            if (value == 0) ret = "Unknown";
            if (value == 1) ret = "Not vulnerable";
            if (value == 2) ret = "Possibly vulnerable, but not exploitable";
            if (value == 3) ret = "Nulnerable and exploitable";
            return ret;
        }

        public Color ColorHolderToColor(ColorHolder c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }
    }
}
