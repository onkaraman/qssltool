using Excel;
using QSSLTool.Compacts;
using QSSLTool.Gateways;
using System;
using System.Threading;

namespace QSSLTool.FileParsers.Concretes
{
    public class ExcelFileParser : FileParser
    {
        public ExcelFileParser(string _filePath, Extension ext)
        {
            filePath = _filePath;
            extension = ext;

            OpenFile(filePath);
            prepareFile();
        }

        /// <summary>
        /// Maps the imported Excel file to a DataNode-List. The first row
        /// will be treated as the header row. All subsequent rows will be added
        /// as childs of the respective header rows.
        /// </summary>
        private void parse(IExcelDataReader reader)
        {
            int ipIndex = -1;
            int urlIndex = -1;
            int protocolIndex = -1;
            int rankingIndex = -1;
            int fingerPrintIndex = -1;
            int expirationIndex = -1;
            int TLSIndex = -1;
            int rc4Index = -1;
            int md5Index = -1;

            // Get headers
            reader.Read();
            int columnIndex = 0;
            while (reader.GetString(columnIndex) != null)
            {
                string cmp = reader.GetString(columnIndex);

                if (cmp.Contains("IP")) ipIndex = columnIndex;
                else if (cmp.Contains("URL")) urlIndex = columnIndex;
                else if (cmp.Contains("TLS")) TLSIndex = columnIndex;
                else if (cmp.Contains("MD5")) md5Index = columnIndex;
                else if (cmp.Contains("RC4")) rc4Index = columnIndex;
                else if (cmp.ToLower().Contains("ranking")) rankingIndex = columnIndex;
                else if (cmp.ToLower().Contains("protocol")) protocolIndex = columnIndex;
                else if (cmp.ToLower().Contains("fingerprint")) fingerPrintIndex = columnIndex;
                else if (cmp.ToLower().Contains("expiration")) expirationIndex = columnIndex;
                columnIndex += 1; 
            }

            // Get rows and add them as children of each header
            while (reader.Read())
            {
                HostEntry h = new HostEntry(reader.GetString(ipIndex),
                         reader.GetString(urlIndex), reader.GetString(protocolIndex),
                         reader.GetString(rankingIndex), reader.GetString(fingerPrintIndex),
                         Convert.ToDateTime(reader.GetString(expirationIndex)), 
                         reader.GetString(TLSIndex));
                if (!h.isEmpty()) entries.Add(h);
            }
            reader.Close();
            ParserDelegator.CallOnParseComplete();
        }

        protected override void prepareFile()
        {
            IExcelDataReader reader;
            if (extension == Extension.xlsx)
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            else reader = ExcelReaderFactory.CreateBinaryReader(stream);

            ThreadPool.QueueUserWorkItem(o => parse(reader));
        }
    }
}
