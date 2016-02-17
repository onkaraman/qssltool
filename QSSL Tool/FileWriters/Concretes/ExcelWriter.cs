using OfficeOpenXml;
using QSSLTool.Compacts;
using System.Drawing;
using System.IO;

namespace QSSLTool.FileWriters.Concretes
{
    public class ExcelWriter
    {
        private enum attribute { positive, neutral, negative, none }
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
            applyGeneralStyling();
            addRows();
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

        private void addCell(string address, string content,
            attribute attr = attribute.none)
        {
            _sheet.Cells[address].Value = content;
            if (attr == attribute.neutral)
            {
                _sheet.Cells[address].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                _sheet.Cells[address].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 235, 156));
                _sheet.Cells[address].Style.Font.Color.SetColor(Color.FromArgb(191,149,0));
            }
            else if (attr == attribute.negative)
            {
                _sheet.Cells[address].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                _sheet.Cells[address].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 199, 206));
                _sheet.Cells[address].Style.Font.Color.SetColor(Color.FromArgb(156, 0, 6));
            }
            else if (attr == attribute.positive)
            {
                _sheet.Cells[address].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                _sheet.Cells[address].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(198, 239, 206));
                _sheet.Cells[address].Style.Font.Color.SetColor(Color.FromArgb(0, 97, 0));
            }
        }

        private void applyGeneralStyling()
        {
            _sheet.Cells["A1:I1"].Style.Font.Bold = true;
            _sheet.Cells["A1:I1"].Style.Font.Color.SetColor(Color.White);
            _sheet.Cells["A1:I1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            _sheet.Cells["A1:I1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(150, 150, 150));
        }

        private void addRows()
        {
            foreach (HostEntry he in _hosts.List) addRow(he);
        }

        private void addRow(HostEntry entry)
        {
            _sheet.Cells[string.Format("A{0}:I{0}", _cursor)].Style.Font.Name = "Arial";
            _sheet.Cells[string.Format("A{0}:I{0}", _cursor)].Style.Font.Size = 9;

            addCell(string.Format("A{0}", _cursor), entry.URL.Content, detemineCellAttribute(entry.URL));
            addCell(string.Format("B{0}", _cursor), entry.Ranking.Content, detemineCellAttribute(entry.Ranking));
            addCell(string.Format("C{0}", _cursor), entry.IP.Content, detemineCellAttribute(entry.IP));
            addCell(string.Format("D{0}", _cursor), entry.Protocol.Content, detemineCellAttribute(entry.Protocol));
            addCell(string.Format("E{0}", _cursor), entry.FingerPrintCert.Content, detemineCellAttribute(entry.FingerPrintCert));
            addCell(string.Format("F{0}", _cursor), entry.RC4.Content, detemineCellAttribute(entry.RC4));
            addCell(string.Format("G{0}", _cursor), entry.MD5.Content, detemineCellAttribute(entry.MD5));
            addCell(string.Format("H{0}", _cursor), entry.Expiration.Content,
                detemineCellAttribute(entry.Expiration));
            addCell(string.Format("I{0}", _cursor), entry.TLS.Content, detemineCellAttribute(entry.TLS));

            _cursor += 1;
        }

        private attribute detemineCellAttribute(HostEntryAttribute s)
        {

            return attribute.none;
        }
    }
}
