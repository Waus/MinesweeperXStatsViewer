using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace MinesweeperXStatsViewer.Views
{
    public partial class SettingsWindow : Window
    {
        private static readonly Regex _digitsOnly = new Regex("^[0-9]+$");
        public SettingsWindow()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void Numeric_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_digitsOnly.IsMatch(e.Text);
        }

        private void Numeric_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                var txt = (string)e.DataObject.GetData(DataFormats.Text);
                if (!_digitsOnly.IsMatch(txt))
                    e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void Numeric_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
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
            BeginnerBox.Text = (Properties.Settings.Default.BegTimeFilterForIOE).ToString();
            IntermediateBox.Text = (Properties.Settings.Default.IntTimeFilterForIOE).ToString();
            ExpertBox.Text = (Properties.Settings.Default.ExpTimeFilterForIOE).ToString();
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
            Properties.Settings.Default.BegTimeFilterForIOE = BeginnerBox.Text;
            Properties.Settings.Default.IntTimeFilterForIOE = IntermediateBox.Text;
            Properties.Settings.Default.ExpTimeFilterForIOE = ExpertBox.Text;

            Properties.Settings.Default.Save();
            Close();
        }
    }
}
