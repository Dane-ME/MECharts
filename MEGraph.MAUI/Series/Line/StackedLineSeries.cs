using MEGraph.MAUI.Charts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MEGraph.MAUI.Series.Line
{
    public class StackedLineSeries : ISeries
    {
        public string Name { get; set; } = "StackedLineSeries";
        public List<float> Data { get; set; } = new();
        public Color StrokeColor { get; set; } = Color.FromArgb("#5B9BD5");
        public Color FillColor { get; set; } = Color.FromArgb("#5B9BD5");
        public float StrokeWidth { get; set; } = 3f;
        public float FillOpacity { get; set; } = 0.3f;

        // Vị trí trong stack
        public int StackOrder { get; set; } = 0;

        public void Draw(ICanvas canvas, RectF plotArea)
        {
            Draw(canvas, plotArea, null, null, null);
        }

        public void Draw(ICanvas canvas, RectF plotArea, float? globalMinY, float? globalMaxY, List<float>? baseValues)
        {
            if (Data == null || Data.Count < 2) return;
            if (baseValues == null || baseValues.Count != Data.Count) return;

            float stepX = plotArea.Width / (Data.Count - 1);

            // Sử dụng global min/max từ renderer
            float maxY = globalMaxY ?? Data.Max();
            float minY = globalMinY ?? Data.Min();
            float rangeY = (maxY - minY == 0) ? 1 : maxY - minY;

            canvas.Antialias = true;

            var path = new PathF();
            var fillPath = new PathF();

            // Điểm đầu tiên
            float x0 = plotArea.Left;
            float stackedY0 = baseValues[0] + Data[0]; // Tổng cộng dồn
            float baseY0 = baseValues[0]; // Base value

            float y0 = plotArea.Bottom - ((stackedY0 - minY) / rangeY * plotArea.Height);
            float baseY0Pos = plotArea.Bottom - ((baseY0 - minY) / rangeY * plotArea.Height);

            path.MoveTo(x0, y0);
            fillPath.MoveTo(x0, baseY0Pos);
            fillPath.LineTo(x0, y0);

            // Các điểm tiếp theo
            for (int i = 1; i < Data.Count; i++)
            {
                float x = plotArea.Left + (i * stepX);
                float stackedY = baseValues[i] + Data[i]; // Tổng cộng dồn
                float baseY = baseValues[i]; // Base value

                float y = plotArea.Bottom - ((stackedY - minY) / rangeY * plotArea.Height);
                float baseYPos = plotArea.Bottom - ((baseY - minY) / rangeY * plotArea.Height);

                path.LineTo(x, y);
                fillPath.LineTo(x, y);
            }

            // Line
            canvas.StrokeColor = StrokeColor;
            canvas.StrokeSize = StrokeWidth;
            canvas.DrawPath(path);
        }
    }
}
