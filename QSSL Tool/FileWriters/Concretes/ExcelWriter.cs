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
        private int _customAttributeCount;
        private int _cursor;
        private int _filteredOut;
        public int FilteredOut { get { return _filteredOut; } }
        private string _path;
        private string _lastParsedColumn;
        private List<HostEntry> _hosts;
        private ExcelPackage _excelPackage;
        private ExcelWorksheet _sheet;

        public ExcelWriter(List<HostEntry> hosts, string path)
        {
            _cursor = 2;
            _path = path;
            _hosts = hosts;
            _customAttributeCount = _hosts[0].CustomAttributes.Count;
        
            _excelPackage = new ExcelPackage();
            _sheet = _excelPackage.Workbook.Worksheets.Add("A");
            _sheet.View.ShowGridLines = true;

            addHeaders();
            applyHeaderStyling();
            addRows();
        }

        /// <summary>
        /// Will save the excel file at the path passed in the constructor of this object.
        /// </summary>
        public void Save()
        {
            _excelPackage.SaveAs(new FileInfo(_path));
        }

        /// <summary>
        /// Will add the headers for known host entry attribute types.
        /// </summary>
        private void addHeaders()
        {
            addCell(ExcelColumnAdresser.Static.NextIndexed(1), 
                "URL", 32, ExcelColumnAdresser.Static.Index);
            addCell(ExcelColumnAdresser.Static.NextIndexed(1), 
                "Ranking", 10, ExcelColumnAdresser.Static.Index);
            addCell(ExcelColumnAdresser.Static.NextIndexed(1), 
                "IP", 14, ExcelColumnAdresser.Static.Index);
            addCell(ExcelColumnAdresser.Static.NextIndexed(1), 
                "Protocol", 10, ExcelColumnAdresser.Static.Index);
            addCell(ExcelColumnAdresser.Static.NextIndexed(1), 
                "Fingerprint certificate", 30, ExcelColumnAdresser.Static.Index);
            addCell(ExcelColumnAdresser.Static.NextIndexed(1), 
                "RC4 in use?", 15, ExcelColumnAdresser.Static.Index);
            addCell(ExcelColumnAdresser.Static.NextIndexed(1), 
                "Expiration", 17, ExcelColumnAdresser.Static.Index);
            addCell(ExcelColumnAdresser.Static.NextIndexed(1),
                "Warning expiration", 22, ExcelColumnAdresser.Static.Index);
            addCell(ExcelColumnAdresser.Static.NextIndexed(1), 
                "Protocol versions", 23, ExcelColumnAdresser.Static.Index);
            addCell(ExcelColumnAdresser.Static.NextIndexed(1), 
                "Beast vulnerability", 23, ExcelColumnAdresser.Static.Index);
            addCell(ExcelColumnAdresser.Static.NextIndexed(1), 
                "Forward secrecy", 35, ExcelColumnAdresser.Static.Index);
            addCell(ExcelColumnAdresser.Static.NextIndexed(1), 
                "Heartbleed vulnerability", 23, ExcelColumnAdresser.Static.Index);
            addCell(ExcelColumnAdresser.Static.NextIndexed(1), 
                "Signature algorithm", 23, ExcelColumnAdresser.Static.Index);
            addCell(ExcelColumnAdresser.Static.NextIndexed(1), 
                "Poodle vulnerable", 25, ExcelColumnAdresser.Static.Index);
            addCell(ExcelColumnAdresser.Static.NextIndexed(1), 
                "Extended validation", 23, ExcelColumnAdresser.Static.Index);
            addCell(ExcelColumnAdresser.Static.NextIndexed(1), 
                "OpenSSL CCS Vulnerable", 23, ExcelColumnAdresser.Static.Index);
            addCell(ExcelColumnAdresser.Static.NextIndexed(1),
                "HTTP Server signature", 30, ExcelColumnAdresser.Static.Index);
            addCell(ExcelColumnAdresser.Static.NextIndexed(1), 
                "Server host name", 35, ExcelColumnAdresser.Static.Index);
            addCell(ExcelColumnAdresser.Static.NextIndexed(1),
                "3DES Cipher Presence", 35, ExcelColumnAdresser.Static.Index);

            _lastParsedColumn = ExcelColumnAdresser.Static.Latest;
            addCustomHeaders();
        }

        /// <summary>
        /// Will add headers to the excel file, the parser couldn't fetch from the API.
        /// </summary>
        private void addCustomHeaders()
        {
            foreach(HostEntryAttribute hea in _hosts[0].CustomAttributes)
            {
                addCell(ExcelColumnAdresser.Static.NextIndexed(1),
                hea.CustomName, 25, ExcelColumnAdresser.Static.Index);
            }
        }

        /// <summary>
        /// Applies the styling for a single cell based on the colorin enum.
        /// </summary>
        private void applyCellStyling(string address, coloring col)
        {
            _sheet.Cells[address].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

            Color background = Color.White;
            Color foreground = Color.Black;
            ColorHolder chbg = Settings.Static.ColorSettings.NoneBG;
            ColorHolder chfg = Settings.Static.ColorSettings.NoneFG;

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

        /// <summary>
        /// Will apply the styling for the headers by coloring them.
        /// Custom headers will be colored differently.
        /// </summary>
        private void applyHeaderStyling()
        {
            string address = "A1:" + string.Format("{0}1", ExcelColumnAdresser.Static.Latest);
            _sheet.Cells[address].Style.Font.Bold = true;
            _sheet.Cells[address].Style.Font.Color.SetColor(Color.White);
            _sheet.Cells[address].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            _sheet.Cells[address].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(136, 161, 177));

            address = "A1:" + string.Format("{0}1", _lastParsedColumn);
            _sheet.Cells[address].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(103, 125, 139));
        }

        /// <summary>
        /// Will add a cell to the spreadsheet to export with the passed address with given context.
        /// </summary>
        /// <param name="address">Where to add the cell.</param>
        /// <param name="content">What to write in it.</param>
        /// <param name="width">The width of the column of the cell.</param>
        /// <param name="columnNr">The number of the column.</param>
        private void addCell(string address, string content,
            int width = -1, int columnNr = -1)
        {
            _sheet.Cells[address].Value = content;
            if (width > 0 || columnNr > 0) _sheet.Column(columnNr).Width = width;
        }

        /// <summary>
        /// Will add a cell to the spreadsheet to export with the passed address with given context.
        /// </summary>
        /// <param name="address">Where to add the cell.</param>
        /// <param name="content">What to write in it.</param>
        /// <param name="col">What coloring to apply for that cell.</param>
        private void addCell(string address, string content,
            coloring col = coloring.none)
        {
            _sheet.Cells[address].Value = content;
            applyCellStyling(address, col);
        }

        /// <summary>
        /// Will run through the list of host entries and add each attribute
        /// to the current row.
        /// </summary>
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
            string address = string.Format("A{0}:{1}{2}", _cursor, ExcelColumnAdresser.Static.Latest, _cursor);
            _sheet.Cells[string.Format(address, _cursor)].Style.Font.Name = "Arial";
            _sheet.Cells[string.Format(address, _cursor)].Style.Font.Size = 8;

            ExcelColumnAdresser.Static.Reset();

            addCell(ExcelColumnAdresser.Static.NextIndexed(_cursor), 
                entry.URL.ToString(), detemineCellColoring(entry.URL));
            addCell(ExcelColumnAdresser.Static.NextIndexed(_cursor), 
                entry.Ranking.ToString(), detemineCellColoring(entry.Ranking));
            addCell(ExcelColumnAdresser.Static.NextIndexed(_cursor), 
                entry.IP.ToString(), detemineCellColoring(entry.IP));
            addCell(ExcelColumnAdresser.Static.NextIndexed(_cursor), 
                entry.Protocol.ToString(), detemineCellColoring(entry.Protocol));
            addCell(ExcelColumnAdresser.Static.NextIndexed(_cursor), 
                entry.FingerPrintCert.ToString(), detemineCellColoring(entry.FingerPrintCert));
            addCell(ExcelColumnAdresser.Static.NextIndexed(_cursor), 
                entry.RC4.ToString(), detemineCellColoring(entry.RC4));
            addCell(ExcelColumnAdresser.Static.NextIndexed(_cursor), 
                entry.Expiration.ToString(), detemineCellColoring(entry.Expiration));
            addCell(ExcelColumnAdresser.Static.NextIndexed(_cursor),
                entry.WarningExpiration.ToString(), detemineCellColoring(entry.WarningExpiration, entry));
            addCell(ExcelColumnAdresser.Static.NextIndexed(_cursor), 
                entry.ProtocolVersions.ToString(), detemineCellColoring(entry.ProtocolVersions));
            addCell(ExcelColumnAdresser.Static.NextIndexed(_cursor), 
                entry.BeastVulnerable.ToString(), detemineCellColoring(entry.BeastVulnerable));
            addCell(ExcelColumnAdresser.Static.NextIndexed(_cursor), 
                entry.ForwardSecrecy.ToString(), detemineCellColoring(entry.ForwardSecrecy));
            addCell(ExcelColumnAdresser.Static.NextIndexed(_cursor), 
                entry.Heartbleed.ToString(), detemineCellColoring(entry.Heartbleed));
            addCell(ExcelColumnAdresser.Static.NextIndexed(_cursor),
                entry.SignatureAlgorithm.ToString(), detemineCellColoring(entry.SignatureAlgorithm));
            addCell(ExcelColumnAdresser.Static.NextIndexed(_cursor),
                entry.PoodleVulnerable.ToString(), detemineCellColoring(entry.PoodleVulnerable));
            addCell(ExcelColumnAdresser.Static.NextIndexed(_cursor),
                entry.ExtendedValidation.ToString(), detemineCellColoring(entry.ExtendedValidation));
            addCell(ExcelColumnAdresser.Static.NextIndexed(_cursor),
                entry.OpenSSLCCSVulnerable.ToString(), detemineCellColoring(entry.OpenSSLCCSVulnerable));
            addCell(ExcelColumnAdresser.Static.NextIndexed(_cursor),
                entry.HTTPServerSignature.ToString(), detemineCellColoring(entry.HTTPServerSignature));
            addCell(ExcelColumnAdresser.Static.NextIndexed(_cursor),
                entry.ServerHostname.ToString(), detemineCellColoring(entry.ServerHostname));
            addCell(ExcelColumnAdresser.Static.NextIndexed(_cursor),
                entry._3DES.ToString(), detemineCellColoring(entry.ServerHostname));

            foreach (HostEntryAttribute hea in entry.CustomAttributes)
            {
                addCell(ExcelColumnAdresser.Static.NextIndexed(_cursor), 
                    hea.ToString(), detemineCellColoring(entry.CustomAttributes[0]));
            }
            _cursor += 1;
        }

        /// <summary>
        /// Will determine the coloring of the cell based on the content it holds
        /// for its type.
        /// </summary>
        private coloring detemineCellColoring(HostEntryAttribute s, HostEntry he = null)
        {
            if (s.Attribute == HostEntryAttribute.Type.Protocol)
            {
                if (s.ToString().ToLower().Equals("http")) return coloring.negative;
                return coloring.positive;
            }
            else if (s.Attribute == HostEntryAttribute.Type.Ranking)
            {
                if (s.ToString().ToLower().StartsWith("a")) return coloring.positive;
                else if (s.ToString().ToLower().StartsWith("b")) return coloring.neutral;
                else return coloring.negative;
            }
            else if (s.Attribute == HostEntryAttribute.Type.Fingerprint)
            {
                if (s.ToString().Contains("256")) return coloring.positive;
                else return coloring.neutral;
            }
            else if (s.Attribute == HostEntryAttribute.Type.Expiration)
            {
                DateTime dt = DateTime.Parse(s.ToString());
                if (dt > DateTime.Today.AddDays(10)) return coloring.positive;
                else if (dt < DateTime.Today) return coloring.negative;
            }
            else if (s.Attribute == HostEntryAttribute.Type.WarningExpiration)
            {
                if (he.WarningExpired) return coloring.negative;
                else return coloring.positive;
            }
            else if (s.Attribute == HostEntryAttribute.Type.RC4)
            {
                if (s.ToString().Contains("True")) return coloring.negative;
                else if (s.ToString().Contains("False")) return coloring.positive;
            }
            else if (s.Attribute == HostEntryAttribute.Type.MD5)
            {
                if (s.ToString().Contains("Yes")) return coloring.negative;
                else if (s.ToString().Contains("No")) return coloring.positive;
            }
            else if (s.Attribute == HostEntryAttribute.Type.BeastVulnerability)
            {
                if (s.ToString().Contains("Yes")) return coloring.negative;
                else if (s.ToString().Contains("No")) return coloring.positive;
            }
            else if (s.Attribute == HostEntryAttribute.Type.ForwardSecrecy)
            {
                if (s.ToString().Contains("Yes")) return coloring.positive;
                else if (s.ToString().Contains("No")) return coloring.negative;
            }
            else if (s.Attribute == HostEntryAttribute.Type.Heartbleed)
            {
                if (s.ToString().Contains("Yes")) return coloring.negative;
                else if (s.ToString().Contains("No")) return coloring.positive;
            }
            else if (s.Attribute == HostEntryAttribute.Type.OpenSSLCCSVulnerable)
            {
                if (s.ToString().Contains("Not")) return coloring.positive;
                else return coloring.negative;
            }
            else if (s.Attribute == HostEntryAttribute.Type.OpenSSLCCSVulnerable)
            {
                if (s.ToString().Contains("Not")) return coloring.positive;
                else return coloring.neutral;
            }
            else if (s.Attribute == HostEntryAttribute.Type._3DES)
            {
                if (s.ToString().Contains("True")) return coloring.negative;
                else return coloring.positive;
            }
            return coloring.none;
        }

        /// <summary>
        /// Will return a message which reports about the export of the excel file.
        /// </summary>
        public string GetMessage()
        {
            string str = string.Format("File has been exported with {0} entries.", _hosts.Count);

            if (_filteredOut > 0) str += string.Format("\n{0} items filtered out.", _filteredOut);
            if (_customAttributeCount > 0) str += string.Format("\n{0} attributes transferred.", _customAttributeCount);
            else str = string.Format(str, "");
            return str;
        }

    }
}
