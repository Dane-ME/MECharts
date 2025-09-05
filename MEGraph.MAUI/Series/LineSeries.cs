using MEGraph.MAUI.Charts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MEGraph.MAUI.Series
{
    public class LineSeries : ISeries
    {
        public string Name { get; set; } = "LineSeries";
        public List<float> Data { get; set; } = new();
        public Color StrokeColor { get; set; } = Color.FromArgb("#5B9BD5");
        public float StrokeWidth { get; set; } = 3f;

        /// <summary>
        /// Vẽ series trong plotArea (không đè lên Title, Axis).
        /// </summary>
        public void Draw(ICanvas canvas, RectF plotArea)
        {
            if (Data == null || Data.Count < 2) return;

            float stepX = plotArea.Width / (Data.Count - 1);
            float maxY = Data.Max();
            float minY = Data.Min();
            float rangeY = (maxY - minY == 0) ? 1 : maxY - minY;

            for (int i = 0; i < Data.Count - 1; i++)
            {
                float x1 = plotArea.Left + (i * stepX);
                float y1 = plotArea.Bottom - ((Data[i] - minY) / rangeY * plotArea.Height);

                float x2 = plotArea.Left + ((i + 1) * stepX);
                float y2 = plotArea.Bottom - ((Data[i + 1] - minY) / rangeY * plotArea.Height);

                canvas.StrokeColor = StrokeColor;
                canvas.StrokeSize = StrokeWidth;
                canvas.DrawLine(x1, y1, x2, y2);
            }
        }
    }
}
