using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Series;
using MEGraph.MAUI.Styles;
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

            _baseChart.Legend?.Draw(canvas, dirtyRect, _baseChart.Series);

        }
        //private (float left, float top, float right, float bottom) CalculateAutoMargin(ICanvas canvas, BaseChart chart, RectF outerArea)
        //{
        //    float top = 0, left = 0, right = 20, bottom = 0;

        //    // Title
        //    if (!string.IsNullOrWhiteSpace(chart.Title))
        //    {
        //        var titleSize = canvas.GetStringSize(
        //            chart.Title,
        //            Microsoft.Maui.Graphics.Font.Default,  // dùng default font
        //            18
        //        );
        //        top = titleSize.Height + 10; // +10 padding
        //    }

        //    // Axes
        //    foreach (var axis in chart.Axes)
        //    {
        //        if (axis is ValueAxis valueAxis && axis.Orientation == AxisOrientation.Y)
        //        {
        //            var allSeries = chart.Series.OfType<LineSeries>().ToList();
        //            if (allSeries.Any())
        //            {
        //                float minY = allSeries.Min(s => s.Data.Min());
        //                float maxY = allSeries.Max(s => s.Data.Max());

        //                int tickCount = 5;
        //                var labels = Enumerable.Range(0, tickCount + 1)
        //                                       .Select(i => (minY + i * (maxY - minY) / tickCount).ToString("0"))
        //                                       .ToList();

        //                float maxWidth = labels.Max(l => canvas.GetStringSize(
        //                    l,
        //                    Microsoft.Maui.Graphics.Font.Default, 
        //                    axis.Title.FontSize
        //                    ).Width);

        //                var titleSize = string.IsNullOrWhiteSpace(axis.Title.Text)
        //                    ? new SizeF(0, 0)
        //                    : canvas.GetStringSize(
        //                        axis.Title.Text, 
        //                        Microsoft.Maui.Graphics.Font.Default, 
        //                        axis.Title.FontSize);

        //                left = Math.Max(left, maxWidth + 10 + titleSize.Height);
        //            }
        //        }
        //        else if (axis is CategoryAxis categoryAxis && axis.Orientation == AxisOrientation.X)
        //        {
        //            float maxLabelHeight = 0;
        //            float maxLabelMargin = 0;

        //            foreach (var label in categoryAxis.Labels)
        //            {
        //                var size = canvas.GetStringSize(
        //                    label.Text,
        //                    Microsoft.Maui.Graphics.Font.Default,
        //                    label.FontSize
        //                );
        //                maxLabelHeight = Math.Max(maxLabelHeight, size.Height + label.Margin);
        //                maxLabelMargin = Math.Max(maxLabelMargin, label.Margin);
        //            }

        //            var titleSize = string.IsNullOrWhiteSpace(categoryAxis.Title.Text)
        //                ? new SizeF(0, 0)
        //                : canvas.GetStringSize(categoryAxis.Title.Text,
        //                                       Microsoft.Maui.Graphics.Font.Default,
        //                                       categoryAxis.Title.FontSize);

        //            float padding = 5;

        //            bottom = Math.Max(bottom, maxLabelHeight + maxLabelMargin + padding + titleSize.Height + categoryAxis.Title.Margin);
        //        }
        //    }

        //    return (left, top, right, bottom);
        //}
        private (float left, float top, float right, float bottom) CalculateAutoMargin(ICanvas canvas, BaseChart chart, RectF outerArea)
        {
            float top = 0, left = 0, right = 20, bottom = 0;

            // Title
            if (!string.IsNullOrWhiteSpace(chart.Title))
            {
                var titleSize = canvas.GetStringSize(
                    chart.Title,
                    Microsoft.Maui.Graphics.Font.Default,
                    18
                );
                top = titleSize.Height + 10;
            }

            // Axes
            foreach (var axis in chart.Axes)
            {
                if (axis is ValueAxis valueAxis && axis.Orientation == AxisOrientation.Y)
                {
                    // Lọc series có dữ liệu
                    var allSeries = chart.Series
                        .OfType<LineSeries>()
                        .Where(s => s?.Data != null && s.Data.Count > 0)
                        .ToList();

                    if (allSeries.Any())
                    {
                        // Gom toàn bộ giá trị để tránh Min/Max trên tập rỗng
                        var values = allSeries.SelectMany(s => s.Data).ToList();
                        float minY = values.Min();
                        float maxY = values.Max();
                        if (Math.Abs(maxY - minY) < float.Epsilon)
                        {
                            // tránh trùng min=max
                            maxY = minY + 1f;
                        }

                        int tickCount = 5;
                        var labels = Enumerable.Range(0, tickCount + 1)
                                               .Select(i => (minY + i * (maxY - minY) / tickCount).ToString("0"))
                                               .ToList();

                        float maxWidth = labels.Max(l => canvas.GetStringSize(
                            l,
                            Microsoft.Maui.Graphics.Font.Default,
                            axis.Title.FontSize
                        ).Width);

                        var titleSize = string.IsNullOrWhiteSpace(axis.Title.Text)
                            ? new SizeF(0, 0)
                            : canvas.GetStringSize(
                                axis.Title.Text,
                                Microsoft.Maui.Graphics.Font.Default,
                                axis.Title.FontSize);

                        left = Math.Max(left, maxWidth + 10 + titleSize.Height);
                    }
                    // nếu không có dữ liệu, bỏ qua tăng lề trái cho trục Y
                }
                else if (axis is CategoryAxis categoryAxis && axis.Orientation == AxisOrientation.X)
                {
                    float maxLabelHeight = 0;
                    float maxLabelMargin = 0;

                    var labels = categoryAxis.Labels ?? new List<AxisLabel>();
                    foreach (var label in labels)
                    {
                        var size = canvas.GetStringSize(
                            label.Text,
                            Microsoft.Maui.Graphics.Font.Default,
                            label.FontSize
                        );
                        maxLabelHeight = Math.Max(maxLabelHeight, size.Height + label.Margin);
                        maxLabelMargin = Math.Max(maxLabelMargin, label.Margin);
                    }

                    var titleSize = string.IsNullOrWhiteSpace(categoryAxis.Title.Text)
                        ? new SizeF(0, 0)
                        : canvas.GetStringSize(categoryAxis.Title.Text,
                                               Microsoft.Maui.Graphics.Font.Default,
                                               categoryAxis.Title.FontSize);

                    float padding = 5;

                    bottom = Math.Max(bottom, maxLabelHeight + maxLabelMargin + padding + titleSize.Height + categoryAxis.Title.Margin);
                }
            }

            return (left, top, right, bottom);
        }
    }
}
