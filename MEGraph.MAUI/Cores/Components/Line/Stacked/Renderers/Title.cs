using MEGraph.MAUI.Title;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Cores.Components.Line.Stacked.Renderers
{
    public class Title : ITitle
    {
        public void Draw(ICanvas canvas, RectF dirtyRect, string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return;

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
                title,
                titleArea,
                HorizontalAlignment.Center,
                VerticalAlignment.Center
            );
        }
    }
}
