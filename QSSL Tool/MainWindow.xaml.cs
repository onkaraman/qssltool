using SSLLabsApiWrapper;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace QSSL_Tool
{

    public partial class MainWindow : Window
    {
        private SSLLabsApiService _service;
        public MainWindow()
        {
            InitializeComponent();
            _service = new SSLLabsApiService("https://api.ssllabs.com/api/v2");
            ThreadPool.QueueUserWorkItem(o => analyze());
        }

        private void analyze()
        {
            SSLLabsApiWrapper.Models.Response.Analyze a = _service.AutomaticAnalyze("https://www.google.de", SSLLabsApiService.Publish.Off, SSLLabsApiService.StartNew.On,
                SSLLabsApiService.FromCache.Off, 1, SSLLabsApiService.All.On, SSLLabsApiService.IgnoreMismatch.Off, 200, 3);
            Debug.WriteLine(1);
        }
    }
}
