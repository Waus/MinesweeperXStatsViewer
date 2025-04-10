using System.Windows;

namespace MinesweeperXStatsViewer.Views
{
    public partial class InitialSettingsWindow : Window
    {
        public InitialSettingsWindow()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            JanuaryBox.Text = Properties.Settings.Default.January;
            FebruaryBox.Text = Properties.Settings.Default.February;
            MarchBox.Text = Properties.Settings.Default.March;
            AprilBox.Text = Properties.Settings.Default.April;
            MayBox.Text = Properties.Settings.Default.May;
            JuneBox.Text = Properties.Settings.Default.June;
            JulyBox.Text = Properties.Settings.Default.July;
            AugustBox.Text = Properties.Settings.Default.August;
            SeptemberBox.Text = Properties.Settings.Default.September;
            OctoberBox.Text = Properties.Settings.Default.October;
            NovemberBox.Text = Properties.Settings.Default.November;
            DecemberBox.Text = Properties.Settings.Default.December;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.January = JanuaryBox.Text;
            Properties.Settings.Default.February = FebruaryBox.Text;
            Properties.Settings.Default.March = MarchBox.Text;
            Properties.Settings.Default.April = AprilBox.Text;
            Properties.Settings.Default.May = MayBox.Text;
            Properties.Settings.Default.June = JuneBox.Text;
            Properties.Settings.Default.July = JulyBox.Text;
            Properties.Settings.Default.August = AugustBox.Text;
            Properties.Settings.Default.September = SeptemberBox.Text;
            Properties.Settings.Default.October = OctoberBox.Text;
            Properties.Settings.Default.November = NovemberBox.Text;
            Properties.Settings.Default.December = DecemberBox.Text;

            Properties.Settings.Default.Save();
            Close();
        }
    }
}
