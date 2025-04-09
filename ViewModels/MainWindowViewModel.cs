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

        public ObservableCollection<StatsItem> StatsItems
        {
            get => _statsItems;
            set
            {
                _statsItems = value;
                OnPropertyChanged();
            }
        }

        private ViewModeEnum _currentView = ViewModeEnum.HistoryView;
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
        public ICommand ExpTopBBBVPerSecondCommand { get; }
        public ICommand IntTopBBBVPerSecondCommand { get; }
        public ICommand BegTopBBBVPerSecondCommand { get; }

        public event Action RequestSettingsWindow;

        public MainWindowViewModel()
        {
            _statsItems = new ObservableCollection<StatsItem>();
            _allItems = new ObservableCollection<StatsItem>();

            LoadFileCommand = new RelayCommand(LoadFile);
            OpenSettingsCommand = new RelayCommand(_ => RequestSettingsWindow?.Invoke());

            AllHistoryCommand = new RelayCommand(AllHistory);
            ExpHistoryCommand = new RelayCommand(ExpHistory);
            IntHistoryCommand = new RelayCommand(IntHistory);
            BegHistoryCommand = new RelayCommand(BegHistory);

            ExpTopTimeCommand = new RelayCommand(ExpTopTime);
            IntTopTimeCommand = new RelayCommand(IntTopTime);
            BegTopTimeCommand = new RelayCommand(BegTopTime);

            ExpTopBBBVPerSecondCommand = new RelayCommand(ExpTopBBBVPerSecond);
            IntTopBBBVPerSecondCommand = new RelayCommand(IntTopBBBVPerSecond);
            BegTopBBBVPerSecondCommand = new RelayCommand(BegTopBBBVPerSecond);
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

        private void ExpTopBBBVPerSecond(object parameter)
        {
            CurrentView = ViewModeEnum.TopBBBVPerSecondView;
            var filtered = _allItems.Where(x => x.Level == LevelEnum.Exp).OrderBy(x => x.BBBVPerSecondRank).ToList();
            StatsItems = new ObservableCollection<StatsItem>(filtered);
        }

        private void IntTopBBBVPerSecond(object parameter)
        {
            CurrentView = ViewModeEnum.TopBBBVPerSecondView;
            var filtered = _allItems.Where(x => x.Level == LevelEnum.Int).OrderBy(x => x.BBBVPerSecondRank).ToList();
        }

        private void BegTopBBBVPerSecond(object parameter)
        {
            CurrentView = ViewModeEnum.TopBBBVPerSecondView;
            var filtered = _allItems.Where(x => x.Level == LevelEnum.Beg).OrderBy(x => x.BBBVPerSecondRank).ToList();
            StatsItems = new ObservableCollection<StatsItem>(filtered);
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
