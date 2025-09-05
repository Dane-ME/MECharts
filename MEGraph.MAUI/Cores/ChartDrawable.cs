using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Series;
using Microsoft.Maui.Graphics;

namespace MEGraph.MAUI.Cores
{
    public class ChartDrawable : IDrawable, IDisposable
    {
        private BaseChart? _baseChart;
        public ChartDrawable(BaseChart baseChart)
        {
            _baseChart = baseChart;
        }

        public void Dispose()
        {
            _baseChart = null;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (_baseChart == null) return;
            var (leftMargin, topMargin, rightMargin, bottomMargin) = CalculateAutoMargin(canvas, _baseChart, dirtyRect);

            var plotArea = new RectF(
                dirtyRect.Left + leftMargin,
                dirtyRect.Top + topMargin,
                dirtyRect.Width - leftMargin - rightMargin,
                dirtyRect.Height - topMargin - bottomMargin
            );

            canvas.FillColor = Colors.WhiteSmoke;
            canvas.FillRectangle(dirtyRect);

            if (!string.IsNullOrWhiteSpace(_baseChart.Title))
            {
                const float titleHeight = 30f; 
                var titleArea = new RectF(
                    dirtyRect.Left,
                    dirtyRect.Top,
                    dirtyRect.Width,
                    titleHeight
                );

                canvas.FontSize = 18;
                canvas.FontColor = Colors.Black;
                canvas.Font = Microsoft.Maui.Graphics.Font.DefaultBold;

                canvas.DrawString(
                    _baseChart.Title,
                    titleArea,
                    HorizontalAlignment.Center,
                    VerticalAlignment.Center
                );
            }

            foreach (var axis in _baseChart.Axes)
            {
                axis.Draw(canvas, dirtyRect, plotArea);
            }

            foreach (var series in _baseChart.Series)
            {
                series.Draw(canvas, plotArea);
            }

            _baseChart.Legend?.Draw(canvas, dirtyRect, _baseChart.Series);

        }
        private (float left, float top, float right, float bottom) CalculateAutoMargin(ICanvas canvas, BaseChart chart, RectF outerArea)
        {
            float top = 0, left = 0, right = 20, bottom = 0;

            // Title
            if (!string.IsNullOrWhiteSpace(chart.Title))
            {
                var titleSize = canvas.GetStringSize(
                    chart.Title,
                    Microsoft.Maui.Graphics.Font.Default,  // dùng default font
                    18
                );
                top = titleSize.Height + 10; // +10 padding
            }

            // Axes
            foreach (var axis in chart.Axes)
            {
                if (axis is ValueAxis valueAxis && axis.Orientation == AxisOrientation.Y)
                {
                    var allSeries = chart.Series.OfType<LineSeries>().ToList();
                    if (allSeries.Any())
                    {
                        float minY = allSeries.Min(s => s.Data.Min());
                        float maxY = allSeries.Max(s => s.Data.Max());

                        int tickCount = 5;
                        var labels = Enumerable.Range(0, tickCount + 1)
                                               .Select(i => (minY + i * (maxY - minY) / tickCount).ToString("0"))
                                               .ToList();

                        float maxWidth = labels.Max(l => canvas.GetStringSize(
                            l,
                            Microsoft.Maui.Graphics.Font.Default, 
                            12
                            ).Width);

                        var titleSize = string.IsNullOrWhiteSpace(axis.Title)
                            ? new SizeF(0, 0)
                            : canvas.GetStringSize(
                                axis.Title, 
                                Microsoft.Maui.Graphics.Font.Default, 
                                14);

                        left = Math.Max(left, maxWidth + 10 + titleSize.Height);
                    }
                }
                else if (axis is CategoryAxis categoryAxis && axis.Orientation == AxisOrientation.X)
                {
                    var labels = categoryAxis.Labels ?? new List<string>();
                    float maxHeight = labels.Any()
                        ? labels.Max(l => canvas.GetStringSize(l, Microsoft.Maui.Graphics.Font.Default, 12).Height)
                        : 0;

                    var titleSize = string.IsNullOrWhiteSpace(axis.Title)
                        ? new SizeF(0, 0)
                        : canvas.GetStringSize(axis.Title, Microsoft.Maui.Graphics.Font.Default, 14);

                    bottom = Math.Max(bottom, maxHeight + 5 + titleSize.Height + 5);
                }
            }

            return (left, top, right, bottom);
        }

    }
}
