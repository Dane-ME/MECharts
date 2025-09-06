using MEGraph.MAUI.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Cores.Components.Line.Renderers
{
    public class Series : ISeries
    {
        private BaseChart? _baseChart;
        public string Name => _baseChart?.Title ?? "Name is not exist!";

        public void Draw(ICanvas canvas, RectF plotArea, BaseChart? baseChart)
        {
            if (baseChart == null) throw new ArgumentNullException(nameof(baseChart));
            _baseChart = baseChart;
            Draw(canvas, plotArea);
        }
        public void Draw(ICanvas canvas, RectF plotArea)
        {
            if (_baseChart == null) return;
            var allLineSeries = _baseChart.Series.OfType<LineSeries>().ToList();
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

            foreach (var series in _baseChart.Series)
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
