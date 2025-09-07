using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RTitle = MEGraph.MAUI.Cores.Components.Line.Stacked.Renderers.Title;
using RAxes = MEGraph.MAUI.Cores.Components.Line.Stacked.Renderers.Axes;
using RLegend = MEGraph.MAUI.Cores.Components.Line.Stacked.Renderers.Legend;
using RSeries = MEGraph.MAUI.Cores.Components.Line.Stacked.Renderers.Series;

namespace MEGraph.MAUI.Cores.Pipeline
{
    public class StackedLineRenderPipeline : IRenderPipeline
    {
        private readonly RTitle _titleRenderer;
        private readonly RAxes _axesRenderer;
        private readonly RSeries _seriesRenderer;
        private readonly RLegend _legendRenderer;
        private BaseChart _chart;

        public StackedLineRenderPipeline()
        {
            _titleRenderer = new RTitle();
            _axesRenderer = new RAxes();
            _seriesRenderer = new RSeries();
            _legendRenderer = new RLegend();
        }

        public StackedLineRenderPipeline(BaseChart chart) : this()
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

            // 2. Tính toán plot area
            var plotArea = CalculatePlotArea(canvas, dirtyRect, chart);

            // 3. Vẽ title
            _titleRenderer.Draw(canvas, dirtyRect, chart.Title);

            // 4. Vẽ axes
            _axesRenderer.Draw(canvas, dirtyRect, plotArea, chart);

            // 5. Vẽ stacked series
            _seriesRenderer.Draw(canvas, plotArea, chart);

            // 6. Vẽ legend
            //_legendRenderer.Draw(canvas, dirtyRect, chart.Legend, chart.Series);
        }

        public RectF CalculatePlotArea(ICanvas canvas, RectF dirtyRect, BaseChart chart)
        {
            var margins = _axesRenderer.CalculateMargins(canvas, dirtyRect, chart.Axes);

            return new RectF(
                dirtyRect.Left + margins.Left,
                dirtyRect.Top + margins.Top,
                dirtyRect.Width - margins.Left - margins.Right,
                dirtyRect.Height - margins.Top - margins.Bottom
            );
        }

        private void DrawBackground(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.WhiteSmoke;
            canvas.FillRectangle(dirtyRect);
        }
    }
}