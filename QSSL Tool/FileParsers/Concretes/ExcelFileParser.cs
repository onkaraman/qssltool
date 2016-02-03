using System;

namespace QSSLTool.FileParsers.Concretes
{
    public class ExcelFileParser : FileParser
    {
        public ExcelFileParser(string filePath)
        {
            prepareFile();
        }

        public override void Parse()
        {
            throw new NotImplementedException();
        }

        protected override void prepareFile()
        {
            throw new NotImplementedException();
        }
    }
}
