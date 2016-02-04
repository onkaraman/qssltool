using QSSLTool.FileParsers;
using QSSLTool.FileParsers.Concretes;
using System.IO;

namespace QSSLTool.Gateways
{
    public class ParserDelegator
    {
        private ExcelFileParser _excelParser;
        
        public void Delegate(string path)
        {
            FileParser.Extension ext = FileParser.GetFileExtension(path);
            if (ext == FileParser.Extension.xls 
                || ext == FileParser.Extension.xlsx)
            {
                _excelParser = new ExcelFileParser(path, ext);
            }
        }

    }
}
