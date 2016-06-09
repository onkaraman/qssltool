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
        /// <summary>
        /// Will be true when the user wants only expired hosts
        /// to be exported.
        /// </summary>
        public bool AlreadyExpired;
        /// <summary>
        /// Will be true when the users only hosts to be exported
        /// which will expire soon, according to settings.
        /// </summary>
        public bool WarningExpired;
        private string _rankingFilter;
        /// <summary>
        /// Will contain the grade/ranking the user wants the hosts to be filtered by.
        /// </summary>
        public string RankingFilter { get { return _rankingFilter; } }
        private int[] _gradeCount;
        /// <summary>
        /// Contains the count of each grade of the analyzed hosts.
        /// [0] = a, [1] = b, [2] = c, [3] = total, [4], lower than c.
        /// </summary>
        public int [] GradeCount { get { return _gradeCount; } }
        private int _expiredCount;
        /// <summary>
        /// Will return the count of hosts which are already expired.
        /// </summary>
        public int ExpiredCount { get { return _expiredCount; } }
        private int _warningCount;
        /// <summary>
        /// Will return the count of hosts which will expire soon according to settings.
        /// </summary>
        public int WarningCount { get { return _warningCount; } }        

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

                if (he.Expired) _expiredCount += 1;
                if (he.WarningExpired) _warningCount += 1;
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
