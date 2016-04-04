using Excel;
using QSSLTool.Compacts;
using QSSLTool.Gateways;
using System;
using System.Diagnostics;
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
        /// Reads the first row to determine which column is located on which column-index.
        /// After that HostEntries will be created using those indexes and added to the internal
        /// list of HostEntries.
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
            int RC4Index = -1;
            int MD5Index = -1;
            int SSLIndex = -1;

            // Get headers
            reader.Read();
            int columnIndex = 0;
            try
            {
                while (reader.GetString(columnIndex) != null)
                {
                    string cmp = reader.GetString(columnIndex);

                    if (cmp.Contains("IP"))
                        ipIndex = columnIndex;
                    else if (cmp.Contains("URL"))
                        urlIndex = columnIndex;
                    else if (cmp.Contains("TLS"))
                        TLSIndex = columnIndex;
                    else if (cmp.Contains("MD5"))
                        MD5Index = columnIndex;
                    else if (cmp.Contains("RC4"))
                        RC4Index = columnIndex;
                    else if (cmp.ToLower().Contains("ranking"))
                        rankingIndex = columnIndex;
                    else if (cmp.ToLower().Contains("protocol"))
                        protocolIndex = columnIndex;
                    else if (cmp.ToLower().Contains("fingerprint"))
                        fingerPrintIndex = columnIndex;
                    else if (cmp.ToLower().Contains("expiration"))
                        expirationIndex = columnIndex;
                    else if (cmp.ToLower().Contains("ssl version"))
                        SSLIndex = columnIndex;
                    columnIndex += 1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Excel header reading touched outer bounds: {0}", ex.Message));
            }

            // Get rows and add them as children of each header
            while (reader.Read())
            {
                string ssl = getColumn(reader, SSLIndex);

                HostEntry h = new HostEntry(getColumn(reader, urlIndex),
                    getColumn(reader, protocolIndex));

                h.SetIP(getColumn(reader, ipIndex));
                h.SetRanking(getColumn(reader, rankingIndex));
                h.SetFingerPrintCert(getColumn(reader, fingerPrintIndex));
                h.SetExpirationDate(getColumn(reader, expirationIndex));
                h.SetTLS(getColumn(reader, TLSIndex));
                h.SetMD5(getColumn(reader, MD5Index));
                h.SetSSL(getColumn(reader, MD5Index));

                if (!h.IsEmpty()) entries.Add(h);
            }
            reader.Close();
            ParserDelegator.CallOnParseComplete();
        }

        /// <summary>
        /// Will try to get the content of the passed column.
        /// If it is empty or an error occurs, null will be returned.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        private string getColumn(IExcelDataReader reader, int index)
        {
            try
            {
                return reader.GetString(index);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Uses the proper file reader for the Excel file.
        /// </summary>
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
