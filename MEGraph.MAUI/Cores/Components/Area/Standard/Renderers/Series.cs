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
        private BaseChart? _baseChart;
        public string Name => _baseChart?.Title ?? "Name is not exist!";

        public void Draw(ICanvas canvas, RectF plotArea, BaseChart? baseChart)
        {
            if (baseChart == null) return;

            var allAreaSeries = baseChart.Series.OfType<AreaSeries>().ToList();
            float? globalMinY = null;
            float? globalMaxY = null;

            if (allAreaSeries.Any())
            {
                var allValues = allAreaSeries.SelectMany(s => s.Data).ToList();
                if (allValues.Any())
                {
                    globalMinY = allValues.Min();
                    globalMaxY = allValues.Max();
                }
            }

            foreach (var series in baseChart.Series)
            {
                if (series is AreaSeries areaSeries)
                {
                    areaSeries.Draw(canvas, plotArea, globalMinY, globalMaxY);
                }
                else
                {
                    series.Draw(canvas, plotArea);
                }
            }
        }
    }
}
