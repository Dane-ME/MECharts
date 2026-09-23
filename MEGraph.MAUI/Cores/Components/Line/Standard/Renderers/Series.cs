// START - 2.4.0 - EDIT - Fix duplicate min/max: dùng GetMinY/GetMaxY từ series thay vì tính lại.
using MEGraph.MAUI.Series;
using MEGraph.MAUI.Series.Line;
using System.Collections.Generic;
using System.Linq;

namespace MEGraph.MAUI.Cores.Components.Line.Standard.Renderers
{
    public class Series
    {
        public void Draw(ICanvas canvas, RectF plotArea, BaseChart? baseChart)
        {
            if (baseChart == null) return;

            var allLineSeries = baseChart.Series.OfType<LineSeries>().ToList();

            // Tính globalMin/Max một lần từ series — tránh mỗi series tự tính riêng
            float? globalMinY = null;
            float? globalMaxY = null;

            if (allLineSeries.Any())
            {
                globalMinY = allLineSeries.Min(s => s.GetMinY());
                globalMaxY = allLineSeries.Max(s => s.GetMaxY());
            }

            foreach (var series in baseChart.Series)
            {
                if (series is LineSeries lineSeries)
                    lineSeries.Draw(canvas, plotArea, globalMinY, globalMaxY, baseChart.AnimationProgress);
                else
                    series.Draw(canvas, plotArea);
            }
        }
    }
}
// END - 2.4.0 - EDIT - Fix duplicate min/max
