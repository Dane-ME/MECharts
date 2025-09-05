using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Series
{
    public class LineSeries : ISeries
    {
        public string Name { get; set; } = "LineSeries";
        public List<float> Data { get; set; } = new ();
        public Color StrokeColor { get; set; } = Colors.Blue;
        public float StrokeWidth { get; set; } = 2f;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (Data == null || Data.Count < 2) return;

            float stepX = dirtyRect.Width / (Data.Count - 1);
            float maxY = Data.Max();
            float minY = Data.Min();

            for (int i = 0; i < Data.Count - 1; i++)
            {
                float x1 = i * stepX;
                float y1 = dirtyRect.Height - ((Data[i] - minY) / (maxY - minY) * dirtyRect.Height);

                float x2 = (i + 1) * stepX;
                float y2 = dirtyRect.Height - ((Data[i + 1] - minY) / (maxY - minY) * dirtyRect.Height);

                canvas.StrokeColor = StrokeColor;
                canvas.StrokeSize = StrokeWidth;
                canvas.DrawLine(x1, y1, x2, y2);
            }
        }
    }
}
