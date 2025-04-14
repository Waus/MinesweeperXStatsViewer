using System;
using MinesweeperXStatsViewer.Models;

namespace MinesweeperXStatsViewer.Models
{
    public class StatsYearlyAggregate
    {
        public LevelEnum Level { get; set; }
        public int Year { get; set; }
        public int GamesWon { get; set; }
        public double AverageTime { get; set; }
        public double Average3BVPerSec { get; set; }
        public double BestTime { get; set; }
        public double Best3BVPerSec { get; set; }
    }
}
