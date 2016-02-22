using QSSLTool.Patterns;

namespace QSSLTool.Gateways
{
    public class ExportFilter : LazyStatic<ExportFilter>
    {
        private string _rankingFilter;
        public ExportFilter() { }

        public void SetRankingOnly(string s)
        {
            if (s.StartsWith(*)) _rankingFilter = "*";
            else if (s.Contains("As")) _rankingFilter = "A";
            else if (s.Contains("Bs")) _rankingFilter = "B";
            else if (s.Contains("Cs")) _rankingFilter = "C";
            else if (s.Contains("Lower")) _rankingFilter = "-";
        }
    }
}
