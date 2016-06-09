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
            assignEvents();
            restore();
        }

        /// <summary>
        /// Will prepare the UI by restoring user settings.
        /// </summary>
        private void setupViews()
        {
            RankingFilterComboBox.ItemsSource = null;
            RankingFilterComboBox.ItemsSource = generateItems();
            AlreadyExpiredCheckBox.Content = string.Format(Properties.Resources.alreadyExpiredCount,
                ExportFilter.Static.ExpiredCount);
            WarningCheckBox.Content = string.Format(Properties.Resources.warnedExpiredCount,
                ExportFilter.Static.WarningCount);
        }

        /// <summary>
        /// Will localize the UI according to the user's regional settings
        /// </summary>
        private void localize()
        {
            RankingFilterLabel.Text = Properties.Resources.rankingFilter;
            ExpirationFilterLabel.Text = Properties.Resources.expirationFilter;
        }

        /// <summary>
        /// Will assign events to the UI controls.
        /// </summary>
        private void assignEvents()
        {
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
                string.Format(Properties.Resources.rankingEverything, 
                    ExportFilter.Static.GradeCount[3]),
                string.Format(Properties.Resources.rankingOnlyAs, 
                    ExportFilter.Static.GradeCount[0]),
                string.Format(Properties.Resources.rankingOnlyBs, 
                    ExportFilter.Static.GradeCount[1]),
                string.Format(Properties.Resources.rankingOnlyCs, 
                    ExportFilter.Static.GradeCount[2]),
                string.Format(Properties.Resources.rankingOnlyLower, 
                    ExportFilter.Static.GradeCount[4])
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

        /// <summary>
        /// Will set the ranking the user wants the results to be filtered by.
        /// </summary>
        private void GradeFilterComboBoxSelection(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ExportFilter.Static.SetRankingOnly(RankingFilterComboBox.Items[RankingFilterComboBox.SelectedIndex].ToString());  
        }

        /// <summary>
        /// Will set the export filter to only export the host entries which 
        /// are already expired.
        /// </summary>
        private void AlreadyExpiredCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            ExportFilter.Static.AlreadyExpired = true;
            AlreadyExpiredCheckBox.IsChecked = true;
        }

        private void AlreadyExpiredCheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            ExportFilter.Static.AlreadyExpired = false;
            AlreadyExpiredCheckBox.IsChecked = false; ;
        }

        /// <summary>
        /// Will set the export filter to only export the host entries which 
        /// will expire soon according to settings.
        /// </summary>
        private void WarningCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            ExportFilter.Static.WarningExpired = true;
            WarningCheckBox.IsChecked = true;
        }

        private void WarningCheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            ExportFilter.Static.WarningExpired = false;
            WarningCheckBox.IsChecked = false;
        }

    }
}
