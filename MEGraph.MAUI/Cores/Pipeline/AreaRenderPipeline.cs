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

            // 2. Tính toán plot area
            var plotArea = CalculatePlotArea(canvas, dirtyRect, chart);

            // 3. Vẽ title
            _titleRenderer.Draw(canvas, dirtyRect, chart.Title);

            // 4. Vẽ axes (Line chart specific)
            _axesRenderer.Draw(canvas, dirtyRect, plotArea, chart);

            // 5. Vẽ series (Line chart specific)
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

        public void DrawBackground(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = _chart.BackgroundColor ?? Colors.Transparent;
            canvas.FillRectangle(dirtyRect);
        }
    }
}
