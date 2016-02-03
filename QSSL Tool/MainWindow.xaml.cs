using SSLLabsApiWrapper;
using SSLLabsApiWrapper.Models.Response;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace QSSLTool
{

    public partial class MainWindow : Window
    {
        private SSLLabsApiService _service;

        public MainWindow()
        {
            InitializeComponent();
            MainPageWindow.Title = getWindowTitle();

            _service = new SSLLabsApiService("https://api.ssllabs.com/api/v2");
            checkConnectionStatus();
            reloadSettings();
            //ThreadPool.QueueUserWorkItem(o => analyze());
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
                ConnectionDot.Fill = new SolidColorBrush(Color.FromArgb(255,90,209,8));
                ConnectionStatusText.Text = "Connected to API";
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

        private void analyze()
        {
            Analyze a = _service.AutomaticAnalyze("https://www.google.de", SSLLabsApiService.Publish.Off, SSLLabsApiService.StartNew.On,
                SSLLabsApiService.FromCache.Off, 1, SSLLabsApiService.All.On, SSLLabsApiService.IgnoreMismatch.Off, 200, 3);
            Debug.WriteLine(1);
        }
    }
}
