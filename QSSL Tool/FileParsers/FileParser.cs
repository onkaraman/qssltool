using System.Data;
using System.IO;

namespace QSSLTool.FileParsers
{
    public abstract class FileParser
    {
        protected string filePath;
        public enum Extension { xls, xlsx }
        protected Extension extension;
        protected FileStream stream;
        protected DataSet dataSet;
        
        public void OpenFile(string path)
        {
            stream = File.Open(path, FileMode.Open, FileAccess.Read);
            prepareFile();
        }

        protected abstract void prepareFile();

        public abstract void Parse();

        public static Extension GetFileExtension(string path)
        {
            if (path.EndsWith("xls")) return Extension.xls;
            else return Extension.xlsx;
        }
    }
}
