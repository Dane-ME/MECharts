using MEGraph.MAUI.Series;
using MEGraph.MAUI.Series.Line;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Cores.Components.Line.Standard.Renderers
{
    public class Series
    {
        private BaseChart? _baseChart;
        public string Name => _baseChart?.Title ?? "Name is not exist!";

        public void Draw(ICanvas canvas, RectF plotArea, BaseChart? baseChart)
        {
            if (baseChart == null) return;

            var allLineSeries = baseChart.Series.OfType<LineSeries>().ToList();
            float? globalMinY = null;
            float? globalMaxY = null;

            if (allLineSeries.Any())
            {
                var allValues = allLineSeries.SelectMany(s => s.Data).ToList();
                if (allValues.Any())
                {
                    globalMinY = allValues.Min();
                    globalMaxY = allValues.Max();
                }
            }

            foreach (var series in baseChart.Series)
            {
                if (series is LineSeries lineSeries)
                {
                    lineSeries.Draw(canvas, plotArea, globalMinY, globalMaxY);
                }
                else
                {
                    series.Draw(canvas, plotArea);
                }
            }
        }
    }
}
