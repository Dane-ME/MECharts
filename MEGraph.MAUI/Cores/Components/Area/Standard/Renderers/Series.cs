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
            float? globalMinY = null;
            float? globalMaxY = null;

            if (allAreaSeries.Any())
            {
                globalMinY = allAreaSeries.Min(s => s.GetMinY());
                globalMaxY = allAreaSeries.Max(s => s.GetMaxY());
            }

            foreach (var series in baseChart.Series)
            {
                if (series is AreaSeries areaSeries)
                    areaSeries.Draw(canvas, plotArea, globalMinY, globalMaxY);
                else
                    series.Draw(canvas, plotArea);
            }
        }
        // END - 2.4.0 - EDIT
    }
}
