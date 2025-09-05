using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MEGraph.MAUI.Axes.IAxis;

namespace MEGraph.MAUI.Axes
{
    public class ValueAxis : IAxis
    {
        public string Title { get; set; } = "Value";
        public AxisOrientation Orientation { get; set; } = AxisOrientation.Y;
        public Color StrokeColor { get; set; } = Colors.Black;
        public void Draw(ICanvas canvas, RectF outerArea, RectF plotArea)
        {
            canvas.StrokeColor = StrokeColor;
            canvas.StrokeSize = 1;

            if (Orientation == AxisOrientation.Y)
            {
                canvas.DrawLine(plotArea.Left, plotArea.Top, plotArea.Left, plotArea.Bottom);

                var titleArea = new RectF(
                    outerArea.Left,
                    plotArea.Top,
                    plotArea.Left - outerArea.Left, 
                    plotArea.Height
                );

                canvas.SaveState();
                canvas.Translate(titleArea.Center.X, titleArea.Center.Y);
                canvas.Rotate(-90); 
                canvas.FontSize = 14;
                canvas.FontColor = Colors.Black;
                canvas.DrawString(Title, 0, 0, HorizontalAlignment.Center);
                canvas.RestoreState();
            }
            else if (Orientation == AxisOrientation.X)
            {
                canvas.DrawLine(plotArea.Left, plotArea.Bottom, plotArea.Right, plotArea.Bottom);

                var titleArea = new RectF(
                    plotArea.Left,
                    plotArea.Bottom,
                    plotArea.Width,
                    outerArea.Bottom - plotArea.Bottom 
                );

                canvas.FontSize = 14;
                canvas.FontColor = Colors.Black;
                canvas.DrawString(Title, titleArea, HorizontalAlignment.Center, VerticalAlignment.Center);
            }
        }

    }
}
