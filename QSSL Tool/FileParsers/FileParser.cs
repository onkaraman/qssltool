using QSSLTool.Compacts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
        protected HostEntryList entries;
        public HostEntryList HostEntries { get { return entries; } }

        public void OpenFile(string path)
        {
            stream = File.Open(path, FileMode.Open, FileAccess.Read);
            entries = new HostEntryList();
        }

        protected abstract void prepareFile();

        public static Extension GetFileExtension(string path)
        {
            if (path.EndsWith("xls")) return Extension.xls;
            else return Extension.xlsx;
        }
    }
}
