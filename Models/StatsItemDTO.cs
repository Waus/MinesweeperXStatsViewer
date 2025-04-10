using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperXStatsViewer.Models
{
    public class StatsItemDTO
    {
        public string Level { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Mines { get; set; }
        public double Time { get; set; }
        public int BBBV { get; set; }
        public int Solved { get; set; }
        public double BBBVPerSec { get; set; }
        public double Est { get; set; }
        public int Left { get; set; }
        public int Middle { get; set; }
        public int Right { get; set; }
        public string Date { get; set; }
        public string Started { get; set; }
    }
}
