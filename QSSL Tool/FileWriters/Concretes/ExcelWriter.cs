using OfficeOpenXml;
using QSSLTool.Compacts;
using System.Drawing;
using System.IO;

namespace QSSLTool.FileWriters.Concretes
{
    public class ExcelWriter
    {
        private int _cursor;
        private string _path;
        private HostEntryList _hosts;
        private ExcelPackage _excelPackage;
        private ExcelWorksheet _sheet;

        public ExcelWriter(HostEntryList hosts, string path)
        {
            _cursor = 2;
            _path = path;
            _hosts = hosts;

            _excelPackage = new ExcelPackage();
            _sheet = _excelPackage.Workbook.Worksheets.Add("A");
            _sheet.View.ShowGridLines = true;
            addHeaders();
            applyStyling();
        }

        public void Save()
        {
            _excelPackage.SaveAs(new FileInfo(@"C:\Users\Onur\Desktop\demoOut.xlsx"));
        }

        private void addHeaders()
        {
            addCell("A1", "URL", 20, 1);
            addCell("B1", "Ranking", 10, 2);
            addCell("C1", "IP", 14, 3);
            addCell("D1", "Protocol", 10, 4);
            addCell("E1", "Fingerprint certificate", 30, 5);
            addCell("F1", "RC4 in use?", 20, 6);
            addCell("G1", "MD5 in use?", 18, 7);
            addCell("H1", "Expiration", 20, 8);
            addCell("I1", "TLS", 16, 9);
        }

        private void addCell(string address, string content,
            int width = -1, int column = -1)
        {
            _sheet.Cells[address].Value = content;
            if (width > 0 || column > 0)
            {
                _sheet.Column(column).Width = width;
            }
        }

        private void applyStyling()
        {
            _sheet.Cells["A1:I1"].Style.Font.Bold = true;
            _sheet.Cells["A1:I1"].Style.Font.Color.SetColor(Color.White);
            _sheet.Cells["A1:I1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            _sheet.Cells["A1:I1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(150, 150, 150));
        }

        public void AddRow(HostEntry entry)
        {
            _sheet.Cells[string.Format("A{0}:I{0}", _cursor)].Style.Font.Name = "Arial";
            _sheet.Cells[string.Format("A{0}:I{0}", _cursor)].Style.Font.Size = 9;

            addCell(string.Format("A{0}", _cursor), entry.URL);
            addCell(string.Format("B{0}", _cursor), entry.Ranking);
            addCell(string.Format("C{0}", _cursor), entry.IP);
            addCell(string.Format("D{0}", _cursor), entry.Protocol);
            addCell(string.Format("E{0}", _cursor), entry.FingerPrintCert);
            addCell(string.Format("F{0}", _cursor), entry.RC4);
            addCell(string.Format("G{0}", _cursor), entry.MD5);
            addCell(string.Format("H{0}", _cursor), entry.Expiration.ToString("dd.MM.yyyy"));
            addCell(string.Format("I{0}", _cursor), entry.TLS);
            _cursor += 1;
        }
    }
}
