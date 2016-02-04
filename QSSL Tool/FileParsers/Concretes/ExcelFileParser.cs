﻿using Excel;
using QSSLTool.Compacts;
using QSSLTool.Gateways;
using System;
using System.Threading;

namespace QSSLTool.FileParsers.Concretes
{
    public class ExcelFileParser : FileParser
    {
        public int Rows
        {
            get
            {
                return nodes[0].Subrows.Count - 1;
            }
        }


        public ExcelFileParser(string _filePath, Extension ext)
        {
            filePath = _filePath;
            extension = ext;

            OpenFile(filePath);
            prepareFile();
        }

        private void parse(IExcelDataReader reader)
        {
            // Get headers
            reader.Read();
            int i = 0;
            while (reader.GetString(i) != null)
            {
                nodes.Add(new DataNode(reader.GetString(i)));
                i += 1;
            }

            // Get rows and add them as children of each header
            i = 0;
            while (reader.Read())
            {
                while (reader.GetString(i) != null)
                {
                    nodes[i].Subrows.Add(new DataNode(i.ToString(),
                        reader.GetString(i)));
                    i += 1;
                }
                i = 0;
            }
            reader.Close();
            ParserDelegator.CallOnParseComplete();
        }

        protected override void prepareFile()
        {
            IExcelDataReader reader;
            if (extension == Extension.xlsx)
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            else reader = ExcelReaderFactory.CreateBinaryReader(stream);
            System.Threading.ThreadPool.QueueUserWorkItem(o => parse(reader));
        }
    }
}
