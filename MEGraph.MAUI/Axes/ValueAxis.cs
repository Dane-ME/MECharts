using MEGraph.MAUI.Styles;
using Microsoft.Maui.Graphics;
using static MEGraph.MAUI.Axes.IAxis;

namespace MEGraph.MAUI.Axes
{
    public class ValueAxis : IAxis
    {
        public AxisTitle Title { get; set; } = new AxisTitle("Value");
        public List<AxisLabel> Labels { get; set; } = new();
        public AxisOrientation Orientation { get; set; } = AxisOrientation.Y;
        public Color StrokeColor { get; set; } = Colors.Black;
        public float StrokeSize { get; set; } = 1;

        public void Draw(ICanvas canvas, RectF outerArea, RectF plotArea)
        {
            canvas.StrokeColor = StrokeColor;
            canvas.StrokeSize = StrokeSize;

            if (Orientation == AxisOrientation.Y)
            {
                // Vẽ trục Y
                canvas.DrawLine(plotArea.Left, plotArea.Top, plotArea.Left, plotArea.Bottom);

                // Khu vực cho Title
                var titleArea = new RectF(
                    outerArea.Left,
                    plotArea.Top,
                    plotArea.Left - outerArea.Left,
                    plotArea.Height
                );

                if (!string.IsNullOrWhiteSpace(Title?.Text))
                {
                    canvas.SaveState();
                    canvas.Translate(titleArea.Center.X, titleArea.Center.Y);
                    canvas.Rotate(-90);
                    canvas.FontSize = Title.FontSize;
                    canvas.FontColor = Title.FontColor;
                    canvas.DrawString(Title.Text, 0, 0, HorizontalAlignment.Center);
                    canvas.RestoreState();
                }
            }
            else if (Orientation == AxisOrientation.X)
            {
                // Vẽ trục X
                canvas.DrawLine(plotArea.Left, plotArea.Bottom, plotArea.Right, plotArea.Bottom);

                var titleArea = new RectF(
                    plotArea.Left,
                    plotArea.Bottom,
                    plotArea.Width,
                    outerArea.Bottom - plotArea.Bottom
                );

                if (!string.IsNullOrWhiteSpace(Title?.Text))
                {
                    canvas.FontSize = Title.FontSize;
                    canvas.FontColor = Title.FontColor;
                    canvas.DrawString(
                        Title.Text,
                        titleArea,
                        HorizontalAlignment.Center,
                        VerticalAlignment.Center
                    );
                }
            }
        }
    }
}
