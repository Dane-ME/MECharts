// START - 2.1.4 - ADD - Fix the issue where axes were lost when rendering multiple charts.
// START - 2.4.0 - EDIT - Manager thread-safe với lock object.
using MEGraph.MAUI.Axes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MEGraph.MAUI.Cores
{
    public static class Manager
    {
        private static readonly object _lock = new();
        private static readonly Dictionary<string, BaseChart> _baseChartManager = new();

        public static Dictionary<string, BaseChart> GetBCManager()
        {
            lock (_lock)
            {
                // Trả về bản sao tránh race condition khi caller iterate
                return new Dictionary<string, BaseChart>(_baseChartManager);
            }
        }

        public static void AddChart(BaseChart chart)
        {
            if (chart == null) return;
            lock (_lock)
            {
                if (!_baseChartManager.ContainsKey(chart.Id))
                    _baseChartManager.Add(chart.Id, chart);
            }
        }

        public static void RemoveChart(string id)
        {
            if (string.IsNullOrEmpty(id)) return;
            lock (_lock)
            {
                _baseChartManager.Remove(id);
            }
        }

        public static bool TryGetChart(string id, out BaseChart chart)
        {
            lock (_lock)
            {
                return _baseChartManager.TryGetValue(id, out chart);
            }
        }
    }
}
// END - 2.4.0 - EDIT - Manager thread-safe với lock object.
// END - 2.1.4 - ADD - Fix the issue where axes were lost when rendering multiple charts.
