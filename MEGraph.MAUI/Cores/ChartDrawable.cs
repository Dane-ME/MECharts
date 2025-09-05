using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;

namespace MEGraph.MAUI.Cores
{
    public class ChartDrawable : IDrawable
    {
        private readonly BaseChart _baseChart;
        public ChartDrawable(BaseChart baseChart)
        {
            _baseChart = baseChart;
        }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.WhiteSmoke;
            canvas.FillRectangle(dirtyRect);

            foreach (var series in _baseChart.Series)
            {
                series.Draw(canvas, dirtyRect);
            }
        }
    }
}
