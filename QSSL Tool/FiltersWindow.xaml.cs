using QSSLTool.Gateways;
using System;
using System.Windows;

namespace QSSLTool
{
    /// <summary>
    /// Interaction logic for FiltersWindow.xaml
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
            ExpireDaysTextBox.MaxLength = 3;
            restore();

            RankingFilterComboBox.SelectionChanged += GradeFilterComboBoxSelection;
            AlreadyExpiredCheckBox.Checked += AlreadyExpiredCheckBoxChecked;
            WillExpireCheckBox.Checked += WillExpireCheckBoxChecked;
            ExpireDaysTextBox.TextChanged += ExpireDaysTextBoxTextChanged;
        }

        /// <summary>
        /// Will restore the filter settings to the controls.
        /// </summary>
        private void restore()
        {
            if (ExportFilter.Static.AlreadyExpired)
            {
                AlreadyExpiredCheckBoxChecked(null, null);
                setExpiredSettings(false);
            }
            else if (ExportFilter.Static.ExpireTimeFrame > 0)
            {
                ExpireDaysTextBox.Text = ExportFilter.Static.ExpireTimeFrame.ToString();
                WillExpireCheckBox.IsChecked = true;

                if (ExpireComboBox.Text.ToLower().Equals("days"))
                    ExpireComboBox.SelectedItem = ExpireComboBox.Items[0];
                if (ExpireComboBox.Text.ToLower().Equals("weeks"))
                    ExpireComboBox.SelectedItem = ExpireComboBox.Items[1];
                if (ExpireComboBox.Text.ToLower().Equals("months"))
                    ExpireComboBox.SelectedItem = ExpireComboBox.Items[2];
                if (ExpireComboBox.Text.ToLower().Equals("years"))
                    ExpireComboBox.SelectedItem = ExpireComboBox.Items[3];
            }

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
            WillExpireCheckBox.IsChecked = false;
            setExpiredSettings(false);
            ExportFilter.Static.SetAlreadyExpired(true);
        }

        private void WillExpireCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            ExpireDaysTextBoxTextChanged(null, null);
            AlreadyExpiredCheckBox.IsChecked = false;
            setExpiredSettings(true);
        }

        /// <summary>
        /// Applies the settings to the export only with positive numbers.
        /// </summary>
        private void ExpireDaysTextBoxTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                if (ExpireDaysTextBox.Text.Length <= 0) ExpireDaysTextBox.Text = "";
                else
                {
                    int i = int.Parse(ExpireDaysTextBox.Text);
                    if (i < 0)
                    {
                        i = -i;
                        ExpireDaysTextBox.Text = i.ToString();
                    }
                    ExportFilter.Static.SetAlreadyExpired(false, i, ExpireComboBox.Text);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Only numbers allowed.", "QSSL Tool");
            }
        }

        private void setExpiredSettings(bool set)
        {
            ExpireDaysTextBox.IsEnabled = set;
            ExpireComboBox.IsEnabled = set;
        }
    }
}
