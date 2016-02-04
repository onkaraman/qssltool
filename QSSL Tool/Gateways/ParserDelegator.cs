using QSSLTool.FileParsers;
using QSSLTool.FileParsers.Concretes;
using System;
using System.IO;

namespace QSSLTool.Gateways
{
    public class ParserDelegator
    {
        private int _readyRows;
        public int ReadyRows { get { return _readyRows; } }
        private ExcelFileParser _excelParser;
        public static event Action OnParseComplete;

        private void CalcRows()
        {
            _readyRows = 0;
            _readyRows += _excelParser.Rows;
        }

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

        public ParserDelegator()
        {
            OnParseComplete += CalcRows;
        }
    }
}
