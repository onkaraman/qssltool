using QSSLTool.Compacts;
using QSSLTool.Gateways;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace QSSLTool
{
    /// <summary>
    /// Will apply the UI input to the global filter settings for the export.
    /// </summary>
    public partial class FiltersWindow : Window
    {
        public FiltersWindow()
        {
            InitializeComponent();
            setupViews();
        }

        private void setupViews()
        {
            RankingFilterComboBox.ItemsSource = null;
            RankingFilterComboBox.ItemsSource = generateItems();
            restore();

            RankingFilterComboBox.SelectionChanged += GradeFilterComboBoxSelection;
            AlreadyExpiredCheckBox.Checked += AlreadyExpiredCheckBoxChecked;
            AlreadyExpiredCheckBox.Unchecked += AlreadyExpiredCheckBoxUnchecked;
            WarningCheckBox.Checked += WarningCheckBoxChecked;
            WarningCheckBox.Unchecked += WarningCheckBoxUnchecked;
        }

        /// <summary>
        /// Will generate a list of combobox items to filter out host 
        /// entries by their rankings.
        /// </summary>
        /// <returns></returns>
        private List<string> generateItems()
        {
            List<string> cbi = new List<string>
            {
                string.Format("* Everything ({0})", ExportFilter.Static.GradeCount[3]),
                string.Format("Only As ({0})", ExportFilter.Static.GradeCount[0]),
                string.Format("Only Bs ({0})", ExportFilter.Static.GradeCount[1]),
                string.Format("Only Cs ({0})", ExportFilter.Static.GradeCount[2]),
                string.Format("Lower than Cs ({0})", ExportFilter.Static.GradeCount[4])
            };

            return cbi;
        }

        /// <summary>
        /// Will restore the filter settings to the controls.
        /// </summary>
        private void restore()
        {
            if (ExportFilter.Static.AlreadyExpired) AlreadyExpiredCheckBoxChecked(null, null);

            if (ExportFilter.Static.WarningExpired) WarningCheckBoxChecked(null, null);

            if (ExportFilter.Static.RankingFilter.Contains("*"))
                RankingFilterComboBox.SelectedIndex = 0;
            if (ExportFilter.Static.RankingFilter.Contains("A"))
                RankingFilterComboBox.SelectedIndex = 1;
            if (ExportFilter.Static.RankingFilter.Contains("B"))
                RankingFilterComboBox.SelectedIndex = 2;
            if (ExportFilter.Static.RankingFilter.Contains("C"))
                RankingFilterComboBox.SelectedIndex = 3;
            if (ExportFilter.Static.RankingFilter.Contains("Lower"))
                RankingFilterComboBox.SelectedIndex = 4;
        }

        private void GradeFilterComboBoxSelection(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ExportFilter.Static.SetRankingOnly(RankingFilterComboBox.Items[RankingFilterComboBox.SelectedIndex].ToString());  
        }

        private void AlreadyExpiredCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            ExportFilter.Static.AlreadyExpired = true;
        }

        private void AlreadyExpiredCheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            ExportFilter.Static.AlreadyExpired = false;
        }

        private void WarningCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            ExportFilter.Static.WarningExpired = true;
        }

        private void WarningCheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            ExportFilter.Static.WarningExpired = false;
        }

    }
}
