using System;

namespace QSSLTool.FileParsers.Concretes
{
    public class ExcelFileParser : FileParser
    {
        public ExcelFileParser(string _filePath, Extension ext)
        {
            filePath = _filePath;
            extension = ext;
            prepareFile();
        }

        public override void Parse()
        {
            throw new NotImplementedException();
        }

        protected override void prepareFile()
        {
            if (extension == Extension.xlsx)
            {

            }
        }
    }
}
