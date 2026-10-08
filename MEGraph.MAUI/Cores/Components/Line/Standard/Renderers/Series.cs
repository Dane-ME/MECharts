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
            var valueAxes = baseChart.Axes?.Where(a => a.Orientation == MEGraph.MAUI.Axes.AxisOrientation.Y).ToList();
            var defaultValueAxis = valueAxes?.FirstOrDefault(a => a.AxisPosition == MEGraph.MAUI.Axes.AxisPosition.Left) 
                                   ?? valueAxes?.FirstOrDefault();

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
                {
                    // Tìm trục tương ứng với series (qua AxisId hoặc vị trí mặc định)
                    var targetAxis = !string.IsNullOrEmpty(lineSeries.AxisId)
                        ? valueAxes?.FirstOrDefault(a => a.Id == lineSeries.AxisId)
                        : defaultValueAxis;

                    float? minY = targetAxis?.MinValue;
                    float? maxY = targetAxis?.MaxValue;

                    if (minY == null || maxY == null)
                    {
                        minY = lineSeries.GetMinY();
                        maxY = lineSeries.GetMaxY();
                    }

                    lineSeries.Draw(canvas, plotArea, minY, maxY, baseChart.AnimationProgress, totalPoints);
                }
                else
                {
                    series.Draw(canvas, plotArea);
                }
            }
        }
    }
}
// END - 2.4.0 - EDIT - Fix duplicate min/max
