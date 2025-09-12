using MEGraph.MAUI.Theme;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Series.Pie
{
    public class PieSeries : ISeries
    {
        public string Name { get; set; } = "PieChart";
        public List<float> Data { get; set; } = new();
        public List<Color> Colors { get; set; } = new();
        public Color? Stroke { get; set; } = Color.FromArgb("#FFFFFF");
        public float StrokeWidth { get; set; } = 0f;
        public bool Clockwise { get; set; } = false;
        public bool Closed { get; set; } = false;

        public void Draw(ICanvas canvas, RectF plotArea)
        {
            var (dataCount, colorCount) = (Data.Count, Colors.Count);
            var cx = plotArea.Left + plotArea.Width / 2f;
            var cy = plotArea.Top + plotArea.Height / 2f;
            var diameter = Math.Min(plotArea.Width, plotArea.Height);
            var radius = diameter / 2f;
            //START - 2.1.3 - ADD - hit-test, cache and state hover  
            _radius = radius;
            _cx = cx;
            _cy = cy;
            //END - 2.1.3 - ADD - hit-test, cache and state hover  
            var total = Data.Sum();
            if (total <= 0) return;

            if (dataCount > colorCount)
            {
                int fillEmpty = dataCount - colorCount;
                Colors.AddRange(PieChartColor.GetSliceColorsDistinct(Colors, fillEmpty));
            }
            //START - 2.1.3 - ADD - hit-test, cache and state hover  
            _slicesRangeCcw.Clear();
            float cum = 0f;

            for (int i = 0; i < dataCount; i++)
            {
                var value = Math.Max(0, Data[i]);
                if (value <= 0) { _slicesRangeCcw.Add((cum, cum)); continue; }

                var sweepCcw = (value / total) * 360f;
                _slicesRangeCcw.Add((cum, cum + sweepCcw));
                cum += sweepCcw;
            }
            //END - 2.1.3 - ADD - hit-test, cache and state hover
            float startAngle = -90f; // Bắt đầu từ 12 giờ

            for (int i = 0; i < dataCount; i++)
            {
                var value = Math.Max(0, Data[i]);
                if (value <= 0) continue;
                var sweepAngle = (value / total) * 360f;
                var actualSweepAngle = Clockwise ? -sweepAngle : sweepAngle;
                canvas.FillColor = Colors[i];
                var rect = new RectF(cx - radius, cy - radius, diameter, diameter);
                var path = new PathF();
                path.MoveTo(cx, cy); // Tâm
                var startRad = startAngle * MathF.PI / 180f;
                var startX = cx + radius * MathF.Cos(startRad);
                var startY = cy + radius * MathF.Sin(startRad);
                path.LineTo(startX, startY);

                // Thêm cung - thử cách khác
                // Nếu AddArc không hoạt động đúng, vẽ bằng nhiều đường thẳng nhỏ
                int numPoints = Math.Max(2, (int)Math.Abs(sweepAngle) / 5);
                for (int j = 1; j <= numPoints; j++)
                {
                    var angle = startAngle + (actualSweepAngle * j / numPoints);
                    var rad = angle * MathF.PI / 180f;
                    var x = cx + radius * MathF.Cos(rad);
                    var y = cy + radius * MathF.Sin(rad);
                    path.LineTo(x, y);
                }

                path.LineTo(cx, cy);
                path.Close();

                canvas.FillPath(path, WindingMode.NonZero);

                startAngle += actualSweepAngle;
            }
        }

        //START - 2.1.3 - ADD - hit-test, cache and state hover   

        public int SelectIndex { get; set; } = -1;
        private float _cx, _cy, _radius;
        private readonly List<(float FromDeg, float ToDeg)> _slicesRangeCcw = new();
        private static float Nomalize360(float deg)
        {
            float a = deg % 360f;
            return a < 0 ? a + 360f : a;
        }
        private static float AngleFrom12Ccw(float x, float y, float cx, float cy)
        {
            float angle = MathF.Atan2(y - cy, x - cx) * 180f / MathF.PI + 90f;
            float from12 = angle - (-90f);
            return Nomalize360(angle);
        }
        public int HitTest(PointF p)
        {
            if (_radius <= 0 || _slicesRangeCcw.Count != Data.Count) return -1;

            var dx = p.X - _cx;
            var dy = p.Y - _cy;
            var dist = MathF.Sqrt(dx * dx + dy * dy);
            if (dist > _radius ||  dist < 0f) return -1; // Ngoài bán kính
            var angle = AngleFrom12Ccw(p.X, p.Y, _cx, _cy);
            for (int i = 0; i < _slicesRangeCcw.Count; i++)
            {
                var (from, to) = _slicesRangeCcw[i];
                if (angle >= from && angle < to) return i;
            }
            return -1;
        }
        public (float cx, float cy, float radius) GetGeometry() => (_cx, _cy, _radius);
        public float GetSliceMidAngle(int index)
        {
            if (index < 0 || index >= _slicesRangeCcw.Count) return 0f;
            var (from, to) = _slicesRangeCcw[index];
            return (from + to) * 0.5f;
        }
        //END - 2.1.3 - ADD - hit-test, cache and state hover

    }
}
