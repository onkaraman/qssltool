using QSSLTool.Compacts;
using QSSLTool.FileParsers;
using QSSLTool.FileParsers.Concretes;
using System;
using System.Collections.Generic;
using System.IO;

namespace QSSLTool.Gateways
{
    /// <summary>
    /// This class delegates the read filename to a file parser
    /// which is specialized for that file type.
    /// </summary>
    public class ParserDelegator
    {
        private ExcelFileParser _excelParser;
        public static event Action OnParseComplete;

        /// <summary>
        /// Will choose the fitting file parser according to the file extension
        /// of the imported file.
        /// </summary>
        public void Delegate(string path)
        {
            FileParser.Extension ext = FileParser.GetFileExtension(path);
            if (ext == FileParser.Extension.xls 
                || ext == FileParser.Extension.xlsx)
            {
                _excelParser = new ExcelFileParser(path, ext);
            }
        }

        public static void CallOnParseComplete()
        {
            if (OnParseComplete != null) OnParseComplete();
        }

        public List<HostEntry> GetHostEntries()
        {
            return _excelParser.HostEntries;
        }

    }
}
