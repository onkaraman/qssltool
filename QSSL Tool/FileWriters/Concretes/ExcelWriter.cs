using OfficeOpenXml;
using QSSLTool.Compacts;
using QSSLTool.Gateways;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace QSSLTool.FileWriters.Concretes
{
    /// <summary>
    /// This class writes analyzed data to an excel file.
    /// </summary>
    public class ExcelWriter
    {
        private enum coloring { positive, neutral, negative, none }
        private int _cursor;
        private int _filteredOut;
        public int FilteredOut { get { return _filteredOut; } }
        private string _path;
        private List<HostEntry> _hosts;
        private ExcelPackage _excelPackage;
        private ExcelWorksheet _sheet;

        public ExcelWriter(List<HostEntry> hosts, string path)
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

        /// <summary>
        /// Will save the excel file at the path passed in the constructor of this object.
        /// </summary>
        public void Save()
        {
            _excelPackage.SaveAs(new FileInfo(_path));
        }

        private void addHeaders()
        {
            addCell("A1", "URL", 32, 1);
            addCell("B1", "Ranking", 10, 2);
            addCell("C1", "IP", 14, 3);
            addCell("D1", "Protocol", 10, 4);
            addCell("E1", "Fingerprint certificate", 30, 5);
            addCell("F1", "RC4 in use?", 15, 6);
            addCell("G1", "MD5 in use?", 15, 7);
            addCell("H1", "Expiration", 17, 8);
            addCell("I1", "Protocol versions", 23, 9);
            addCell("J1", "Beast vulnerability", 23, 10);
            addCell("K1", "Forward secrecy", 23, 11);
            addCell("L1", "Heartbleed vulnerability", 23, 12);
            //addCell("M1", "TLS", 23, 9);
        }

        private void addCell(string address, string content,
            int width = -1, int column = -1)
        {
            _sheet.Cells[address].Value = content;
            if (width > 0 || column > 0) _sheet.Column(column).Width = width;
        }

        private void addCell(string address, string content,
            coloring col = coloring.none)
        {
            _sheet.Cells[address].Value = content;
            applyCellStyling(address, col);
        }

        /// <summary>
        /// Applies the styling for a single cell based on the colorin enum.
        /// </summary>
        private void applyCellStyling(string address, coloring col)
        {
            _sheet.Cells[address].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

            Color background = Color.White;
            Color foreground = Color.Black;
            ColorHolder chbg = null;
            ColorHolder chfg = null;

            if (col == coloring.neutral)
            {
                chbg = Settings.Static.ColorSettings.NeutralBG;
                chfg = Settings.Static.ColorSettings.NeutralFG;
            }
            else if (col == coloring.negative)
            {
                chbg = Settings.Static.ColorSettings.NegativeBG;
                chfg = Settings.Static.ColorSettings.NegativeFG;
            }
            else if (col == coloring.positive)
            {
                chbg = Settings.Static.ColorSettings.PositiveBG;
                chfg = Settings.Static.ColorSettings.PositiveFG;
            }

            background = Color.FromArgb(chbg.R, chbg.G, chbg.B);
            foreground = Color.FromArgb(chfg.R, chfg.G, chfg.B);

            _sheet.Cells[address].Style.Fill.BackgroundColor.SetColor(background);
            _sheet.Cells[address].Style.Font.Color.SetColor(foreground);
        }

        private void applyGeneralStyling()
        {
            _sheet.Cells["A1:L1"].Style.Font.Bold = true;
            _sheet.Cells["A1:L1"].Style.Font.Color.SetColor(Color.White);
            _sheet.Cells["A1:L1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            _sheet.Cells["A1:L1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(103, 125, 139));
        }

        private void addRows()
        {
            foreach (HostEntry he in _hosts)
            {
                if (he.AppliesToFilters()) addRow(he);
                else _filteredOut += 1;
            }

        }

        /// <summary>
        /// Will add a row to the excel sheet with the data which held by the HostEntry.
        /// </summary>
        /// <param name="entry"></param>
        private void addRow(HostEntry entry)
        {
            _sheet.Cells[string.Format("A{0}:I{0}", _cursor)].Style.Font.Name = "Arial";
            _sheet.Cells[string.Format("A{0}:I{0}", _cursor)].Style.Font.Size = 9;

            addCell(string.Format("A{0}", _cursor), entry.URL.ToString(), detemineCellColoring(entry.URL));
            addCell(string.Format("B{0}", _cursor), entry.Ranking.ToString(), detemineCellColoring(entry.Ranking));
            addCell(string.Format("C{0}", _cursor), entry.IP.ToString(), detemineCellColoring(entry.IP));
            addCell(string.Format("D{0}", _cursor), entry.Protocol.ToString(), detemineCellColoring(entry.Protocol));
            addCell(string.Format("E{0}", _cursor), entry.FingerPrintCert.ToString(), detemineCellColoring(entry.FingerPrintCert));
            addCell(string.Format("F{0}", _cursor), entry.RC4.ToString(), detemineCellColoring(entry.RC4));
            addCell(string.Format("G{0}", _cursor), entry.MD5.ToString(), detemineCellColoring(entry.MD5));
            addCell(string.Format("H{0}", _cursor), entry.Expiration.ToString(),
                detemineCellColoring(entry.Expiration));
            addCell(string.Format("I{0}", _cursor), entry.ProtocolVersions.ToString(), detemineCellColoring(entry.ProtocolVersions));
            addCell(string.Format("J{0}", _cursor), entry.BeastVuln.ToString(), detemineCellColoring(entry.BeastVuln));
            addCell(string.Format("K{0}", _cursor), entry.ForwardSecrecy.ToString(), detemineCellColoring(entry.ForwardSecrecy));
            addCell(string.Format("L{0}", _cursor), entry.Heartbleed.ToString(), detemineCellColoring(entry.Heartbleed));
            _cursor += 1;
        }

        /// <summary>
        /// Will determine the coloring of the cell based on the content it holds
        /// for its type.
        /// </summary>
        private coloring detemineCellColoring(HostEntryAttribute s)
        {
            if (s.Attribute == HostEntryAttribute.AttributeType.Protocol)
            {
                if (s.ToString().ToLower().Equals("http")) return coloring.negative;
                return coloring.positive;
            }
            else if (s.Attribute == HostEntryAttribute.AttributeType.Ranking)
            {
                if (s.ToString().ToLower().StartsWith("a")) return coloring.positive;
                else if (s.ToString().ToLower().StartsWith("b")) return coloring.neutral;
                else return coloring.negative;
            }
            else if (s.Attribute == HostEntryAttribute.AttributeType.Fingerprint)
            {
                if (s.ToString().Contains("256")) return coloring.positive;
                else return coloring.neutral;
            }
            else if (s.Attribute == HostEntryAttribute.AttributeType.Expiration)
            {
                DateTime dt = DateTime.Parse(s.ToString());
                if (dt > DateTime.Today.AddDays(10)) return coloring.positive;
                else if (dt < DateTime.Today) return coloring.negative;
            }
            else if (s.Attribute == HostEntryAttribute.AttributeType.RC4)
            {
                if (s.ToString().Contains("True")) return coloring.negative;
                else if (s.ToString().Contains("False")) return coloring.positive;
            }
            return coloring.neutral;
        }

        public string GetMessage()
        {
            string str = "Excel file has been exported.{0}";
            if (_filteredOut > 0)
            {
                str = string.Format(str, " " + _filteredOut + " items have been filtered out.");
            }
            else str = string.Format(str, "");
            return str;
        }
    }
}
