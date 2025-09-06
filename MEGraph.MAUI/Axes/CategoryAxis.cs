using MEGraph.MAUI.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Axes
{
    public class CategoryAxis : IAxis
    {
        public AxisTitle Title { get; set; } = new AxisTitle("Category");
        public AxisOrientation Orientation { get; set; } = AxisOrientation.X;
        public List<AxisLabel> Labels { get; set; } = new();
        public Color StrokeColor { get; set; } = Colors.Black;

        public void Draw(ICanvas canvas, RectF outerArea, RectF plotArea)
        {
            if (Orientation != AxisOrientation.X) return;

            canvas.StrokeColor = StrokeColor;
            canvas.StrokeSize = 1;

            canvas.DrawLine(plotArea.Left, plotArea.Bottom, plotArea.Right, plotArea.Bottom);

            float maxLabelHeight = 0;
            foreach (var lbl in Labels)
            {
                var size = canvas.GetStringSize(lbl.Text, Microsoft.Maui.Graphics.Font.Default, lbl.FontSize);
                maxLabelHeight = Math.Max(maxLabelHeight, size.Height + lbl.Margin);
            }

            float titleHeight = 0;
            if (!string.IsNullOrWhiteSpace(Title?.Text))
            {
                var titleSize = canvas.GetStringSize(Title.Text, Microsoft.Maui.Graphics.Font.Default, Title.FontSize);
                titleHeight = titleSize.Height + Title.Margin;
            }

            var labelArea = new RectF(plotArea.Left, plotArea.Bottom + Labels.First().Margin, plotArea.Width, maxLabelHeight);
            var titleArea = new RectF(plotArea.Left, labelArea.Bottom, plotArea.Width, titleHeight);

            if (Labels?.Any() == true)
            {
                float stepX = plotArea.Width / (Labels.Count - 1);
                for (int i = 0; i < Labels.Count; i++)
                {
                    float x = plotArea.Left + i * stepX;
                    var lbl = Labels[i];

                    canvas.FontSize = lbl.FontSize;
                    canvas.FontColor = lbl.FontColor;
                    canvas.DrawString(lbl.Text, x, labelArea.Center.Y, HorizontalAlignment.Center);
                }
            }

            if (!string.IsNullOrWhiteSpace(Title?.Text))
            {
                canvas.FontSize = Title.FontSize;
                canvas.FontColor = Title.FontColor;
                canvas.DrawString(
                    Title.Text,
                    titleArea,
                    HorizontalAlignment.Center,
                    VerticalAlignment.Top
                );
            }
        }

    }
}
