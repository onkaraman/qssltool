using SSLLabsApiWrapper;
using System;

namespace QSSLTool.Gateways
{
    [Serializable]
    public class AnalyzerSettings
    {
        public int WarningDays;
        public SSLLabsApiService.Publish Publish;
        public SSLLabsApiService.FromCache FromCache;
        public SSLLabsApiService.IgnoreMismatch IgnoreMismatch;

        public AnalyzerSettings()
        {
            WarningDays = 35;
            Publish = SSLLabsApiService.Publish.Off;
            FromCache = SSLLabsApiService.FromCache.Off;
            IgnoreMismatch = SSLLabsApiService.IgnoreMismatch.On;
        }

        public void SetPublish(bool? b)
        {
            if (b == true) Publish = SSLLabsApiService.Publish.On;
            else Publish = SSLLabsApiService.Publish.Off;
        }

        public void SetFromCache(bool? b)
        {
            if (b == true) FromCache = SSLLabsApiService.FromCache.On;
            else FromCache = SSLLabsApiService.FromCache.Off;
        }

        public void SetIgnoreMismatch(bool? b)
        {
            if (b == true) IgnoreMismatch = SSLLabsApiService.IgnoreMismatch.On;
            else IgnoreMismatch = SSLLabsApiService.IgnoreMismatch.Off;
        }
    }
}
