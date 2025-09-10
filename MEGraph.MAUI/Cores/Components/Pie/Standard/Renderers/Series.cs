using MEGraph.MAUI.Series;
using MEGraph.MAUI.Series.Pie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Cores.Components.Pie.Standard.Renderers
{
    public class Series : ISeries
    {
        private BaseChart? _baseChart;
        public string Name => _baseChart?.Title ?? "StackedLineSeriesRenderer";
        public void Draw(ICanvas canvas, RectF plotArea, BaseChart? baseChart)
        {
            if (baseChart == null || baseChart.Series.Count > 1) return;
            _baseChart = baseChart;
            Draw(canvas, plotArea);
        }
        public void Draw(ICanvas canvas, RectF plotArea)
        {
            if (_baseChart == null) return;
            var pieSeries = _baseChart.Series
                .OfType<PieSeries>()
                .SingleOrDefault();

            pieSeries?.Draw(canvas, plotArea);
        }
    }
}
