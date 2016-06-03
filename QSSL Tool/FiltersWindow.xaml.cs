using QSSLTool.Gateways;
using System.Windows;

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
            restore();

            RankingFilterComboBox.SelectionChanged += GradeFilterComboBoxSelection;
            AlreadyExpiredCheckBox.Checked += AlreadyExpiredCheckBoxChecked;
            AlreadyExpiredCheckBox.Unchecked += AlreadyExpiredCheckBoxUnchecked;
            WarningCheckBox.Checked += WarningCheckBoxChecked;
            WarningCheckBox.Unchecked += WarningCheckBoxUnchecked;
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
