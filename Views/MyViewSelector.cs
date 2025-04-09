using MinesweeperXStatsViewer.Models;
using System.Windows;
using System.Windows.Controls;

namespace MinesweeperXStatsViewer.Views
{
    public class MyViewSelector : DataTemplateSelector
    {
        public DataTemplate HistoryTemplate { get; set; }
        public DataTemplate TopTimeTemplate { get; set; }
        public DataTemplate TopBBBVPerSecondTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ViewModeEnum mode)
            {
                if (mode == ViewModeEnum.TopTimeView) return TopTimeTemplate;
                if (mode == ViewModeEnum.TopBBBVPerSecondView) return TopBBBVPerSecondTemplate;
                return HistoryTemplate;
            }
            return base.SelectTemplate(item, container);
        }
    }
}