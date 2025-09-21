using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Series.Area
{
    public class AreaSeries : ISeries
    {
        public string Name { get; set; } = "AreaSeries";
        public List<float> Data { get; set; } = new();
        public Color StrokeColor { get; set; } = Color.FromArgb("#5B9BD5");
        public Color? FillColor { get; set; } = Color.FromArgb("#D9E6F5");
        public float StrokeWidth { get; set; } = 3f;
        public void Draw(ICanvas canvas, RectF plotArea)
        {
            Draw(canvas, plotArea, null, null);
        }
        public void Draw(ICanvas canvas, RectF plotArea, float? globalMinY, float? globalMaxY)
        {
            if (Data == null || Data.Count < 2) return;

            float stepX = plotArea.Width / (Data.Count - 1);
            float maxY = globalMaxY ?? Data.Max();
            float minY = globalMinY ?? Data.Min();
            float rangeY = (maxY - minY == 0) ? 1 : maxY - minY;

            canvas.Antialias = true;

            var path = new PathF();

            // Điểm đầu tiên
            float x0 = plotArea.Left;
            float y0 = plotArea.Bottom - ((Data[0] - minY) / rangeY * plotArea.Height);
            path.MoveTo(x0, y0);

            // Các điểm dữ liệu tiếp theo
            for (int i = 1; i < Data.Count; i++)
            {
                float x = plotArea.Left + (i * stepX);
                float y = plotArea.Bottom - ((Data[i] - minY) / rangeY * plotArea.Height);
                path.LineTo(x, y);
            }

            path.LineTo(plotArea.Right, plotArea.Bottom);
            path.LineTo(plotArea.Left, plotArea.Bottom);
            path.Close();

            if (FillColor != null)
            {
                canvas.FillColor = FillColor;
                canvas.FillPath(path);
            }

            canvas.StrokeColor = StrokeColor;
            canvas.StrokeSize = StrokeWidth;
            canvas.StrokeLineJoin = LineJoin.Round;

            var linePath = new PathF();
            linePath.MoveTo(x0, y0);
            for (int i = 1; i < Data.Count; i++)
            {
                float x = plotArea.Left + (i * stepX);
                float y = plotArea.Bottom - ((Data[i] - minY) / rangeY * plotArea.Height);
                linePath.LineTo(x, y);
            }
            canvas.DrawPath(linePath);
        }

    }
}
