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
        private bool _fileLoaded = false;
        private ObservableCollection<StatsItem> _statsItems;
        private ObservableCollection<StatsItem> _allItems;
        private ObservableCollection<StatsMonthlyAggregate> _statsMonthlyAggregated;
        private ObservableCollection<StatsYearlyAggregate> _statsYearlyAggregated;
        private ViewModeEnum _currentView;
        private string _currentViewDisplay;

        private int? BegIOEFilter =>
        int.TryParse(Properties.Settings.Default.BegTimeFilterForIOE, out var v)
          ? v
          : null;

        private int? IntIOEFilter =>
        int.TryParse(Properties.Settings.Default.IntTimeFilterForIOE, out var v)
          ? v
          : null;

        private int? ExpIOEFilter =>
        int.TryParse(Properties.Settings.Default.ExpTimeFilterForIOE, out var v)
          ? v
          : null;

        public bool FileLoaded
        {
            get => _fileLoaded;
            set { _fileLoaded = value; }
        }

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

        public string CurrentViewDisplay
        {
            get => _currentViewDisplay;
            set { _currentViewDisplay = value; OnPropertyChanged(); }
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
            CurrentViewDisplay = "Please choose stats file";

            LoadFileCommand = new RelayCommand(LoadFile);
            OpenSettingsCommand = new RelayCommand(_ => RequestSettingsWindow?.Invoke());
            AllHistoryCommand = new RelayCommand(_ => FilterItems(x => true, ViewModeEnum.HistoryView, "All history"));
            AllHistoryFLCommand = new RelayCommand(_ => FilterItems(x => x.Right > 0, ViewModeEnum.HistoryView, "All FL history"));
            AllHistoryNFCommand = new RelayCommand(_ => FilterItems(x => x.Right == 0, ViewModeEnum.HistoryView, "All NF history"));
            ExpHistoryCommand = new RelayCommand(_ => FilterItems(x => x.Level == LevelEnum.Exp, ViewModeEnum.HistoryView, "Exp history"));
            ExpHistoryFLCommand = new RelayCommand(_ => FilterItems(x => x.Level == LevelEnum.Exp && x.Right > 0, ViewModeEnum.HistoryView, "Exp FL history"));
            ExpHistoryNFCommand = new RelayCommand(_ => FilterItems(x => x.Level == LevelEnum.Exp && x.Right == 0, ViewModeEnum.HistoryView, "Exp NF history"));
            IntHistoryCommand = new RelayCommand(_ => FilterItems(x => x.Level == LevelEnum.Int, ViewModeEnum.HistoryView, "Int history"));
            IntHistoryFLCommand = new RelayCommand(_ => FilterItems(x => x.Level == LevelEnum.Int && x.Right > 0, ViewModeEnum.HistoryView, "Int FL history"));
            IntHistoryNFCommand = new RelayCommand(_ => FilterItems(x => x.Level == LevelEnum.Int && x.Right == 0, ViewModeEnum.HistoryView, "Int NF history"));
            BegHistoryCommand = new RelayCommand(_ => FilterItems(x => x.Level == LevelEnum.Beg, ViewModeEnum.HistoryView, "Beg history"));
            BegHistoryFLCommand = new RelayCommand(_ => FilterItems(x => x.Level == LevelEnum.Beg && x.Right > 0, ViewModeEnum.HistoryView, "Beg FL history"));
            BegHistoryNFCommand = new RelayCommand(_ => FilterItems(x => x.Level == LevelEnum.Beg && x.Right == 0, ViewModeEnum.HistoryView, "Beg NF history"));
            ExpTopTimeCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Exp, x => x.Time, true, ViewModeEnum.TopTimeView, (item, rank) => item.TimeRank = rank, "Exp top time"));
            ExpTopTimeFLCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Exp && x.Right > 0, x => x.Time, true, ViewModeEnum.TopTimeView, (item, rank) => item.TimeRank = rank, "Exp FL top time"));
            ExpTopTimeNFCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Exp && x.Right == 0, x => x.Time, true, ViewModeEnum.TopTimeView, (item, rank) => item.TimeRank = rank, "Exp NF top time"));
            IntTopTimeCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Int, x => x.Time, true, ViewModeEnum.TopTimeView, (item, rank) => item.TimeRank = rank, "Int top time"));
            IntTopTimeFLCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Int && x.Right > 0, x => x.Time, true, ViewModeEnum.TopTimeView, (item, rank) => item.TimeRank = rank, "Int FL top time"));
            IntTopTimeNFCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Int && x.Right == 0, x => x.Time, true, ViewModeEnum.TopTimeView, (item, rank) => item.TimeRank = rank, "Int NF top time"));
            BegTopTimeCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Beg, x => x.Time, true, ViewModeEnum.TopTimeView, (item, rank) => item.TimeRank = rank, "Beg top time"));
            BegTopTimeFLCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Beg && x.Right > 0, x => x.Time, true, ViewModeEnum.TopTimeView, (item, rank) => item.TimeRank = rank, "Beg FL top time"));
            BegTopTimeNFCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Beg && x.Right == 0, x => x.Time, true, ViewModeEnum.TopTimeView, (item, rank) => item.TimeRank = rank, "Beg NF top time"));
            ExpTopBBBVPerSecCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Exp, x => x.BBBVPerSec, false, ViewModeEnum.TopBBBVPerSecView, (item, rank) => item.BBBVPerSecRank = rank, "Exp top 3BV/s"));
            ExpTopBBBVPerSecFLCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Exp && x.Right > 0, x => x.BBBVPerSec, false, ViewModeEnum.TopBBBVPerSecView, (item, rank) => item.BBBVPerSecRank = rank, "Exp FL top 3BV/s"));
            ExpTopBBBVPerSecNFCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Exp && x.Right == 0, x => x.BBBVPerSec, false, ViewModeEnum.TopBBBVPerSecView, (item, rank) => item.BBBVPerSecRank = rank, "Exp NF top 3BV/s"));
            IntTopBBBVPerSecCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Int, x => x.BBBVPerSec, false, ViewModeEnum.TopBBBVPerSecView, (item, rank) => item.BBBVPerSecRank = rank, "Int top 3BV/s"));
            IntTopBBBVPerSecFLCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Int && x.Right > 0, x => x.BBBVPerSec, false, ViewModeEnum.TopBBBVPerSecView, (item, rank) => item.BBBVPerSecRank = rank, "Int FL top 3BV/s"));
            IntTopBBBVPerSecNFCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Int && x.Right == 0, x => x.BBBVPerSec, false, ViewModeEnum.TopBBBVPerSecView, (item, rank) => item.BBBVPerSecRank = rank, "Int NF top 3BV/s"));
            BegTopBBBVPerSecCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Beg, x => x.BBBVPerSec, false, ViewModeEnum.TopBBBVPerSecView, (item, rank) => item.BBBVPerSecRank = rank, "Beg top 3BV/s"));
            BegTopBBBVPerSecFLCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Beg && x.Right > 0, x => x.BBBVPerSec, false, ViewModeEnum.TopBBBVPerSecView, (item, rank) => item.BBBVPerSecRank = rank, "Beg FL top 3BV/s"));
            BegTopBBBVPerSecNFCommand = new RelayCommand(_ => FilterAndSortItems(x => x.Level == LevelEnum.Beg && x.Right == 0, x => x.BBBVPerSec, false, ViewModeEnum.TopBBBVPerSecView, (item, rank) => item.BBBVPerSecRank = rank, "Beg NF top 3BV/s"));
            ExpMonthlyStatsCommand = new RelayCommand(_ => MonthlyStats(x => x.Level == LevelEnum.Exp && (ExpIOEFilter == null || x.Time < ExpIOEFilter), "Exp monthly stats"));
            ExpMonthlyStatsFLCommand = new RelayCommand(_ => MonthlyStats(x => x.Level == LevelEnum.Exp && (ExpIOEFilter == null || x.Time < ExpIOEFilter) && x.Right > 0, "Exp FL monthly stats"));
            ExpMonthlyStatsNFCommand = new RelayCommand(_ => MonthlyStats(x => x.Level == LevelEnum.Exp && (ExpIOEFilter == null || x.Time < ExpIOEFilter) && x.Right == 0, "Exp NF monthly stats"));
            IntMonthlyStatsCommand = new RelayCommand(_ => MonthlyStats(x => x.Level == LevelEnum.Int && (IntIOEFilter == null || x.Time < IntIOEFilter), "Int monthly stats"));
            IntMonthlyStatsFLCommand = new RelayCommand(_ => MonthlyStats(x => x.Level == LevelEnum.Int && (IntIOEFilter == null || x.Time < IntIOEFilter) && x.Right > 0, "Int FL monthly stats"));
            IntMonthlyStatsNFCommand = new RelayCommand(_ => MonthlyStats(x => x.Level == LevelEnum.Int && (IntIOEFilter == null || x.Time < IntIOEFilter) && x.Right == 0, "Int NF monthly stats"));
            BegMonthlyStatsCommand = new RelayCommand(_ => MonthlyStats(x => x.Level == LevelEnum.Beg && (BegIOEFilter == null || x.Time < BegIOEFilter), "Beg monthly stats"));
            BegMonthlyStatsFLCommand = new RelayCommand(_ => MonthlyStats(x => x.Level == LevelEnum.Beg && (BegIOEFilter == null || x.Time < BegIOEFilter) && x.Right > 0, "Beg FL monthly stats"));
            BegMonthlyStatsNFCommand = new RelayCommand(_ => MonthlyStats(x => x.Level == LevelEnum.Beg && (BegIOEFilter == null || x.Time < BegIOEFilter) && x.Right == 0, "Beg NF monthly stats"));
            ExpYearlyStatsCommand = new RelayCommand(_ => YearlyStats(x => x.Level == LevelEnum.Exp && (ExpIOEFilter == null || x.Time < ExpIOEFilter), "Exp yearly stats"));
            ExpYearlyStatsFLCommand = new RelayCommand(_ => YearlyStats(x => x.Level == LevelEnum.Exp && (ExpIOEFilter == null || x.Time < ExpIOEFilter) && x.Right > 0, "Exp FL yearly stats"));
            ExpYearlyStatsNFCommand = new RelayCommand(_ => YearlyStats(x => x.Level == LevelEnum.Exp && (ExpIOEFilter == null || x.Time < ExpIOEFilter) && x.Right == 0, "Exp NF yearly stats"));
            IntYearlyStatsCommand = new RelayCommand(_ => YearlyStats(x => x.Level == LevelEnum.Int && (IntIOEFilter == null || x.Time < IntIOEFilter), "Int yearly stats"));
            IntYearlyStatsFLCommand = new RelayCommand(_ => YearlyStats(x => x.Level == LevelEnum.Int && (IntIOEFilter == null || x.Time < IntIOEFilter) && x.Right > 0, "Int FL yearly stats"));
            IntYearlyStatsNFCommand = new RelayCommand(_ => YearlyStats(x => x.Level == LevelEnum.Int && (IntIOEFilter == null || x.Time < IntIOEFilter) && x.Right == 0, "Int NF yearly stats"));
            BegYearlyStatsCommand = new RelayCommand(_ => YearlyStats(x => x.Level == LevelEnum.Beg && (BegIOEFilter == null || x.Time < BegIOEFilter), "Beg yearly stats"));
            BegYearlyStatsFLCommand = new RelayCommand(_ => YearlyStats(x => x.Level == LevelEnum.Beg && (BegIOEFilter == null || x.Time < BegIOEFilter) && x.Right > 0, "Beg FL yearly stats"));
            BegYearlyStatsNFCommand = new RelayCommand(_ => YearlyStats(x => x.Level == LevelEnum.Beg && (BegIOEFilter == null || x.Time < BegIOEFilter) && x.Right == 0, "Beg NF yearly stats"));
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
                    SetViewDisplay("All levels history");
                    FileLoaded = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while loading file:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void FilterItems(Func<StatsItem, bool> predicate, ViewModeEnum view, string displayName)
        {
            if (!FileLoaded)
            {
                return;
            }
            CurrentView = view;
            SetViewDisplay(displayName);
            var filtered = _allItems.Where(predicate).ToList();
            StatsItems = new ObservableCollection<StatsItem>(filtered);
        }

        private void FilterAndSortItems(Func<StatsItem, bool> predicate,
                                Func<StatsItem, double> sortKey,
                                bool ascending,
                                ViewModeEnum view,
                                Action<StatsItem, int> rankAssigner,
                                string displayName)
        {
            if (!FileLoaded)
            {
                return;
            }
            CurrentView = view;
            SetViewDisplay(displayName);
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


        private void MonthlyStats(Func<StatsItem, bool> predicate, string displayName)
        {
            if (!FileLoaded)
            {
                return;
            }
            CurrentView = ViewModeEnum.MonthlyStatsView;
            SetViewDisplay(displayName);
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

        private void YearlyStats(Func<StatsItem, bool> predicate, string displayName)
        {
            if (!FileLoaded)
            {
                return;
            }
            CurrentView = ViewModeEnum.YearlyStatsView;
            SetViewDisplay(displayName);
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

        private void SetViewDisplay(string name) => CurrentViewDisplay = name;
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
