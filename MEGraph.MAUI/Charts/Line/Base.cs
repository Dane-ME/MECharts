using MEGraph.MAUI.Cores;
using MEGraph.MAUI.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Charts.Line
{
    public abstract class Base : BaseChart
    {
        protected List<LineSeries> LineSeriesList { get; set; } = new();
        protected LineSeries? PrimarySeries { get; private set; }
        protected Base() { }

        protected LineSeries CreateSeries(string name = "Series")
        {
            var series = new LineSeries() { Name = name };
            LineSeriesList.Add(series);
            ((BaseChart)this).Series.Add(series);
            if (PrimarySeries == null)
                PrimarySeries = series;
            return series;
        }
        public virtual void SetLineData(IEnumerable<float> data)
        {
            if (PrimarySeries == null)
                CreateSeries();
            PrimarySeries.Data = data.ToList();
            Refresh();
        }
        public virtual void AddLineSeries(LineSeries series)
        {
            LineSeriesList.Add(series);
            ((BaseChart)this).Series.Add(series);

            if (PrimarySeries == null)
                PrimarySeries = series;

            Refresh();
        }

        public Color LineColor
        {
            get => PrimarySeries?.StrokeColor ?? Colors.Blue;
            set
            {
                if (PrimarySeries == null)
                    CreateSeries();
                PrimarySeries.StrokeColor = value;
                Refresh();
            }
        }

        public float LineWidth
        {
            get => PrimarySeries?.StrokeWidth ?? 3f;
            set
            {
                if (PrimarySeries == null)
                    CreateSeries();
                PrimarySeries.StrokeWidth = value;
                Refresh();
            }
        }

        protected abstract void DrawLineChart(ICanvas canvas, RectF plotArea);
        protected abstract void CalculateLinePoints(List<LineSeries> series, RectF plotArea);

    }
}
