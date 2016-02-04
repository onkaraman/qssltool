using Microsoft.Win32;
using QSSLTool.Gateways;
using SSLLabsApiWrapper;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace QSSLTool
{

    public partial class MainWindow : Window
    {
        private SSLLabsApiService _service;
        private ParserDelegator _parserDelegator;
        private DispatcherTimer _runTimer;

        public MainWindow()
        {
            InitializeComponent();
            MainPageWindow.Title = getWindowTitle();

            _service = new SSLLabsApiService("https://api.ssllabs.com/api/v2");
            _parserDelegator = new ParserDelegator();
            ParserDelegator.OnParseComplete += ParserDelegatorOnParseComplete;

            checkConnectionStatus();
            reloadSettings();
            setupViews();
            prepareAnimations();
        }

        private string getWindowTitle()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            return String.Format("QSSL Tool (version {0})", version.ToString());
        }

        private void checkConnectionStatus()
        {
            Info inf = _service.Info();
            if (inf.Online)
            {
                ConnectionDot.Fill = new SolidColorBrush(Color.FromArgb(255, 90, 209, 8));
                ConnectionStatusText.Text = "Connected to API (ssllabs.com)";
            }
            else
            {
                ConnectionDot.Fill = new SolidColorBrush(Color.FromArgb(255, 209, 17, 8));
                ConnectionStatusText.Text = "Not connected to API";
            }
        }

        private void reloadSettings()
        {
            URLField.Text = "https://";
        }

        private void prepareAnimations()
        {
            CurrentStatGrid.Opacity = 0;
        }

        private void setupViews()
        {
            OpenFileButton.Click += OpenFileButtonClick;
            StartButton.Click += StartButtonClick;
        }

        private void OpenFileButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dia = new OpenFileDialog();
            dia.Filter = "Excel 97-2003 (*.xls)|*.xls|Excel 2007 (*.xlsx)|*.xlsx";
            dia.Multiselect = false;

            bool? clicked = dia.ShowDialog();
            if (clicked == true)
            {
                AnalyzeButton.IsEnabled = false;
                OpenFileButton.IsEnabled = false;
                URLField.IsEnabled = false;
                ProgressBar.Visibility = Visibility.Visible;
                _parserDelegator.Delegate(dia.FileName);
            }
        }

        private void ParserDelegatorOnParseComplete()
        {
            Dispatcher.Invoke(delegate()
            {
                URLField.Text = string.Format("Loaded {0} rows",
                    _parserDelegator.ReadyRows);
                ProgressBar.Visibility = Visibility.Collapsed;
                StartButton.Visibility = Visibility.Visible;
            });
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            Storyboard sb = this.FindResource("CurrentStatGrid_In") as Storyboard;
            sb.Begin();
        }

    }
}
