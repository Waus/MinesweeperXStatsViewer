using MinesweeperXStatsViewer.Models;
using System.Windows;
using System.Windows.Controls;

namespace MinesweeperXStatsViewer.Views
{
    public class MyViewSelector : DataTemplateSelector
    {
        public DataTemplate HistoryTemplate { get; set; }
        public DataTemplate TopTimeTemplate { get; set; }
        public DataTemplate TopBBBVPerSecTemplate { get; set; }
        public DataTemplate MonthlyStatsTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ViewModeEnum mode)
            {
                switch (mode)
                {
                    case ViewModeEnum.HistoryView:
                        return HistoryTemplate;
                    case ViewModeEnum.TopTimeView:
                        return TopTimeTemplate;
                    case ViewModeEnum.TopBBBVPerSecView:
                        return TopBBBVPerSecTemplate;
                    case ViewModeEnum.MonthlyStatsView:
                        return MonthlyStatsTemplate;
                    default:
                        return HistoryTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
