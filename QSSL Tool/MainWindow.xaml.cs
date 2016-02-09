using Microsoft.Win32;
using QSSLTool.Gateways;
using QSSLTool.Queries;
using SSLLabsApiWrapper;
using SSLLabsApiWrapper.Models.Response;
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
        private SSLAnalyzer _sslAnalyzer;
        private DispatcherTimer _runTimer;
        private DateTime _dateTimeNow;

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
            RecentOutcomeGrid.Opacity = 0;
        }

        private void setupViews()
        {
            OpenFileButton.Click += OpenFileButtonClick;
            StartButton.Click += StartButtonClick;

            ElapsedTimeLabel.Text = "";
            HostsCheckedLabel.Text = "";
        }


        private void OpenFileButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dia = new OpenFileDialog();
            dia.ValidateNames = false;
            dia.Filter = "Excel 2007 (*.xlsx)|*.xlsx|Excel 97 - 2003(*.xls) | *.xls";
            dia.Multiselect = false;

            bool? clicked = dia.ShowDialog();
            if (clicked == true)
            {
                AnalyzeButton.IsEnabled = false;
                OpenFileButton.IsEnabled = false;
                URLField.IsEnabled = false;
                ProgressBar.Visibility = Visibility.Visible;
                URLField.Text = dia.FileName;
                _parserDelegator.Delegate(dia.FileName);
            }
        }

        private void ParserDelegatorOnParseComplete()
        {
            Dispatcher.Invoke(delegate()
            {
                ProgressBar.Visibility = Visibility.Collapsed;
                StartButton.Visibility = Visibility.Visible;

            });
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            Storyboard sb = FindResource("CurrentStatGrid_In") as Storyboard;
            sb.Begin();
            _sslAnalyzer = new SSLAnalyzer(_parserDelegator.GetHostEntries(), _service);
            _sslAnalyzer.OnAnalyzeComplete += OnAnalyzeComplete;
            _sslAnalyzer.Start();

            _dateTimeNow = new DateTime();

            _runTimer = new DispatcherTimer();
            _runTimer.Interval = TimeSpan.FromSeconds(1);
            _runTimer.Tick += runTimerTick;
            _runTimer.Start();

            ProgressBar.Visibility = Visibility.Visible;
        }

        private void OnAnalyzeComplete()
        {
            Dispatcher.Invoke(delegate ()
            {
                if (RecentOutcomeGrid.Opacity == 0)
                {
                    Storyboard sb = FindResource("RecentOutcomeGrid_In") as Storyboard;
                    sb.Begin();
                }
                RecentOutcomeLabel.Text = string.Format("Recent outcome for {0}",
                        _sslAnalyzer.RecentlyAnalyzed.URL);
                DifferenceListBox.ItemsSource = _sslAnalyzer.RecentlyAnalyzed.Differences;
            });
        }

        private void runTimerTick(object sender, EventArgs e)
        {
            updateCurrentCheck();
            updateTimeElapsed();
            updateHostsChecked();
        }

        private void updateCurrentCheck()
        {
            if (_sslAnalyzer.Current != null)
            {
                string str = string.Format("> ({0}) {1}", 
                    _sslAnalyzer.Current.Protocol.ToLower(),
                    _sslAnalyzer.Current.URL);
                CurrentHostLabel.Text = str;
            }
        }

        private void updateTimeElapsed()
        {
            _dateTimeNow = _dateTimeNow.AddSeconds(1);
            DateTime est = new DateTime();
            est = est.AddSeconds(_sslAnalyzer.EstimateRuntime(_dateTimeNow));

            string elapsed = string.Format("Elapsed time: {0} / {1}",
                _dateTimeNow.ToString("HH:mm:ss"),
                est.ToString("HH:mm:ss"));

            ElapsedTimeLabel.Text = elapsed;
        }

        private void updateHostsChecked()
        {
            string hostsChecked = string.Format("{0}/{1} hosts analyzed",
                _sslAnalyzer.Done, _parserDelegator.ReadyRows);

            HostsCheckedLabel.Text = hostsChecked;
        }
    }
}
