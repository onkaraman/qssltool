using Microsoft.Win32;
using QSSLTool.Compacts;
using QSSLTool.FileWriters.Concretes;
using QSSLTool.Gateways;
using QSSLTool.Queries;
using SSLLabsApiWrapper;
using SSLLabsApiWrapper.Models.Response;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace QSSLTool
{

    public partial class MainWindow : Window
    {
        private bool _singleQueryStarted;
        private bool _massQueryStarted;
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

        private void setupViews()
        {
            URLField.KeyDown += URLFieldKeyDown;
            AnalyzeButton.Click += AnalyzeButtonClick;
            OpenFileButton.Click += OpenFileButtonClick;
            StartButton.Click += StartButtonClick;
            ExportExcelButton.Click += ExportExcelButtonClick;

            ElapsedTimeLabel.Text = "";
            HostsCheckedLabel.Text = "";
            CurrentHostLabel.Text = "";
        }

        private void prepareAnimations()
        {
            CurrentStatGrid.Opacity = 0;
            RecentOutcomeGrid.Opacity = 0;
            OptionsGrid.Opacity = 0;
        }

        private void setupSSLAnalyzer(HostEntryList hel)
        {
            _sslAnalyzer = new SSLAnalyzer(hel, _service);
            _sslAnalyzer.OnAnalyzeProgressed += OnAnalyzeProgressed;
            _sslAnalyzer.OnAnalyzeComplete += OnAnalyzeComplete;
            _sslAnalyzer.Start();
            ProgressBar.Visibility = Visibility.Visible;
        }


        private void OnAnalyzeComplete()
        {
            Dispatcher.Invoke(delegate ()
            {
                _singleQueryStarted = false;
                stopMassQuery();
                startAnimation("OptionsGrid_In");
            });
        }

        private void stopMassQuery()
        {
            _massQueryStarted = false;
            AnalyzeButton.IsEnabled = true;
            OpenFileButton.IsEnabled = true;
            URLField.IsEnabled = true;

            try
            {
                _runTimer.Stop();
                _runTimer = null;
                _sslAnalyzer.Stop();
                _sslAnalyzer.OnAnalyzeProgressed -= OnAnalyzeProgressed;
            }
            catch (Exception) { }

            URLField.Text = "https://";
            StartButton.Content = "Start";
            StartButton.Visibility = Visibility.Collapsed;
            ProgressBar.Visibility = Visibility.Collapsed;
            prepareAnimations();
        }

        private void setupRunTimer()
        {
            _runTimer = new DispatcherTimer();
            _runTimer.Interval = TimeSpan.FromSeconds(1);
            _runTimer.Tick += runTimerTick;
            _runTimer.Start();
        }

        private void OnAnalyzeProgressed()
        {
            Dispatcher.Invoke(delegate ()
            {
                if (RecentOutcomeGrid.Opacity == 0) startAnimation("RecentOutcomeGrid_In");
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
                    _sslAnalyzer.Current.Protocol.Content.ToLower(),
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
            string msg = "";
            if (!_singleQueryStarted)
            {
                msg = string.Format("{0}/{1} hosts analyzed",
                _sslAnalyzer.Done, _parserDelegator.ReadyRows);
            }
            else msg = "Single host analysis";
            string hostsChecked = msg;

            HostsCheckedLabel.Text = hostsChecked;
        }

        private void startAnimation(string name)
        {
            Storyboard sb = FindResource(name) as Storyboard;
            sb.Begin();
        }

        #region View events
        private void URLFieldKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                AnalyzeButtonClick(null, null);
            }
        }

        private void AnalyzeButtonClick(object sender, RoutedEventArgs e)
        {
            if (URLField.Text.StartsWith("https://") && URLField.Text.Length > 15)
            {
                _singleQueryStarted = true;
                AnalyzeButton.IsEnabled = false;
                OpenFileButton.IsEnabled = false;
                URLField.IsEnabled = false;

                string url = URLField.Text.Replace("https://", "");
                startAnimation("CurrentStatGrid_In");

                HostEntryList hel = new HostEntryList();
                HostEntry he = new HostEntry("", url, "https", "", "",
                    DateTime.Now, "", "");
                hel.Add(he);

                setupSSLAnalyzer(hel);

                _dateTimeNow = new DateTime();
                setupRunTimer();
            }
            else
            {
                MessageBox.Show("URL has to be a proper HTTPS address.", "QSSL Tool");
            }
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

                try
                {
                    _parserDelegator.Delegate(dia.FileName);
                }
                catch (Exception)
                {
                    MessageBox.Show("Couldn't open file. Is another process accessing it?",
                        "QSSL Tool");
                    stopMassQuery();
                }
            }
        }

        private void ParserDelegatorOnParseComplete()
        {
            Dispatcher.Invoke(delegate ()
            {
                ProgressBar.Visibility = Visibility.Collapsed;
                StartButton.Visibility = Visibility.Visible;

            });
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            if (!_massQueryStarted)
            {
                StartButton.Content = "Stop";
                startAnimation("CurrentStatGrid_In");
                setupSSLAnalyzer(_parserDelegator.GetHostEntries());

                _dateTimeNow = new DateTime();
                setupRunTimer();
                _massQueryStarted = true;
            }
            else stopMassQuery();
        }

        private void ExportExcelButtonClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dia = new SaveFileDialog();

            dia.Filter = "Excel files (*.xlsx)|*.xlsx";

            if (dia.ShowDialog() == true)
            {
                ExcelWriter writer = new ExcelWriter(_sslAnalyzer.AnalyzedEntries, dia.FileName);
                writer.Save();
                MessageBox.Show("Excel file has been exported.", "QSSL Tool");
            }
        }
        #endregion
    }
}
