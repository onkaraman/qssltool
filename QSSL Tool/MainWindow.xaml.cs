using System;
using Microsoft.Win32;
using QSSLTool.Compacts;
using QSSLTool.FileWriters.Concretes;
using QSSLTool.Gateways;
using QSSLTool.Queries;
using SSLLabsApiWrapper;
using SSLLabsApiWrapper.Models.Response;
using System.Collections.Generic;
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
            this.Closed += MainWindowClosed;
            MainPageWindow.Title = getWindowTitle();

            _service = new SSLLabsApiService("https://api.ssllabs.com/api/v2");
            _parserDelegator = new ParserDelegator();
            ParserDelegator.OnParseComplete += ParserDelegatorOnParseComplete;

            checkConnectionStatus();
            reloadSettings();
            setupViews();
            assignEvents();
            prepareAnimations();

        }

        /// <summary>
        /// Applies the version number for the window title to make remote debugging easier.
        /// </summary>
        private string getWindowTitle()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            return string.Format("QSSL Tool (Version {0})", version.ToString());
        }

        /// <summary>
        /// Check if a connection to the API service is possible.
        /// If not, the app won't be usable.
        /// </summary>
        private void checkConnectionStatus()
        {
            Info inf = _service.Info();
            if (inf.Online)
            {
                ConnectionDot.Fill = new SolidColorBrush(Color.FromArgb(255, 90, 209, 8));
                ConnectionStatusText.Text = Properties.Resources.connectedToAPI;
            }
            else
            {
                ConnectionDot.Fill = new SolidColorBrush(Color.FromArgb(255, 209, 17, 8));
                ConnectionStatusText.Text = Properties.Resources.notConnectedToAPI;
                AnalyzeButton.Visibility = Visibility.Collapsed;
                OpenFileButton.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// To restore the latest session.
        /// </summary>
        private void reloadSettings()
        {
            URLField.Text = "https://";
        }

        /// <summary>
        /// Will prepare the UI.
        /// </summary>
        private void setupViews()
        {
            ElapsedTimeLabel.Text = "";
            HostsCheckedLabel.Text = "";
            CurrentHostLabel.Text = "";
        }

        /// <summary>
        /// Will assign events for the UI elements.
        /// </summary>
        private void assignEvents()
        {
            URLField.KeyDown += URLFieldKeyDown;
            AnalyzeButton.Click += AnalyzeButtonClick;
            OpenFileButton.Click += OpenFileButtonClick;
            StartButton.Click += StartButtonClick;
            ExportExcelButton.Click += ExportExcelButtonClick;
            FiltersButton.Click += FiltersButtonClick;
            SettingsLabel.MouseEnter += SettingsLabelMouseEnter;
            SettingsLabel.MouseLeave += SettingsLabelMouseLeave;
            SettingsLabel.MouseUp += SettingsLabelMouseUp;
        }

        /// <summary>
        /// Will prepare the UI for animations by setting opacities
        /// of the current stat, recent outcome and options grid to zero.
        /// </summary>
        private void prepareAnimations()
        {
            CurrentStatGrid.Opacity = 0;
            RecentOutcomeGrid.Opacity = 0;
            OptionsGrid.Opacity = 0;
            FiltersButton.IsEnabled = false;
            ExportExcelButton.IsEnabled = false;
        }

        private void setupSSLAnalyzer(List<HostEntry> hel)
        {
            _sslAnalyzer = new SSLAnalyzer(hel, _service);
            _sslAnalyzer.OnAnalyzeProgressed += OnAnalyzeProgressed;
            _sslAnalyzer.OnAnalyzeComplete += OnAnalyzeComplete;
            _sslAnalyzer.Start();
            ProgressBar.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Resets the whole app to make a fresh search possible.
        /// </summary>
        private void stopMassQuery()
        {
            _massQueryStarted = false;
            AnalyzeButton.IsEnabled = true;
            OpenFileButton.IsEnabled = true;
            URLField.IsEnabled = true;
            URLField.Text = "https://";
            StartButton.Content = Properties.Resources.start;
            StartButton.Visibility = Visibility.Collapsed;
            ProgressBar.Visibility = Visibility.Collapsed;
            prepareAnimations();

            try
            {
                _runTimer.Stop();
                _runTimer = null;
                _sslAnalyzer.Stop();
                _sslAnalyzer.OnAnalyzeProgressed -= OnAnalyzeProgressed;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Will prepare and start the analysis timer, which will
        /// show progress of the ongoing analysis.
        /// </summary>
        private void setupRunTimer()
        {
            _runTimer = new DispatcherTimer();
            _runTimer.Interval = TimeSpan.FromSeconds(1);
            _runTimer.Tick += runTimerTick;
            _runTimer.Start();
        }

        /// <summary>
        /// After a single item of a mass analysis has been progressed,
        /// this method shows the result in a listbox and starts the
        /// animation if it hasn't been started before.
        /// </summary>
        private void OnAnalyzeProgressed()
        {
            Dispatcher.Invoke(delegate ()
            {
                if (RecentOutcomeGrid.Opacity == 0) startAnimation("RecentOutcomeGrid_In");
                RecentOutcomeLabel.Text = string.Format(Properties.Resources.recentOutcome,
                        _sslAnalyzer.RecentlyAnalyzed.URL);
                DifferenceListBox.ItemsSource = _sslAnalyzer.RecentlyAnalyzed.Differences;
                ExportFilter.Static.EnumerateGrades(_sslAnalyzer.AnalyzedEntries);
            });
        }

        /// <summary>
        /// This method will be called once an analysis is complete.
        /// An animation will be triggered.
        /// </summary>
        private void OnAnalyzeComplete()
        {
            Dispatcher.Invoke(delegate ()
            {
                _singleQueryStarted = false;
                stopMassQuery();

                startAnimation("OptionsGrid_In");
                if (_sslAnalyzer.AnalyzedEntries.Count <= 0)
                {
                    ExportExcelButton.IsEnabled = false;
                    ExportExcelButton.Content = Properties.Resources.errorsOccurred;
                }
                else
                {
                    ExportExcelButton.IsEnabled = true;
                    FiltersButton.IsEnabled = true;
                    ExportExcelButton.Content = Properties.Resources.export;
                }
            });
        }

        private void runTimerTick(object sender, EventArgs e)
        {
            updateCurrentCheck();
            updateTimeElapsed();
            updateHostsChecked();
        }

        /// <summary>
        /// Updates the current host label after an item has been progressed.
        /// </summary>
        private void updateCurrentCheck()
        {
            if (_sslAnalyzer.Current != null)
            {
                string str = string.Format("> ({0}) {1}", 
                    _sslAnalyzer.Current.Protocol.ToString().ToLower(),
                    _sslAnalyzer.Current.URL);
                CurrentHostLabel.Text = str;
            }
        }

        /// <summary>
        /// Updates the timer each second during an analysis.
        /// </summary>
        private void updateTimeElapsed()
        {
            _dateTimeNow = _dateTimeNow.AddSeconds(1);
            DateTime est = new DateTime();
            est = est.AddSeconds(_sslAnalyzer.EstimateRuntime(_dateTimeNow));

            string elapsed = string.Format(Properties.Resources.elapsedTime,
                _dateTimeNow.ToString("HH:mm:ss"),
                est.ToString("HH:mm:ss"));

            ElapsedTimeLabel.Text = elapsed;
        }

        /// <summary>
        /// Updates the checked items of a mass analysis.
        /// If it is a single anylsis it will be shown as such.
        /// </summary>
        private void updateHostsChecked()
        {
            string msg = "";
            if (!_singleQueryStarted)
            {
                msg = string.Format(Properties.Resources.hostsAnalyzed,
                _sslAnalyzer.Done, _parserDelegator.GetHostEntries().Count+1);
            }
            else msg = Properties.Resources.singleAnalysis;
            string hostsChecked = msg;

            HostsCheckedLabel.Text = hostsChecked;
        }

        /// <summary>
        /// Starts a storyboard animation which will be found by its name.
        /// </summary>
        private void startAnimation(string name)
        {
            Storyboard sb = FindResource(name) as Storyboard;
            sb.Begin();
        }

        #region View events
        private void SettingsLabelMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SettingsLabel.TextDecorations = null;
        }

        private void SettingsLabelMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SettingsLabel.TextDecorations = TextDecorations.Underline;
        }

        /// <summary>
        /// Íf the user pressed the enter-key, the single analysis will be started.
        /// </summary>
        private void URLFieldKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                AnalyzeButtonClick(null, null);
            }
        }

        /// <summary>
        /// Before a single analysis starts, this method will check whether
        /// the URL is properly written (http is pointless for this cause).
        /// A single analysis will be treated internally as a mass query with a single entry.
        /// </summary>
        private void AnalyzeButtonClick(object sender, RoutedEventArgs e)
        {
            if (URLField.Text.StartsWith("https://") && URLField.Text.Length > 12)
            {
                _singleQueryStarted = true;
                AnalyzeButton.IsEnabled = false;
                OpenFileButton.IsEnabled = false;
                URLField.IsEnabled = false;

                string url = URLField.Text.Replace("https://", "");
                startAnimation("CurrentStatGrid_In");

                List<HostEntry> hel = new List<HostEntry>();
                HostEntry he = new HostEntry(url, "https");
                hel.Add(he);

                setupSSLAnalyzer(hel);

                _dateTimeNow = new DateTime();
                setupRunTimer();
            }
            else
            {
                MessageBox.Show(Properties.Resources.urlMustBeHttps, "QSSL Tool");
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
                prepareAnimations();
                AnalyzeButton.IsEnabled = false;
                OpenFileButton.IsEnabled = false;
                URLField.IsEnabled = false;
                ProgressBar.Visibility = Visibility.Visible;
                URLField.Text = dia.FileName;

                try
                {
                    _parserDelegator.Delegate(dia.FileName);
                    OptionsGrid.Opacity = 0;
                }
                catch (Exception)
                {
                    MessageBox.Show(Properties.Resources.couldntOpenFile, 
                        "QSSL Tool");
                    stopMassQuery();
                }
            }
        }

        /// <summary>
        /// When parsing the opened file is complete.
        /// </summary>
        private void ParserDelegatorOnParseComplete()
        {
            Dispatcher.Invoke(delegate ()
            {
                ProgressBar.Visibility = Visibility.Collapsed;
                StartButton.Visibility = Visibility.Visible;
            });
        }

        /// <summary>
        /// After the mass file has been parsed, the process can be started by pressing the 
        /// start button. The SSL Analyzer will pull the data from the parser and start working.
        /// </summary>
        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            if (!_massQueryStarted)
            {
                OptionsGrid.Opacity = 0;
                StartButton.Content = "Stop";
                startAnimation("CurrentStatGrid_In");
                setupSSLAnalyzer(_parserDelegator.GetHostEntries());

                _dateTimeNow = new DateTime();
                setupRunTimer();
                _massQueryStarted = true;
            }
            else
            {
                stopMassQuery();
                OnAnalyzeComplete();
            }
        }

        private void ExportExcelButtonClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dia = new SaveFileDialog();
            dia.Filter = "Excel files (*.xlsx)|*.xlsx";

            if (dia.ShowDialog() == true)
            {
                ExcelWriter writer = new ExcelWriter(_sslAnalyzer.AnalyzedEntries, dia.FileName);
                writer.Save();
                MessageBox.Show(writer.GetMessage(), "QSSL Tool");
            }
        }

        private void FiltersButtonClick(object sender, RoutedEventArgs e)
        {
            FiltersWindow fw = new FiltersWindow();
            fw.Show();
        }

        private void SettingsLabelMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_singleQueryStarted || _massQueryStarted)
            {
                MessageBox.Show(Properties.Resources.cannotEnterSettings, 
                    "QSSL Tool");
            }
            else
            {
                SettingsWindow sw = new SettingsWindow();
                sw.Show();
            }
        }
        #endregion

        private void MainWindowClosed(object sender, EventArgs e)
        {
            Settings.Static.Save();
            Application.Current.Shutdown();
        }
    }
}
