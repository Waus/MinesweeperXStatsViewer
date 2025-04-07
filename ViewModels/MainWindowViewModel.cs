using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Win32;
using MinesweeperXStatsViewer.Models;
using MinesweeperXStatsViewer.Services;
using System.Linq;
using System.Windows;

namespace MinesweeperXStatsViewer.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ICommand OpenSettingsCommand { get; }

        public event Action RequestSettingsWindow;


        private ObservableCollection<StatsItem> _statsItems;
        private ObservableCollection<StatsItem> _allItems; // Pełna lista, żeby mieć co filtrować

        public ObservableCollection<StatsItem> StatsItems
        {
            get => _statsItems;
            set
            {
                _statsItems = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadFileCommand { get; }
        public ICommand FilterBegCommand { get; }
        public ICommand FilterIntCommand { get; }
        public ICommand FilterExpCommand { get; }

        public MainWindowViewModel()
        {
            _statsItems = new ObservableCollection<StatsItem>();
            _allItems = new ObservableCollection<StatsItem>();

            LoadFileCommand = new RelayCommand(LoadFile);
            FilterBegCommand = new RelayCommand(FilterBeg);
            FilterIntCommand = new RelayCommand(FilterInt);
            FilterExpCommand = new RelayCommand(FilterExp);

            OpenSettingsCommand = new RelayCommand(_ => RequestSettingsWindow?.Invoke());
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
                    System.Windows.MessageBox.Show($"Error while loading file:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void FilterBeg(object parameter)
        {
            var filtered = _allItems.Where(x => x.Level == LevelEnum.Beg);
            StatsItems = new ObservableCollection<StatsItem>(filtered);
        }

        private void FilterInt(object parameter)
        {
            var filtered = _allItems.Where(x => x.Level == LevelEnum.Int);
            StatsItems = new ObservableCollection<StatsItem>(filtered);
        }

        private void FilterExp(object parameter)
        {
            var filtered = _allItems.Where(x => x.Level == LevelEnum.Exp);
            StatsItems = new ObservableCollection<StatsItem>(filtered);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }

    // Prosta implementacja ICommand do obsługi komend
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

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
