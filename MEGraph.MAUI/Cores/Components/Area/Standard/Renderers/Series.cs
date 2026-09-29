using MEGraph.MAUI.Series;
using MEGraph.MAUI.Series.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Cores.Components.Area.Standard.Renderers
{
    public class Series
    {
        // START - 2.4.0 - EDIT - Fix duplicate min/max: dùng GetMinY/GetMaxY từ series.
        public void Draw(ICanvas canvas, RectF plotArea, BaseChart? baseChart)
        {
            if (baseChart == null) return;

            var allAreaSeries = baseChart.Series.OfType<AreaSeries>().ToList();
            
            // Lấy min/max từ trục Value nếu có để đồng bộ hoàn toàn với vạch trục
            var valueAxis = baseChart.Axes?.FirstOrDefault(a => a.Orientation == MEGraph.MAUI.Axes.AxisOrientation.Y);
            float? globalMinY = valueAxis?.MinValue;
            float? globalMaxY = valueAxis?.MaxValue;

            if (globalMinY == null || globalMaxY == null)
            {
                if (allAreaSeries.Any())
                {
                    globalMinY = allAreaSeries.Min(s => s.GetMinY());
                    globalMaxY = allAreaSeries.Max(s => s.GetMaxY());
                }
            }

            var categoryAxis = baseChart.Axes?.FirstOrDefault(a => a.Orientation == MEGraph.MAUI.Axes.AxisOrientation.X);
            int? totalPoints = categoryAxis?.TotalPoints > 0 
                ? categoryAxis.TotalPoints 
                : (allAreaSeries.Any() ? allAreaSeries.Max(s => s.Data?.Count ?? 0) : null);

            foreach (var series in baseChart.Series)
            {
                if (!series.IsVisible) continue;

                if (series is AreaSeries areaSeries)
                    areaSeries.Draw(canvas, plotArea, globalMinY, globalMaxY, baseChart.AnimationProgress, totalPoints);
                else
                    series.Draw(canvas, plotArea);
            }
        }
        // END - 2.4.0 - EDIT
    }
}
