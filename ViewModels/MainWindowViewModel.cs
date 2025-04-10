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
        private ViewModeEnum _currentView;

        public ObservableCollection<StatsItem> StatsItems
        {
            get => _statsItems;
            set
            {
                _statsItems = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<StatsMonthlyAggregate> StatsMonthlyAggregated
        {
            get => _statsMonthlyAggregated;
            set
            {
                _statsMonthlyAggregated = value;
                OnPropertyChanged();
            }
        }

        public ViewModeEnum CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadFileCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand AllHistoryCommand { get; }
        public ICommand ExpHistoryCommand { get; }
        public ICommand IntHistoryCommand { get; }
        public ICommand BegHistoryCommand { get; }
        public ICommand ExpTopTimeCommand { get; }
        public ICommand IntTopTimeCommand { get; }
        public ICommand BegTopTimeCommand { get; }
        public ICommand ExpTopBBBVPerSecCommand { get; }
        public ICommand IntTopBBBVPerSecCommand { get; }
        public ICommand BegTopBBBVPerSecCommand { get; }
        public ICommand ExpMonthlyStatsCommand { get; }
        public ICommand IntMonthlyStatsCommand { get; }
        public ICommand BegMonthlyStatsCommand { get; }
        public event Action RequestSettingsWindow;

        public MainWindowViewModel()
        {
            _statsItems = new ObservableCollection<StatsItem>();
            _allItems = new ObservableCollection<StatsItem>();
            _statsMonthlyAggregated = new ObservableCollection<StatsMonthlyAggregate>();

            LoadFileCommand = new RelayCommand(LoadFile);
            OpenSettingsCommand = new RelayCommand(_ => RequestSettingsWindow?.Invoke());

            AllHistoryCommand = new RelayCommand(AllHistory);
            ExpHistoryCommand = new RelayCommand(ExpHistory);
            IntHistoryCommand = new RelayCommand(IntHistory);
            BegHistoryCommand = new RelayCommand(BegHistory);

            ExpTopTimeCommand = new RelayCommand(ExpTopTime);
            IntTopTimeCommand = new RelayCommand(IntTopTime);
            BegTopTimeCommand = new RelayCommand(BegTopTime);

            ExpTopBBBVPerSecCommand = new RelayCommand(ExpTopBBBVPerSec);
            IntTopBBBVPerSecCommand = new RelayCommand(IntTopBBBVPerSec);
            BegTopBBBVPerSecCommand = new RelayCommand(BegTopBBBVPerSec);

            ExpMonthlyStatsCommand = new RelayCommand(ExpMonthlyStats);
            IntMonthlyStatsCommand = new RelayCommand(IntMonthlyStats);
            BegMonthlyStatsCommand = new RelayCommand(BegMonthlyStats);

            CurrentView = ViewModeEnum.HistoryView;
        }

        private void LoadFile(object parameter)
        {
            var ofd = new OpenFileDialog { Filter = "Txt files|*.txt|All files|*.*" };
            if (ofd.ShowDialog() == true)
            {
                try
                {
                    var list = FileImportService.LoadStatsFromFile(ofd.FileName);
                    _allItems = new ObservableCollection<StatsItem>(list);
                    StatsItems = new ObservableCollection<StatsItem>(list);
                    CurrentView = ViewModeEnum.HistoryView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while loading file:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AllHistory(object parameter)
        {
            CurrentView = ViewModeEnum.HistoryView;
            StatsItems = new ObservableCollection<StatsItem>(_allItems);
        }

        private void ExpHistory(object parameter)
        {
            CurrentView = ViewModeEnum.HistoryView;
            var filtered = _allItems.Where(x => x.Level == LevelEnum.Exp);
            StatsItems = new ObservableCollection<StatsItem>(filtered);
        }

        private void IntHistory(object parameter)
        {
            CurrentView = ViewModeEnum.HistoryView;
            var filtered = _allItems.Where(x => x.Level == LevelEnum.Int);
            StatsItems = new ObservableCollection<StatsItem>(filtered);
        }

        private void BegHistory(object parameter)
        {
            CurrentView = ViewModeEnum.HistoryView;
            var filtered = _allItems.Where(x => x.Level == LevelEnum.Beg);
            StatsItems = new ObservableCollection<StatsItem>(filtered);
        }

        private void ExpTopTime(object parameter)
        {
            CurrentView = ViewModeEnum.TopTimeView;
            var filtered = _allItems.Where(x => x.Level == LevelEnum.Exp).OrderBy(x => x.TimeRank).ToList();
            StatsItems = new ObservableCollection<StatsItem>(filtered);
        }

        private void IntTopTime(object parameter)
        {
            CurrentView = ViewModeEnum.TopTimeView;
            var filtered = _allItems.Where(x => x.Level == LevelEnum.Int).OrderBy(x => x.TimeRank).ToList();
            StatsItems = new ObservableCollection<StatsItem>(filtered);
        }

        private void BegTopTime(object parameter)
        {
            CurrentView = ViewModeEnum.TopTimeView;
            var filtered = _allItems.Where(x => x.Level == LevelEnum.Beg).OrderBy(x => x.TimeRank).ToList();
            StatsItems = new ObservableCollection<StatsItem>(filtered);
        }

        private void ExpTopBBBVPerSec(object parameter)
        {
            CurrentView = ViewModeEnum.TopBBBVPerSecView;
            var filtered = _allItems.Where(x => x.Level == LevelEnum.Exp).OrderBy(x => x.BBBVPerSecRank).ToList();
            StatsItems = new ObservableCollection<StatsItem>(filtered);
        }

        private void IntTopBBBVPerSec(object parameter)
        {
            CurrentView = ViewModeEnum.TopBBBVPerSecView;
            var filtered = _allItems.Where(x => x.Level == LevelEnum.Int).OrderBy(x => x.BBBVPerSecRank).ToList();
            StatsItems = new ObservableCollection<StatsItem>(filtered);
        }

        private void BegTopBBBVPerSec(object parameter)
        {
            CurrentView = ViewModeEnum.TopBBBVPerSecView;
            var filtered = _allItems.Where(x => x.Level == LevelEnum.Beg).OrderBy(x => x.BBBVPerSecRank).ToList();
            StatsItems = new ObservableCollection<StatsItem>(filtered);
        }

        private void ExpMonthlyStats(object parameter)
        {
            CurrentView = ViewModeEnum.MonthlyStatsView;
            var filtered = _allItems.Where(x => x.Level == LevelEnum.Exp);
            var grouped = filtered
                .GroupBy(x => new { x.DateTime.Year, x.DateTime.Month })
                .Select(g => new StatsMonthlyAggregate
                {
                    Level = LevelEnum.Exp,
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    GamesWon = g.Count(),
                    AverageTime = g.Average(s => s.Time),
                    Average3BVPerSec = g.Average(s => s.BBBVPerSec),
                    BestTime = g.Min(s => s.Time),
                    Best3BVPerSec = g.Max(s => s.BBBVPerSec)
                })
                .OrderBy(a => a.Year)
                .ThenBy(a => a.Month)
                .ToList();
            StatsMonthlyAggregated = new ObservableCollection<StatsMonthlyAggregate>(grouped);
        }

        private void IntMonthlyStats(object parameter)
        {
            CurrentView = ViewModeEnum.MonthlyStatsView;
            var filtered = _allItems.Where(x => x.Level == LevelEnum.Int);
            var grouped = filtered
                .GroupBy(x => new { x.DateTime.Year, x.DateTime.Month })
                .Select(g => new StatsMonthlyAggregate
                {
                    Level = LevelEnum.Int,
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

        private void BegMonthlyStats(object parameter)
        {
            CurrentView = ViewModeEnum.MonthlyStatsView;
            var filtered = _allItems.Where(x => x.Level == LevelEnum.Beg);
            var grouped = filtered
                .GroupBy(x => new { x.DateTime.Year, x.DateTime.Month })
                .Select(g => new StatsMonthlyAggregate
                {
                    Level = LevelEnum.Beg,
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

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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
