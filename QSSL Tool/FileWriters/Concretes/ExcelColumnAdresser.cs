using QSSLTool.Patterns;
using System.Collections.Generic;
using System.Linq;

namespace QSSLTool.FileWriters.Concretes
{
    /// <summary>
    /// Will provide the next column address for an excel sheet.
    /// </summary>
    public class ExcelColumnAdresser : LazyStatic<ExcelColumnAdresser>
    {
        private int _index = 0;
        public int Index { get { return _index; } }
        private string _latest;
        public string Previous { get { return _latest; } }
        public List<string> _addresses;
        public string Next
        {
            get
            {
                _latest = _addresses[_index++];
                return _latest;
            }
        }

        /// <summary>
        /// Will return the most recent column address.
        /// </summary>
        public string Latest
        { get { return _latest; } }

        public ExcelColumnAdresser()
        {
            _index = 0;
            _addresses = new List<string>();
            _addresses.AddRange(generate().Take(200));
        }

        private string toBase26(long i)
        {
            if (i == 0) return ""; i--;
            return toBase26(i / 26) + (char)('A' + i % 26);
        }

        private IEnumerable<string> generate()
        {
            long n = 0;
            while (true)
            {
                yield return toBase26(++n);
            }
        }

        /// <summary>
        /// Will return the next character with the passed index.
        /// Will increment the index after this call.
        /// Example: A1
        /// </summary>
        public string NextIndexed(int index)
        {
            return string.Format("{0}{1}", Next, index);
        }

        /// <summary>
        /// Will reset the current position of the alphabet and the index.
        /// </summary>
        public void Reset()
        {
            _index = 0;
            _addresses = new List<string>();
            _addresses.AddRange(generate().Take(200));
        }
    }
}
