using Microsoft.Win32;
using MinesweeperXStatsViewer.Models;
using MinesweeperXStatsViewer.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace MinesweeperXStatsViewer.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<StatsItem> _statsItems;
        private ObservableCollection<StatsItem> _allItems;
        private ObservableCollection<StatsMonthlyAggregate> _statsMonthlyAggregated;
        private ObservableCollection<StatsYearlyAggregate> _statsYearlyAggregated;
        private ViewModeEnum _currentView;
        public ObservableCollection<StatsItem> StatsItems
        {
            get => _statsItems;
            set { _statsItems = value; OnPropertyChanged(); }
        }
        public ObservableCollection<StatsMonthlyAggregate> StatsMonthlyAggregated
        {
            get => _statsMonthlyAggregated;
            set { _statsMonthlyAggregated = value; OnPropertyChanged(); }
        }
        public ObservableCollection<StatsYearlyAggregate> StatsYearlyAggregated
        {
            get => _statsYearlyAggregated;
            set { _statsYearlyAggregated = value; OnPropertyChanged(); }
        }
        public ViewModeEnum CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }
        public ICommand LoadFileCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public event Action RequestSettingsWindow;
        public ICommand AllHistoryCommand { get; }
        public ICommand AllHistoryFLCommand { get; }
        public ICommand AllHistoryNFCommand { get; }
        public ICommand ExpHistoryCommand { get; }
        public ICommand ExpHistoryFLCommand { get; }
        public ICommand ExpHistoryNFCommand { get; }
        public ICommand IntHistoryCommand { get; }
        public ICommand IntHistoryFLCommand { get; }
        public ICommand IntHistoryNFCommand { get; }
        public ICommand BegHistoryCommand { get; }
        public ICommand BegHistoryFLCommand { get; }
        public ICommand BegHistoryNFCommand { get; }
        public ICommand ExpTopTimeCommand { get; }
        public ICommand ExpTopTimeFLCommand { get; }
        public ICommand ExpTopTimeNFCommand { get; }
        public ICommand IntTopTimeCommand { get; }
        public ICommand IntTopTimeFLCommand { get; }
        public ICommand IntTopTimeNFCommand { get; }
        public ICommand BegTopTimeCommand { get; }
        public ICommand BegTopTimeFLCommand { get; }
        public ICommand BegTopTimeNFCommand { get; }
        public ICommand ExpTopBBBVPerSecCommand { get; }
        public ICommand ExpTopBBBVPerSecFLCommand { get; }
        public ICommand ExpTopBBBVPerSecNFCommand { get; }
        public ICommand IntTopBBBVPerSecCommand { get; }
        public ICommand IntTopBBBVPerSecFLCommand { get; }
        public ICommand IntTopBBBVPerSecNFCommand { get; }
        public ICommand BegTopBBBVPerSecCommand { get; }
        public ICommand BegTopBBBVPerSecFLCommand { get; }
        public ICommand BegTopBBBVPerSecNFCommand { get; }
        public ICommand ExpMonthlyStatsCommand { get; }
        public ICommand ExpMonthlyStatsFLCommand { get; }
        public ICommand ExpMonthlyStatsNFCommand { get; }
        public ICommand IntMonthlyStatsCommand { get; }
        public ICommand IntMonthlyStatsFLCommand { get; }
        public ICommand IntMonthlyStatsNFCommand { get; }
        public ICommand BegMonthlyStatsCommand { get; }
        public ICommand BegMonthlyStatsFLCommand { get; }
        public ICommand BegMonthlyStatsNFCommand { get; }
        public ICommand ExpYearlyStatsCommand { get; }
        public ICommand ExpYearlyStatsFLCommand { get; }
        public ICommand ExpYearlyStatsNFCommand { get; }
        public ICommand IntYearlyStatsCommand { get; }
        public ICommand IntYearlyStatsFLCommand { get; }
        public ICommand IntYearlyStatsNFCommand { get; }
        public ICommand BegYearlyStatsCommand { get; }
        public ICommand BegYearlyStatsFLCommand { get; }
        public ICommand BegYearlyStatsNFCommand { get; }

        public MainWindowViewModel()
        {
            _statsItems = new ObservableCollection<StatsItem>();
            _allItems = new ObservableCollection<StatsItem>();
            _statsMonthlyAggregated = new ObservableCollection<StatsMonthlyAggregate>();
            CurrentView = ViewModeEnum.HistoryView;
            LoadFileCommand = new RelayCommand(LoadFile);
            OpenSettingsCommand = new RelayCommand(_ => RequestSettingsWindow?.Invoke());
            AllHistoryCommand = new RelayCommand(_ => FilterItems(x => true, ViewModeEnum.HistoryView));
            AllHistoryFLCommand = new RelayCommand(_ => FilterItems(x => x.Right > 0, ViewModeEnum.HistoryView));
            AllHistoryNFCommand = new RelayCommand(_ => FilterItems(x => x.Right == 0, ViewModeEnum.HistoryView));
            ExpHistoryCommand = new RelayCommand(_ => FilterItems(x => x.Level == LevelEnum.Exp, ViewModeEnum.HistoryView));
            ExpHistoryFLCommand = new RelayCommand(_ => FilterItems(x => x.Level == LevelEnum.Exp && x.Right > 0, ViewModeEnum.HistoryView));
            ExpHistoryNFCommand = new RelayCommand(_ => FilterItems(x => x.Level == LevelEnum.Exp && x.Right == 0, ViewModeEnum.HistoryView));
            IntHistoryCommand = new RelayCommand(_ => FilterItems(x => x.Level == LevelEnum.Int, ViewModeEnum.HistoryView));
            IntHistoryFLCommand = new RelayCommand(_ => FilterItems(x => x.Level == LevelEnum.Int && x.Right > 0, ViewModeEnum.HistoryView));
            IntHistoryNFCommand = new RelayCommand(_ => FilterItems(x => x.Level == LevelEnum.Int && x.Right == 0, ViewModeEnum.HistoryView));
            BegHistoryCommand = new RelayCommand(_ => FilterItems(x => x.Level == LevelEnum.Beg, ViewModeEnum.HistoryView));
            BegHistoryFLCommand = new RelayCommand(_ => FilterItems(x => x.Level == LevelEnum.Beg && x.Right > 0, ViewModeEnum.HistoryView));
            BegHistoryNFCommand = new RelayCommand(_ => FilterItems(x => x.Level == LevelEnum.Beg && x.Right == 0, ViewModeEnum.HistoryView));
            ExpTopTimeCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Exp, x => x.Time, true, ViewModeEnum.TopTimeView, (item, rank) => item.TimeRank = rank));
            ExpTopTimeFLCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Exp && x.Right > 0, x => x.Time, true, ViewModeEnum.TopTimeView, (item, rank) => item.TimeRank = rank));
            ExpTopTimeNFCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Exp && x.Right == 0, x => x.Time, true, ViewModeEnum.TopTimeView, (item, rank) => item.TimeRank = rank));
            IntTopTimeCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Int, x => x.Time, true, ViewModeEnum.TopTimeView, (item, rank) => item.TimeRank = rank));
            IntTopTimeFLCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Int && x.Right > 0, x => x.Time, true, ViewModeEnum.TopTimeView, (item, rank) => item.TimeRank = rank));
            IntTopTimeNFCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Int && x.Right == 0, x => x.Time, true, ViewModeEnum.TopTimeView, (item, rank) => item.TimeRank = rank));
            BegTopTimeCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Beg, x => x.Time, true, ViewModeEnum.TopTimeView, (item, rank) => item.TimeRank = rank));
            BegTopTimeFLCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Beg && x.Right > 0, x => x.Time, true, ViewModeEnum.TopTimeView, (item, rank) => item.TimeRank = rank));
            BegTopTimeNFCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Beg && x.Right == 0, x => x.Time, true, ViewModeEnum.TopTimeView, (item, rank) => item.TimeRank = rank));
            ExpTopBBBVPerSecCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Exp, x => x.BBBVPerSec, false, ViewModeEnum.TopBBBVPerSecView, (item, rank) => item.BBBVPerSecRank = rank));
            ExpTopBBBVPerSecFLCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Exp && x.Right > 0, x => x.BBBVPerSec, false, ViewModeEnum.TopBBBVPerSecView, (item, rank) => item.BBBVPerSecRank = rank));
            ExpTopBBBVPerSecNFCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Exp && x.Right == 0, x => x.BBBVPerSec, false, ViewModeEnum.TopBBBVPerSecView, (item, rank) => item.BBBVPerSecRank = rank));
            IntTopBBBVPerSecCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Int, x => x.BBBVPerSec, false, ViewModeEnum.TopBBBVPerSecView, (item, rank) => item.BBBVPerSecRank = rank));
            IntTopBBBVPerSecFLCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Int && x.Right > 0, x => x.BBBVPerSec, false, ViewModeEnum.TopBBBVPerSecView, (item, rank) => item.BBBVPerSecRank = rank));
            IntTopBBBVPerSecNFCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Int && x.Right == 0, x => x.BBBVPerSec, false, ViewModeEnum.TopBBBVPerSecView, (item, rank) => item.BBBVPerSecRank = rank));
            BegTopBBBVPerSecCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Beg, x => x.BBBVPerSec, false, ViewModeEnum.TopBBBVPerSecView, (item, rank) => item.BBBVPerSecRank = rank));
            BegTopBBBVPerSecFLCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Beg && x.Right > 0, x => x.BBBVPerSec, false, ViewModeEnum.TopBBBVPerSecView, (item, rank) => item.BBBVPerSecRank = rank));
            BegTopBBBVPerSecNFCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Beg && x.Right == 0, x => x.BBBVPerSec, false, ViewModeEnum.TopBBBVPerSecView, (item, rank) => item.BBBVPerSecRank = rank));
            ExpMonthlyStatsCommand = new RelayCommand(_ => MonthlyStats(x => x.Level == LevelEnum.Exp && x.Time < 60)); // Time < 60 to filter out games played for IOE
            ExpMonthlyStatsFLCommand = new RelayCommand(_ => MonthlyStats(x => x.Level == LevelEnum.Exp && x.Right > 0 && x.Time < 60)); // Time < 60 to filter out games played for IOE
            ExpMonthlyStatsNFCommand = new RelayCommand(_ => MonthlyStats(x => x.Level == LevelEnum.Exp && x.Right == 0 && x.Time < 60)); // Time < 60 to filter out games played for IOE
            IntMonthlyStatsCommand = new RelayCommand(_ => MonthlyStats(x => x.Level == LevelEnum.Int));
            IntMonthlyStatsFLCommand = new RelayCommand(_ => MonthlyStats(x => x.Level == LevelEnum.Int && x.Right > 0));
            IntMonthlyStatsNFCommand = new RelayCommand(_ => MonthlyStats(x => x.Level == LevelEnum.Int && x.Right == 0));
            BegMonthlyStatsCommand = new RelayCommand(_ => MonthlyStats(x => x.Level == LevelEnum.Beg));
            BegMonthlyStatsFLCommand = new RelayCommand(_ => MonthlyStats(x => x.Level == LevelEnum.Beg && x.Right > 0));
            BegMonthlyStatsNFCommand = new RelayCommand(_ => MonthlyStats(x => x.Level == LevelEnum.Beg && x.Right == 0));
            ExpYearlyStatsCommand = new RelayCommand(_ => YearlyStats(x => x.Level == LevelEnum.Exp && x.Time < 60)); // Time < 60 to filter out games played for IOE
            ExpYearlyStatsFLCommand = new RelayCommand(_ => YearlyStats(x => x.Level == LevelEnum.Exp && x.Right > 0 && x.Time < 60)); // Time < 60 to filter out games played for IOE
            ExpYearlyStatsNFCommand = new RelayCommand(_ => YearlyStats(x => x.Level == LevelEnum.Exp && x.Right == 0 && x.Time < 60)); // Time < 60 to filter out games played for IOE
            IntYearlyStatsCommand = new RelayCommand(_ => YearlyStats(x => x.Level == LevelEnum.Int));
            IntYearlyStatsFLCommand = new RelayCommand(_ => YearlyStats(x => x.Level == LevelEnum.Int && x.Right > 0));
            IntYearlyStatsNFCommand = new RelayCommand(_ => YearlyStats(x => x.Level == LevelEnum.Int && x.Right == 0));
            BegYearlyStatsCommand = new RelayCommand(_ => YearlyStats(x => x.Level == LevelEnum.Beg));
            BegYearlyStatsFLCommand = new RelayCommand(_ => YearlyStats(x => x.Level == LevelEnum.Beg && x.Right > 0));
            BegYearlyStatsNFCommand = new RelayCommand(_ => YearlyStats(x => x.Level == LevelEnum.Beg && x.Right == 0));
        }

        private void LoadFile(object parameter)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Txt files|*.txt|All files|*.*";
            if (ofd.ShowDialog() == true)
            {
                try
                {
                    var list = FileImportService.LoadStatsFromFile(ofd.FileName);
                    _allItems = new ObservableCollection<StatsItem>(list);
                    StatsItems = new ObservableCollection<StatsItem>(list);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while loading file:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void FilterItems(Func<StatsItem, bool> predicate, ViewModeEnum view)
        {
            CurrentView = view;
            var filtered = _allItems.Where(predicate).ToList();
            StatsItems = new ObservableCollection<StatsItem>(filtered);
        }

        private void FilterAndSortItems(Func<StatsItem, bool> predicate,
                                Func<StatsItem, double> sortKey,
                                bool ascending,
                                ViewModeEnum view,
                                Action<StatsItem, int> rankAssigner)
        {
            CurrentView = view;
            var filtered = ascending
                ? _allItems.Where(predicate).OrderBy(sortKey).ToList()
                : _allItems.Where(predicate).OrderByDescending(sortKey).ToList();
            if (filtered.Count > 0 && rankAssigner != null)
            {
                int rank = 0;
                int rankModifier = 1;
                double previous = sortKey(filtered[0]);
                rank += rankModifier;
                rankAssigner(filtered[0], rank);
                for (int i = 1; i < filtered.Count; i++)
                {
                    double current = sortKey(filtered[i]);
                    if (Math.Abs(current - previous) < 1e-6)
                    {
                        rankAssigner(filtered[i], rank);
                        rankModifier++;
                    }
                    else
                    {
                        rank += rankModifier;
                        rankAssigner(filtered[i], rank);
                        rankModifier = 1;
                        previous = current;
                    }
                }
            }
            StatsItems = new ObservableCollection<StatsItem>(filtered);
        }


        private void MonthlyStats(Func<StatsItem, bool> predicate)
        {
            CurrentView = ViewModeEnum.MonthlyStatsView;
            var filtered = _allItems.Where(predicate);
            var grouped = filtered
                .GroupBy(x => new { x.DateTime.Year, x.DateTime.Month })
                .Select(g => new StatsMonthlyAggregate
                {
                    Level = g.First().Level,
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    GamesWon = g.Count(),
                    BestTime = g.Min(s => s.Time),
                    AverageTime = g.Average(s => s.Time),
                    Best3BVPerSec = g.Max(s => s.BBBVPerSec),
                    Average3BVPerSec = g.Average(s => s.BBBVPerSec)
                })
                .OrderBy(a => a.Year)
                .ThenBy(a => a.Month)
                .ToList();
            StatsMonthlyAggregated = new ObservableCollection<StatsMonthlyAggregate>(grouped);
        }

        private void YearlyStats(Func<StatsItem, bool> predicate)
        {
            CurrentView = ViewModeEnum.YearlyStatsView;
            var filtered = _allItems.Where(predicate);
            var grouped = filtered
                .GroupBy(x => x.DateTime.Year)
                .Select(g => new StatsYearlyAggregate
                {
                    Level = g.First().Level,
                    Year = g.Key,
                    GamesWon = g.Count(),
                    BestTime = g.Min(s => s.Time),
                    AverageTime = g.Average(s => s.Time),
                    Best3BVPerSec = g.Max(s => s.BBBVPerSec),
                    Average3BVPerSec = g.Average(s => s.BBBVPerSec)
                })
                .OrderBy(a => a.Year)
                .ToList();
            StatsYearlyAggregated = new ObservableCollection<StatsYearlyAggregate>(grouped);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
