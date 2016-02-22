using QSSLTool.Patterns;

namespace QSSLTool.Gateways
{
    public class ExportFilter : LazyStatic<ExportFilter>
    {
        private bool _areadyExpired;
        private int _expireTimeFrame;
        private string _rankingFilter;
        private string _expireTimeUnit;
        public string RankingFilter { get { return _rankingFilter; } }
        public bool AlreadyExpired { get { return _areadyExpired; } }
        public int ExpireTimeFrame { get { return _expireTimeFrame; } }
        public string ExpireTimeUnit { get { return _expireTimeUnit; } }

        public ExportFilter()
        {
            SetRankingOnly("*");
        }

        public void SetRankingOnly(string s)
        {
            if (s.StartsWith("*")) _rankingFilter = "*";
            else if (s.Contains("As")) _rankingFilter = "A";
            else if (s.Contains("Bs")) _rankingFilter = "B";
            else if (s.Contains("Cs")) _rankingFilter = "C";
            else if (s.Contains("Lower")) _rankingFilter = "-";
        }

        public void SetAlreadyExpired(bool set, 
            int timeframe = -1, string timespan = null)
        {
            if (set)
            {
                _areadyExpired = true;
            }
            else
            {
                _areadyExpired = false;
                _expireTimeFrame = timeframe;
                _expireTimeUnit = timespan;
            }
        }
    }
}
