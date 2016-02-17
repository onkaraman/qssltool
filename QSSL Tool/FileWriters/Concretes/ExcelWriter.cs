using OfficeOpenXml;
using QSSLTool.Compacts;
using System.IO;

namespace QSSLTool.FileWriters.Concretes
{
    public class ExcelWriter
    {
        private string _path;
        private HostEntryList _hosts;
        private ExcelPackage _excelPackage;
        private ExcelWorksheet _sheet;

        public ExcelWriter(HostEntryList hosts, string path)
        {
            _path = path;
            _hosts = hosts;

            _excelPackage = new ExcelPackage();
            _sheet = _excelPackage.Workbook.Worksheets.Add("A");
            _sheet.View.ShowGridLines = true;
            addHeaders();
            _excelPackage.SaveAs(new FileInfo(@"C:\Users\Onur\Desktop\demoOut.xlsx"));
        }

        private void addHeaders()
        {
            _sheet.Cells["A1"].Value = "URL";
            _sheet.Cells["B1"].Value = "Ranking";
            _sheet.Cells["C1"].Value = "IP";
            _sheet.Cells["D1"].Value = "Protocol";
            _sheet.Cells["E1"].Value = "Fingerprint cert.";
            _sheet.Cells["F1"].Value = "RC4 in use";
            _sheet.Cells["G1"].Value = "MD5 in use";
            _sheet.Cells["H1"].Value = "Expiration";
            _sheet.Cells["I1"].Value = "TLS";
        }
    }
}
