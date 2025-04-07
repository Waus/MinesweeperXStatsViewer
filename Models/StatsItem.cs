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
        public LevelEnum Level { get; set; }
        public double Time { get; set; }
        public int BBBV { get; set; }
        public double BBBVPerSecond { get; set; }
        public int Left { get; set; }
        public int Middle { get; set; }
        public int Right { get; set; }
        public DateTime DateTime { get; set; }

        public int Total => Left + Middle + Right;
        public double LeftPerSecond => Time != 0 ? Left / Time : 0;
        public double MiddlePerSecond => Time != 0 ? Middle / Time : 0;
        public double RightPerSecond => Time != 0 ? Right / Time : 0;
        public double TotalPerSecond => Time != 0 ? Total / Time : 0;
        public double RQP => BBBVPerSecond != 0 ? (Time + 1) / BBBVPerSecond : 1000;



    }
}
