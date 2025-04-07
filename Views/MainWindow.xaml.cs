using MinesweeperXStatsViewer.ViewModels;
using System.Windows;


namespace MinesweeperXStatsViewer.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private MainWindowViewModel vm;

    public MainWindow()
    {
        InitializeComponent();
        vm = new MainWindowViewModel();
        vm.RequestSettingsWindow += OnRequestSettingsWindow;
        DataContext = vm;
    }

    private void OnRequestSettingsWindow()
    {
        var settingsWindow = new InitialSettingsWindow { Owner = this };
        settingsWindow.ShowDialog();
    }


}