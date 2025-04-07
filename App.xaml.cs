using System.Text;
using System.Windows;

namespace MinesweeperXStatsViewer
{
    public partial class App : Application
    {
        public App()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }
}
