using MEGraph.MAUI.Series;
using MEGraph.MAUI.Series.Line;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Cores.Components.Line.Stacked.Renderers
{
    public class Series : ISeries
    {
        private BaseChart? _baseChart;
        public string Name => _baseChart?.Title ?? "StackedLineSeriesRenderer";

        public void Draw(ICanvas canvas, RectF plotArea, BaseChart? baseChart)
        {
            if (baseChart == null) throw new ArgumentNullException(nameof(baseChart));
            _baseChart = baseChart;
            Draw(canvas, plotArea);
        }

        public void Draw(ICanvas canvas, RectF plotArea)
        {
            if (_baseChart == null) return;

            var stackedSeries = _baseChart.Series
                .OfType<StackedLineSeries>()
                .OrderBy(s => s.StackOrder)
                .ToList();

            if (!stackedSeries.Any()) return;

            int maxPoints = stackedSeries.Max(s => s.Data.Count);
            float minstackedSeries = stackedSeries.SelectMany(s => s.Data).Min();

            var accumulatedSums = new float[maxPoints];
            for (int i = 0; i < maxPoints; i++)
            {
                float sum = 0f;
                for (int j = 0; j < stackedSeries.Count; j++)
                {
                    if (i < stackedSeries[j].Data.Count)
                    {
                        sum += stackedSeries[j].Data[i];
                    }
                }
                accumulatedSums[i] = sum;
            }

            float globalMinY = minstackedSeries;
            float globalMaxY = accumulatedSums.Max();
            if (globalMaxY - globalMinY == 0) globalMaxY = globalMinY + 1;

            var accumulated = new float[maxPoints];

            foreach (var series in stackedSeries)
            {
                var baseValues = new List<float>();

                for (int i = 0; i < maxPoints; i++)
                {
                    float current = (i < series.Data.Count) ? series.Data[i] : 0f;
                    baseValues.Add(accumulated[i]);  // Base value = tổng của các series trước
                    accumulated[i] += current;       // Cộng dồn cho series tiếp theo
                }

                series.Draw(canvas, plotArea, globalMinY, globalMaxY, baseValues);
            }
        }
    }
}
