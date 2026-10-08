using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MEGraph.MAUI.Cores.Components.Line.Standard.Renderers;
using MEGraph.MAUI.Cores.Components;

using RTitle = MEGraph.MAUI.Cores.Components.Line.Standard.Renderers.Title;
using RAxes = MEGraph.MAUI.Cores.Components.Line.Standard.Renderers.Axes;
using RSeries = MEGraph.MAUI.Cores.Components.Line.Standard.Renderers.Series;
using RLegend = MEGraph.MAUI.Cores.Components.Line.Standard.Renderers.Legend;
using MEGraph.MAUI.Series.Line;

namespace MEGraph.MAUI.Cores.Pipeline
{
    public class LineRenderPipeline : IRenderPipeline
    {
        private readonly RTitle _titleRenderer;
        private readonly RAxes _axesRenderer;
        private readonly RSeries _seriesRenderer;
        private readonly RLegend _legendRenderer;
        private BaseChart _chart;

        public LineRenderPipeline()
        {
            _titleRenderer = new RTitle();
            _axesRenderer = new RAxes();
            _seriesRenderer = new RSeries();
            _legendRenderer = new RLegend();
        }

        public LineRenderPipeline(BaseChart chart) : this()
        {
            _chart = chart;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if(_chart != null)
            {
                Draw(canvas, dirtyRect, _chart);
            }
        }

        public void Draw(ICanvas canvas, RectF dirtyRect, BaseChart chart)
        {
            // 1. Vẽ background
            DrawBackground(canvas, dirtyRect);

            // 1.5. Đồng bộ AutoRange và TotalPoints cho Axes
            SyncAxesWithData(chart);

            // 2. Tính toán plot area
            var plotArea = CalculatePlotArea(canvas, dirtyRect, chart);

            // 3. Vẽ title
            _titleRenderer.Draw(canvas, dirtyRect, chart.Title);

            // 4. Vẽ axes (Line chart specific)
            _axesRenderer.Draw(canvas, dirtyRect, plotArea, chart);

            // 5. Vẽ series (Line chart specific)
            _seriesRenderer.Draw(canvas, plotArea, chart);

            // 6. Vẽ legend (chỉ khi chart.Legend được set)
            if (chart.Legend != null)
                _legendRenderer.Draw(canvas, dirtyRect, chart.Series);

        }

        private float _lastDataMin = float.NaN;
        private float _lastDataMax = float.NaN;
        private int _lastMaxPoints = -1;

        private void SyncAxesWithData(BaseChart chart)
        {
            if (chart?.Series == null || !chart.Series.Any()) return;

            var lineSeries = chart.Series.OfType<LineSeries>().Where(s => s.IsVisible).ToList();
            if (!lineSeries.Any()) return;

            var allData = lineSeries.Where(s => s.Data != null).SelectMany(s => s.Data).ToList();
            int maxPoints = lineSeries.Where(s => s.Data != null && s.Data.Any()).Select(s => s.Data.Count).DefaultIfEmpty(0).Max();

            float dataMin = allData.Any() ? allData.Min() : 0f;
            float dataMax = allData.Any() ? allData.Max() : 100f;

            bool isRangeChanged = Math.Abs(dataMin - _lastDataMin) > 1e-4f || Math.Abs(dataMax - _lastDataMax) > 1e-4f;
            bool isPointsChanged = maxPoints != _lastMaxPoints;

            // Nếu dữ liệu không đổi (ví dụ đang trong Animation), không tính toán lại để tránh rung lắc frame
            if (!isRangeChanged && !isPointsChanged) return;

            _lastDataMin = dataMin;
            _lastDataMax = dataMax;
            _lastMaxPoints = maxPoints;

            if (chart.Axes != null)
            {
                var yAxes = chart.Axes.Where(a => a.Orientation == MEGraph.MAUI.Axes.AxisOrientation.Y && (a.ChartId == chart.Id || string.IsNullOrEmpty(a.ChartId))).ToList();
                var defaultYAxis = yAxes.FirstOrDefault(a => a.AxisPosition == MEGraph.MAUI.Axes.AxisPosition.Left) ?? yAxes.FirstOrDefault();

                foreach (var yAxis in yAxes)
                {
                    if (!yAxis.IsAutoRange) continue;

                    // Lấy các series gán vào axis này (hoặc default axis nếu series không khai AxisId)
                    var matchedSeries = lineSeries.Where(s =>
                        (!string.IsNullOrEmpty(s.AxisId) && s.AxisId == yAxis.Id) ||
                        (string.IsNullOrEmpty(s.AxisId) && yAxis == defaultYAxis)
                    ).ToList();

                    if (!matchedSeries.Any()) continue;

                    var axisData = matchedSeries.Where(s => s.Data != null).SelectMany(s => s.Data).ToList();
                    if (axisData.Any())
                    {
                        float min = axisData.Min();
                        float max = axisData.Max();
                        yAxis.ApplyAutoRange(min, max);
                    }
                }

                var categoryAxis = chart.Axes.FirstOrDefault(a => a.Orientation == MEGraph.MAUI.Axes.AxisOrientation.X && (a.ChartId == chart.Id || string.IsNullOrEmpty(a.ChartId)));
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
            // START - 2.6.1 - EDIT - Clear canvas and fill chart background
            if (_chart.BackgroundColor != null && _chart.BackgroundColor != Colors.Transparent)
            {
                canvas.FillColor = _chart.BackgroundColor;
                canvas.FillRectangle(dirtyRect);
            }
            // END - 2.6.1 - EDIT
        }
    }
}
