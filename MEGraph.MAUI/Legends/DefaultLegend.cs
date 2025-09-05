using MEGraph.MAUI.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Legends
{
    public class DefaultLegend : ILegend
    {
        public void Draw(ICanvas canvas, RectF dirtyRect, IEnumerable<ISeries> series)
        {
            float x = dirtyRect.Right - 100;
            float y = dirtyRect.Top + 20;

            foreach (var s in series)
            {
                // ô màu
                canvas.FillColor = (s is LineSeries line) ? line.StrokeColor : Colors.Gray;
                canvas.FillRectangle(x, y, 10, 10);

                // tên
                canvas.FontColor = Colors.Black;
                canvas.DrawString(s.Name, x + 15, y, HorizontalAlignment.Left);

                y += 20;
            }
        }
    }
}
