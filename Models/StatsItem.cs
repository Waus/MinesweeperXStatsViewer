using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;

namespace MinesweeperXStatsViewer.Models
{
    public class StatsItem
    {
        public int? TimeRank { get; set; }
        public int? BBBVPerSecRank { get; set; }
        public LevelEnum Level { get; set; }
        public double Time { get; set; }
        public int BBBV { get; set; }
        public double BBBVPerSec { get; set; }
        public int Left { get; set; }
        public int Middle { get; set; }
        public int Right { get; set; }
        public DateTime DateTime { get; set; }

        public int Total => Left + Middle + Right;
        public double LeftPerSec => Time != 0 ? Left / Time : 0;
        public double MiddlePerSec => Time != 0 ? Middle / Time : 0;
        public double RightPerSec => Time != 0 ? Right / Time : 0;
        public double TotalPerSec => Time != 0 ? Total / Time : 0;
        public double RQP => BBBVPerSec != 0 ? (Time + 1) / BBBVPerSec : 1000;



    }
}
