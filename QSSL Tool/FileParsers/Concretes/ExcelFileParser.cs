using Excel;
using QSSLTool.Compacts;
using QSSLTool.Gateways;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace QSSLTool.FileParsers.Concretes
{
    public class ExcelFileParser : FileParser
    {
        private Hashtable _customAttributes;

        public ExcelFileParser(string _filePath, Extension ext)
        {
            filePath = _filePath;
            extension = ext;
            _customAttributes = new Hashtable();

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
            int protocolVersionsIndex = -1;
            int RC4Index = -1;
            int beastIndex = -1;
            int forwardSecrecyIndex = -1;
            int heartbleedIndex = -1;
            int signatureAlgoIndex = -1;
            int poodleIndex = -1;
            int extendedValidIndex = -1;
            int openSSLCCSIndex = -1;
            int HTTPServerSigIndex = -1;
            int serverHostnameIndex = -1;
            int _3DESCipherIndex = -1;

            // Get headers
            reader.Read();
            int columnIndex = 0;
            try
            {
                while (reader.GetString(columnIndex) != null)
                {
                    string cmp = reader.GetString(columnIndex);

                    #region Column finding
                    if (cmp.Equals("IP") && ipIndex == -1) ipIndex = columnIndex;
                    else if (cmp.Contains("URL") && urlIndex == -1) urlIndex = columnIndex;
                    else if (cmp.ToLower().Contains("protocol versions") && protocolVersionsIndex == -1)
                        protocolVersionsIndex = columnIndex;
                    else if (cmp.Contains("RC4") && RC4Index == -1) RC4Index = columnIndex;
                    else if (cmp.ToLower().Contains("ranking") && rankingIndex == -1)
                        rankingIndex = columnIndex;
                    else if (cmp.ToLower().Equals("protocol") && protocolIndex == -1)
                        protocolIndex = columnIndex;
                    else if (cmp.ToLower().Contains("fingerprint") && fingerPrintIndex == -1)
                        fingerPrintIndex = columnIndex;
                    else if (cmp.ToLower().Contains("expiration") && expirationIndex == -1)
                        expirationIndex = columnIndex;
                    else if (cmp.ToLower().Contains("beast") && beastIndex == -1)
                        beastIndex = columnIndex;
                    else if (cmp.ToLower().Contains("forward secrecy") && forwardSecrecyIndex == -1)
                        forwardSecrecyIndex = columnIndex;
                    else if (cmp.ToLower().Contains("heartbleed") && heartbleedIndex == -1)
                        heartbleedIndex = columnIndex;
                    else if (cmp.ToLower().Contains("signature algorithm") && signatureAlgoIndex == -1)
                        signatureAlgoIndex = columnIndex;
                    else if (cmp.ToLower().Contains("poodle") && poodleIndex == -1)
                        poodleIndex = columnIndex;
                    else if (cmp.ToLower().Contains("extended validation") && extendedValidIndex == -1)
                        extendedValidIndex = columnIndex;
                    else if (cmp.ToLower().Contains("openssl ccs") && openSSLCCSIndex == -1)
                        openSSLCCSIndex = columnIndex;
                    else if (cmp.ToLower().Contains("http server sig") && HTTPServerSigIndex == -1)
                        HTTPServerSigIndex = columnIndex;
                    else if (cmp.ToLower().Contains("server host name") && serverHostnameIndex == -1)
                        serverHostnameIndex = columnIndex;
                    else if (cmp.ToLower().Contains("3des cipher presence") && _3DESCipherIndex == -1)
                        _3DESCipherIndex = columnIndex;
                    else
                    {
                        _customAttributes[columnIndex] = cmp;
                    }
                    #endregion

                    columnIndex += 1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Excel header reading touched outer bounds: {0}", ex.Message));
            }

            // Get rows and add them as children of each header
            while (reader.Read() && reader.GetString(urlIndex) != null)
            {
                HostEntry h = new HostEntry(getColumn(reader, urlIndex),
                    getColumn(reader, protocolIndex));

                h.SetIP(getColumn(reader, ipIndex));
                h.SetRanking(getColumn(reader, rankingIndex));
                h.SetFingerPrintCert(getColumn(reader, fingerPrintIndex));
                h.SetExpirationDate(getColumn(reader, expirationIndex));
                h.SetProtocolVersions(getColumn(reader, protocolVersionsIndex));
                h.SetBeastVulnerarbility(getColumn(reader, beastIndex));
                h.SetForwardSecrecy(getColumn(reader, forwardSecrecyIndex));
                h.SetHeartbleedVulnerability(getColumn(reader, heartbleedIndex));
                h.SetSignatureAlgorithm(getColumn(reader, signatureAlgoIndex));
                h.SetPoodleVulnerability(getColumn(reader, poodleIndex));
                h.SetExtendedValidation(getColumn(reader, extendedValidIndex));
                h.SetOpenSSLCCSVulnerable(getColumn(reader, openSSLCCSIndex));
                h.SetHTTPServerSignature(getColumn(reader, HTTPServerSigIndex));
                h.SetServerHostName(getColumn(reader, serverHostnameIndex));
                h.Set3DESPresence(getColumn(reader, _3DESCipherIndex));
                
                foreach (DictionaryEntry entry in _customAttributes)
                {
                    h.AddCustomAttribute((string) entry.Value, 
                        getColumn(reader, (int) entry.Key));
                }
                if (!h.IsEmpty()) entries.Add(h);
            }
            reader.Close();
            ParserDelegator.CallOnParseComplete();
        }

        /// <summary>
        /// Will try to get the content of the passed column.
        /// If it is empty or an error occurs, null will be returned.
        /// </summary>
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
