using System;
using System.Collections.Generic;
using System.Linq;
using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Series.Column;
using Microsoft.Maui.Graphics;

using RTitle = MEGraph.MAUI.Cores.Components.Line.Standard.Renderers.Title;
using RAxes = MEGraph.MAUI.Cores.Components.Line.Standard.Renderers.Axes;
using RLegend = MEGraph.MAUI.Cores.Components.Line.Standard.Renderers.Legend;
using RColumnSeries = MEGraph.MAUI.Cores.Components.Column.Standard.Renderers.Series;

namespace MEGraph.MAUI.Cores.Pipeline
{
    public class ColumnRenderPipeline : IRenderPipeline
    {
        private readonly RTitle _titleRenderer;
        private readonly RAxes _axesRenderer;
        private readonly RColumnSeries _seriesRenderer;
        private readonly RLegend _legendRenderer;
        private BaseChart _chart;

        public ColumnRenderPipeline()
        {
            _titleRenderer = new RTitle();
            _axesRenderer = new RAxes();
            _seriesRenderer = new RColumnSeries();
            _legendRenderer = new RLegend();
        }

        public ColumnRenderPipeline(BaseChart chart) : this()
        {
            _chart = chart;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (_chart != null)
            {
                Draw(canvas, dirtyRect, _chart);
            }
        }

        public void Draw(ICanvas canvas, RectF dirtyRect, BaseChart chart)
        {
            // 1. Background
            DrawBackground(canvas, dirtyRect);

            // 2. Đồng bộ AutoRange và TotalPoints cho Axes (hỗ trợ Dual Y-axis)
            SyncAxesWithData(chart);

            // 3. Tính toán plot area
            var plotArea = CalculatePlotArea(canvas, dirtyRect, chart);

            // 4. Vẽ title
            _titleRenderer.Draw(canvas, dirtyRect, chart.Title);

            // 5. Vẽ axes
            _axesRenderer.Draw(canvas, dirtyRect, plotArea, chart);

            // 6. Vẽ column series
            _seriesRenderer.Draw(canvas, plotArea, chart);

            // 7. Vẽ legend
            if (chart.Legend != null)
            {
                _legendRenderer.Draw(canvas, dirtyRect, chart.Series);
            }
        }

        private float _lastDataMin = float.NaN;
        private float _lastDataMax = float.NaN;
        private int _lastMaxPoints = -1;

        private void SyncAxesWithData(BaseChart chart)
        {
            if (chart?.Series == null || !chart.Series.Any()) return;

            var colSeries = chart.Series.OfType<ColumnSeries>().Where(s => s.IsVisible).ToList();
            if (!colSeries.Any()) return;

            var allData = colSeries.Where(s => s.Data != null).SelectMany(s => s.Data).ToList();
            int maxPoints = colSeries.Where(s => s.Data != null && s.Data.Any()).Select(s => s.Data.Count).DefaultIfEmpty(0).Max();

            float dataMin = allData.Any() ? allData.Min() : 0f;
            float dataMax = allData.Any() ? allData.Max() : 100f;

            bool isRangeChanged = Math.Abs(dataMin - _lastDataMin) > 1e-4f || Math.Abs(dataMax - _lastDataMax) > 1e-4f;
            bool isPointsChanged = maxPoints != _lastMaxPoints;

            if (!isRangeChanged && !isPointsChanged) return;

            _lastDataMin = dataMin;
            _lastDataMax = dataMax;
            _lastMaxPoints = maxPoints;

            if (chart.Axes != null)
            {
                var yAxes = chart.Axes.Where(a => a.Orientation == AxisOrientation.Y && (a.ChartId == chart.Id || string.IsNullOrEmpty(a.ChartId))).ToList();
                var defaultYAxis = yAxes.FirstOrDefault(a => a.AxisPosition == AxisPosition.Left) ?? yAxes.FirstOrDefault();

                foreach (var yAxis in yAxes)
                {
                    if (!yAxis.IsAutoRange) continue;

                    // Series tương ứng với trục này
                    var matchedSeries = colSeries.Where(s =>
                        (!string.IsNullOrEmpty(s.AxisId) && s.AxisId == yAxis.Id) ||
                        (string.IsNullOrEmpty(s.AxisId) && yAxis == defaultYAxis)
                    ).ToList();

                    if (!matchedSeries.Any()) continue;

                    var axisData = matchedSeries.Where(s => s.Data != null).SelectMany(s => s.Data).ToList();
                    if (axisData.Any())
                    {
                        float min = Math.Min(0f, axisData.Min()); // Cột dọc luôn bám 0 làm baseline
                        float max = Math.Max(0f, axisData.Max());
                        yAxis.ApplyAutoRange(min, max);
                    }
                }

                var categoryAxis = chart.Axes.FirstOrDefault(a => a.Orientation == AxisOrientation.X && (a.ChartId == chart.Id || string.IsNullOrEmpty(a.ChartId)));
                if (categoryAxis != null)
                {
                    categoryAxis.TotalPoints = Math.Max(maxPoints, categoryAxis.Labels?.Count ?? 0);
                }
            }
        }

        public RectF CalculatePlotArea(ICanvas canvas, RectF dirtyRect, BaseChart chart)
        {
            var margins = _axesRenderer.CalculateMargins(canvas, dirtyRect, chart.Axes, chart.Id);

            return new RectF(
                dirtyRect.Left + margins.Left,
                dirtyRect.Top + margins.Top,
                dirtyRect.Width - margins.Left - margins.Right,
                dirtyRect.Height - margins.Top - margins.Bottom
            );
        }

        public void DrawBackground(ICanvas canvas, RectF dirtyRect)
        {
            if (_chart?.BackgroundColor != null && _chart.BackgroundColor != Colors.Transparent)
            {
                canvas.FillColor = _chart.BackgroundColor;
                canvas.FillRectangle(dirtyRect);
            }
        }
    }
}
