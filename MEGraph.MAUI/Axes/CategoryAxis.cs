using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Axes
{
    public class CategoryAxis : IAxis
    {
        public string Title { get; set; } = "Category";
        public AxisOrientation Orientation { get; set; } = AxisOrientation.X;
        public List<string> Labels { get; set; } = new();
        public Color StrokeColor { get; set; } = Colors.Black;

        public void Draw(ICanvas canvas, RectF outerArea, RectF plotArea)
        {
            if (Orientation != AxisOrientation.X) return;

            canvas.StrokeColor = StrokeColor;
            canvas.StrokeSize = 1;

            canvas.DrawLine(plotArea.Left, plotArea.Bottom, plotArea.Right, plotArea.Bottom);

            float marginBottom = outerArea.Bottom - plotArea.Bottom;
            float labelHeight = marginBottom * 0.6f; 
            float titleHeight = marginBottom * 0.4f; 

            var labelArea = new RectF(plotArea.Left, plotArea.Bottom, plotArea.Width, labelHeight);
            var titleArea = new RectF(plotArea.Left, labelArea.Bottom, plotArea.Width, titleHeight);

            if (Labels?.Any() == true)
            {
                float step = plotArea.Width / Labels.Count;
                float x = plotArea.Left + step / 2;

                foreach (var label in Labels)
                {
                    canvas.FontSize = 12;
                    canvas.FontColor = Colors.Black;
                    canvas.DrawString(label, x, labelArea.Center.Y, HorizontalAlignment.Center);
                    x += step;
                }
            }

            if (!string.IsNullOrWhiteSpace(Title))
            {
                canvas.FontSize = 14;
                canvas.FontColor = Colors.Black;
                canvas.DrawString(
                    Title,
                    titleArea,
                    HorizontalAlignment.Center,
                    VerticalAlignment.Center
                );
            }
        }
    }
}
