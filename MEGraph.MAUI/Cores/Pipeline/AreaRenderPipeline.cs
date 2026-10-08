using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MEGraph.MAUI.Cores.Components.Area.Standard.Renderers;
using MEGraph.MAUI.Cores.Components;

using RTitle = MEGraph.MAUI.Cores.Components.Area.Standard.Renderers.Title;
using RAxes = MEGraph.MAUI.Cores.Components.Area.Standard.Renderers.Axes;
using RSeries = MEGraph.MAUI.Cores.Components.Area.Standard.Renderers.Series;
using RLegend = MEGraph.MAUI.Cores.Components.Area.Standard.Renderers.Legend;
using MEGraph.MAUI.Series.Area;
namespace MEGraph.MAUI.Cores.Pipeline
{
    public class AreaRenderPipeline : IRenderPipeline
    {
        private readonly RTitle _titleRenderer;
        private readonly RAxes _axesRenderer;
        private readonly RSeries _seriesRenderer;
        private readonly RLegend _legendRenderer; 
        private BaseChart _chart;

        public AreaRenderPipeline()
        {
            _titleRenderer = new RTitle();
            _axesRenderer = new RAxes();
            _seriesRenderer = new RSeries();
            _legendRenderer = new RLegend();
        }
        public AreaRenderPipeline(BaseChart chart) : this()
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
            if (chart?.Series == null || chart.Series.Count == 0) return;

            float dataMin = float.MaxValue;
            float dataMax = float.MinValue;
            int maxPoints = 0;
            bool hasData = false;

            for (int i = 0; i < chart.Series.Count; i++)
            {
                if (chart.Series[i] is AreaSeries s && s.IsVisible && s.Data != null && s.Data.Count > 0)
                {
                    hasData = true;
                    if (s.Data.Count > maxPoints) maxPoints = s.Data.Count;
                    for (int j = 0; j < s.Data.Count; j++)
                    {
                        float v = s.Data[j];
                        if (v < dataMin) dataMin = v;
                        if (v > dataMax) dataMax = v;
                    }
                }
            }

            if (!hasData) return;

            bool isRangeChanged = Math.Abs(dataMin - _lastDataMin) > 1e-4f || Math.Abs(dataMax - _lastDataMax) > 1e-4f;
            bool isPointsChanged = maxPoints != _lastMaxPoints;

            if (!isRangeChanged && !isPointsChanged) return;

            _lastDataMin = dataMin;
            _lastDataMax = dataMax;
            _lastMaxPoints = maxPoints;

            if (chart.Axes != null)
            {
                var valueAxis = chart.Axes.FirstOrDefault(a => a.Orientation == MEGraph.MAUI.Axes.AxisOrientation.Y && (a.ChartId == chart.Id || string.IsNullOrEmpty(a.ChartId)));
                if (valueAxis != null && valueAxis.IsAutoRange && isRangeChanged && hasData)
                {
                    valueAxis.ApplyAutoRange(dataMin, dataMax);
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
