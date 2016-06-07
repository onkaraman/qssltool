using QSSLTool.Compacts;
using QSSLTool.Patterns;
using System.Collections.Generic;

namespace QSSLTool.Gateways
{
    /// <summary>
    /// This class serves as a central hub to save and use
    /// filter settings for the app scope.
    /// </summary>
    public class ExportFilter : LazyStatic<ExportFilter>
    {
        public bool AlreadyExpired;
        public bool WarningExpired;
        private string _rankingFilter;
        public string RankingFilter { get { return _rankingFilter; } }
        private int[] _gradeCount;
        public int [] GradeCount { get { return _gradeCount; } }

        public ExportFilter()
        {
            _gradeCount = new int[] { 0, 0, 0, 0 };
            SetRankingOnly("*");
        }

        /// <summary>
        /// Will take a list of anaylzed entries and extract the count
        /// of each ranking.
        /// </summary>
        /// <param name="analyzed">List of analyzed entries.</param>
        public void EnumerateGrades(List<HostEntry> analyzed)
        {
            int a = 0;
            int b = 0;
            int c = 0;
            int lower = 0;
            int everything = 0;

            foreach (HostEntry he in analyzed)
            {
                if (he.Ranking.ToString().Contains("A")
                    && !he.Ranking.ToString().Contains("failed")) a += 1;
                else if (he.Ranking.ToString().Contains("B")) b += 1;
                else if (he.Ranking.ToString().Contains("C")) c += 1;
                else lower += 1;
            }

            everything = a + b + c + lower;
            _gradeCount = new int[] { a, b, c, everything, lower };
        }

        /// <summary>
        /// Will set the ranking filter for the file to be exported.
        /// </summary>
        /// <param name="s"></param>
        public void SetRankingOnly(string s)
        {
            if (s.StartsWith("*")) _rankingFilter = "*";
            else if (s.Contains("As")) _rankingFilter = "A";
            else if (s.Contains("Bs")) _rankingFilter = "B";
            else if (s.Contains("Cs")) _rankingFilter = "C";
            else if (s.Contains("Lower")) _rankingFilter = "-";
        }

    }
}
