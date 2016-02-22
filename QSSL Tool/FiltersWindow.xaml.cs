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
            ExpireDaysTextBox.Text = "3";

            RankingFilterComboBox.SelectionChanged += GradeFilterComboBoxSelection;
            AlreadyExpiredCheckBox.Checked += AlreadyExpiredCheckBoxChecked;
            WillExpireCheckBox.Checked += WillExpireCheckBoxChecked;
            ExpireDaysTextBox.TextChanged += ExpireDaysTextBoxTextChanged;

            setExpireSettings(false);
        }

        private void GradeFilterComboBoxSelection(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string s = RankingFilterComboBox.Text;  
        }

        private void AlreadyExpiredCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            WillExpireCheckBox.IsChecked = false;
            setExpireSettings(false);
        }

        private void WillExpireCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            AlreadyExpiredCheckBox.IsChecked = false;
            setExpireSettings(true);
        }

        private void setExpireSettings(bool set)
        {
            ExpireDaysTextBox.IsEnabled = set;
            ExpireComboBox.IsEnabled = set;
        }

        private void ExpireDaysTextBoxTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
