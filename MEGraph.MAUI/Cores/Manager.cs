// START - 2.1.4 - ADD - Fix the issue where axes were lost when rendering multiple charts.
using MEGraph.MAUI.Axes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Cores
{
    public static class Manager
    {
        private static Dictionary<string, BaseChart> _baseChartManager { get; }
        static Manager()
        {
            _baseChartManager = new();
        }
        public static Dictionary<string, BaseChart> GetBCManager() => _baseChartManager;
        public static void AddChart(BaseChart chart)
        {
            if (chart == null) return;
            if (!_baseChartManager.ContainsKey(chart.Id))
            {
                _baseChartManager.Add(chart.Id, chart);
            }
        }
    }
}
// END - 2.1.4 - ADD - Fix the issue where axes were lost when rendering multiple charts.
