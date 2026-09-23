using MEGraph.MAUI.Charts;
using MEGraph.MAUI.Series;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MEGraph.MAUI.Series.Line
{
    public class LineSeries : ISeries
    {
        public string Name { get; set; } = "LineSeries";
        public List<float> Data { get; set; } = new();
        public Color StrokeColor { get; set; } = Color.FromArgb("#5B9BD5");
        public float StrokeWidth { get; set; } = 3f;

        // START - 2.4.0 - ADD - Smooth curved line support
        public bool IsSmooth { get; set; } = false;
        public float SmoothTension { get; set; } = 0.25f;
        // END - 2.4.0 - ADD

        // ISeries.Color — map tới StrokeColor cho Legend
        public Color Color => StrokeColor;
        public float GetMinY() => Data?.Any() == true ? Data.Min() : 0f;
        public float GetMaxY() => Data?.Any() == true ? Data.Max() : 0f;
        public void Draw(ICanvas canvas, RectF plotArea)
        {
            Draw(canvas, plotArea, null, null, 1f);
        }
        public void Draw(ICanvas canvas, RectF plotArea, float? globalMinY, float? globalMaxY)
        {
            Draw(canvas, plotArea, globalMinY, globalMaxY, 1f);
        }
        public void Draw(ICanvas canvas, RectF plotArea, float? globalMinY, float? globalMaxY, float progress)
        {
            if (Data == null || Data.Count < 2) return;

            float stepX = plotArea.Width / (Data.Count - 1);
            float maxY = globalMaxY ?? Data.Max();
            float minY = globalMinY ?? Data.Min();
            float rangeY = (maxY - minY == 0) ? 1 : maxY - minY;

            canvas.Antialias = true;

            var points = new PointF[Data.Count];
            for (int i = 0; i < Data.Count; i++)
            {
                float x = plotArea.Left + (i * stepX);
                float y = plotArea.Bottom - ((Data[i] - minY) / rangeY * plotArea.Height);
                points[i] = new PointF(x, y);
            }

            var path = new PathF();
            path.MoveTo(points[0]);

            if (IsSmooth && points.Length > 2)
            {
                int n = points.Length;
                float[] dx = new float[n - 1];
                float[] dy = new float[n - 1];
                float[] m = new float[n - 1];

                for (int i = 0; i < n - 1; i++)
                {
                    dx[i] = points[i + 1].X - points[i].X;
                    dy[i] = points[i + 1].Y - points[i].Y;
                    m[i] = dx[i] == 0 ? 0 : dy[i] / dx[i];
                }

                // Tangents at points
                float[] tangents = new float[n];
                tangents[0] = m[0];
                tangents[n - 1] = m[n - 2];

                for (int i = 1; i < n - 1; i++)
                {
                    if (m[i - 1] * m[i] <= 0)
                    {
                        tangents[i] = 0;
                    }
                    else
                    {
                        tangents[i] = (m[i - 1] + m[i]) / 2f;
                    }
                }

                // Fritsch-Carlson condition to preserve monotonicity (no overshoot)
                for (int i = 0; i < n - 1; i++)
                {
                    if (Math.Abs(dy[i]) < 1e-5f)
                    {
                        tangents[i] = 0;
                        tangents[i + 1] = 0;
                    }
                    else
                    {
                        float alpha = tangents[i] / m[i];
                        float beta = tangents[i + 1] / m[i];
                        float s = alpha * alpha + beta * beta;
                        if (s > 9f)
                        {
                            float tau = 3f / MathF.Sqrt(s);
                            tangents[i] = tau * alpha * m[i];
                            tangents[i + 1] = tau * beta * m[i];
                        }
                    }
                }

                for (int i = 0; i < n - 1; i++)
                {
                    float h = dx[i];
                    float cp1x = points[i].X + h / 3f;
                    float cp1y = points[i].Y + tangents[i] * (h / 3f);
                    float cp2x = points[i + 1].X - h / 3f;
                    float cp2y = points[i + 1].Y - tangents[i + 1] * (h / 3f);

                    // Clamp to plotArea vertically
                    cp1y = Math.Clamp(cp1y, plotArea.Top, plotArea.Bottom);
                    cp2y = Math.Clamp(cp2y, plotArea.Top, plotArea.Bottom);

                    path.CurveTo(cp1x, cp1y, cp2x, cp2y, points[i + 1].X, points[i + 1].Y);
                }
            }
            else
            {
                for (int i = 1; i < points.Length; i++)
                {
                    path.LineTo(points[i]);
                }
            }

            bool shouldClip = progress < 1f;
            if (shouldClip)
            {
                canvas.SaveState();
                float clipWidth = Math.Max(0f, plotArea.Width * Math.Clamp(progress, 0f, 1f));
                canvas.ClipRectangle(plotArea.Left, plotArea.Top, clipWidth, plotArea.Height);
            }

            canvas.StrokeColor = StrokeColor;
            canvas.StrokeSize = StrokeWidth;
            canvas.StrokeLineJoin = LineJoin.Round;
            canvas.DrawPath(path);

            if (shouldClip)
            {
                canvas.RestoreState();
            }
        }
    }
}
