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

            // Lấy min/max từ trục Value nếu có để đồng bộ hoàn toàn với vạch trục
            var valueAxis = baseChart.Axes?.FirstOrDefault(a => a.Orientation == MEGraph.MAUI.Axes.AxisOrientation.Y);
            float? globalMinY = valueAxis?.MinValue;
            float? globalMaxY = valueAxis?.MaxValue;

            if (globalMinY == null || globalMaxY == null)
            {
                if (allLineSeries.Any())
                {
                    globalMinY = allLineSeries.Min(s => s.GetMinY());
                    globalMaxY = allLineSeries.Max(s => s.GetMaxY());
                }
            }

            var categoryAxis = baseChart.Axes?.FirstOrDefault(a => a.Orientation == MEGraph.MAUI.Axes.AxisOrientation.X);
            int? totalPoints = null;
            if (categoryAxis != null && categoryAxis.TotalPoints > 0)
            {
                totalPoints = categoryAxis.TotalPoints;
            }
            else if (allLineSeries.Any())
            {
                totalPoints = allLineSeries.Max(s => s.Data?.Count ?? 0);
            }

            foreach (var series in baseChart.Series)
            {
                if (!series.IsVisible) continue;

                if (series is LineSeries lineSeries)
                    lineSeries.Draw(canvas, plotArea, globalMinY, globalMaxY, baseChart.AnimationProgress, totalPoints);
                else
                    series.Draw(canvas, plotArea);
            }
        }
    }
}
// END - 2.4.0 - EDIT - Fix duplicate min/max
