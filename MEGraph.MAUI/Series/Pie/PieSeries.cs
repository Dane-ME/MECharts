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
            var total = Data.Sum();
            if (total <= 0) return;

            // Bổ sung màu nếu thiếu
            if (dataCount > colorCount)
            {
                int fillEmpty = dataCount - colorCount;
                Colors.AddRange(PieChartColor.GetSliceColorsDistinct(Colors, fillEmpty));
            }

            float startAngle = -90f; // Bắt đầu từ 12 giờ

            for (int i = 0; i < dataCount; i++)
            {
                var value = Math.Max(0, Data[i]);
                if (value <= 0) continue;

                var sweepAngle = (value / total) * 360f;

                // Điều chỉnh hướng quay
                var actualSweepAngle = Clockwise ? -sweepAngle : sweepAngle;

                // Màu tô lát
                canvas.FillColor = Colors[i];

                // Sử dụng FillArc trực tiếp nếu có sẵn, hoặc tạo path đơn giản
                var rect = new RectF(cx - radius, cy - radius, diameter, diameter);

                // Tạo path cho pie slice
                var path = new PathF();
                path.MoveTo(cx, cy); // Tâm

                // Điểm bắt đầu
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

                // Quay về tâm
                path.LineTo(cx, cy);
                path.Close();

                // Tô màu
                canvas.FillPath(path, WindingMode.NonZero);

                // Cập nhật góc bắt đầu
                startAngle += actualSweepAngle;
            }
        }

    }
}
