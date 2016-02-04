using QSSLTool.FileParsers;
using QSSLTool.FileParsers.Concretes;
using System;
using System.IO;

namespace QSSLTool.Gateways
{
    public class ParserDelegator
    {
        private ExcelFileParser _excelParser;
        public static event Action OnParseComplete;
        
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
    }
}
