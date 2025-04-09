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
                switch (mode)
                {
                    case ViewModeEnum.TopTimeView:
                        return TopTimeTemplate;
                    case ViewModeEnum.TopBBBVPerSecondView:
                        return TopBBBVPerSecondTemplate;
                    default:
                        return HistoryTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
