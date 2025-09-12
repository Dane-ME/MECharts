using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Axes.Pie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RLegend = MEGraph.MAUI.Cores.Components.Pie.Standard.Renderers.Legend;
using RSeries = MEGraph.MAUI.Cores.Components.Pie.Standard.Renderers.Series;

namespace MEGraph.MAUI.Cores.Pipeline
{
    public class PieRenderPipeline : IRenderPipeline
    {
        private readonly RSeries _seriesRenderer;
        private readonly RLegend _legendRenderer;
        private BaseChart _chart;

        public PieRenderPipeline()
        {
            _seriesRenderer = new RSeries();
            _legendRenderer = new RLegend();
        }

        public PieRenderPipeline(BaseChart chart) : this()
        {
            _chart = chart;
        }
        public RectF CalculatePlotArea(ICanvas canvas, RectF dirtyRect, BaseChart chart)
        {
            var angular = chart.Axes
                .OfType<Category>()
                .FirstOrDefault(a => a.Orientation == AxisOrientation.Angular);

            var margins = angular?.MeasureMargins(canvas, dirtyRect, chart.Axes) ?? (0f, 0f, 0f, 0f);

            return new RectF(
                dirtyRect.Left + margins.Left,
                dirtyRect.Top + margins.Top,
                dirtyRect.Width - margins.Left - margins.Right,
                dirtyRect.Height - margins.Top - margins.Bottom
            );
        }

        private void DrawBackground(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = _chart.BackgroundColor ?? Colors.Transparent;
            canvas.FillRectangle(dirtyRect);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect, BaseChart chart)
        {
            DrawBackground(canvas, dirtyRect);

            var plotArea = CalculatePlotArea(canvas, dirtyRect, chart);

            if (chart.Axes != null)
            {
                foreach (var axis in chart.Axes)
                {
                    axis.Draw(canvas, dirtyRect, plotArea, chart);
                }
            }

            _seriesRenderer.Draw(canvas, plotArea, chart);

            foreach (var axis in chart.Axes)
            {
                if (axis is Axes.Base baseaxis)
                {
                    baseaxis.DrawOverlay(canvas, dirtyRect, plotArea);
                }
            }


            // 6. Vẽ legend
            //_legendRenderer.Draw(canvas, dirtyRect, chart.Legend, chart.Series);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (_chart != null)
            {
                Draw(canvas, dirtyRect, _chart);
            }
        }
    }
}
