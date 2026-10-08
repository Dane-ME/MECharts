using MEGraph.MAUI.Series;
using System.Collections.Generic;

namespace MEGraph.MAUI.Legends
{
    public class DefaultLegend : ILegend
    {
        public float SwatchSize { get; set; } = 10f;
        public float FontSize { get; set; } = 12f;
        public float ItemSpacing { get; set; } = 18f;
        public float RightPadding { get; set; } = 8f;
        public float TopPadding { get; set; } = 8f;

        public void Draw(ICanvas canvas, RectF dirtyRect, IEnumerable<ISeries> series)
        {
            if (series == null) return;

            // Ước tính width lớn nhất theo số ký tự thay vì đo từng series qua JNI mỗi frame
            int maxChars = 0;
            var seriesList = new List<ISeries>(series);
            for (int i = 0; i < seriesList.Count; i++)
            {
                var name = seriesList[i]?.Name;
                if (name != null && name.Length > maxChars) maxChars = name.Length;
            }
            float maxTextWidth = Math.Max(24f, maxChars * (FontSize * 0.65f));

            float colWidth = SwatchSize + 6f + maxTextWidth + RightPadding;
            float x = dirtyRect.Right - colWidth;
            float y = dirtyRect.Top + TopPadding;

            foreach (var s in seriesList)
            {
                // Swatch màu — căn giữa dọc theo ItemSpacing
                canvas.FillColor = s.Color;
                canvas.FillRectangle(x, y + (ItemSpacing - SwatchSize) / 2f, SwatchSize, SwatchSize);

                // Label tên series
                canvas.FontColor = Colors.Black;
                canvas.FontSize = FontSize;
                canvas.DrawString(s.Name, x + SwatchSize + 4f, y, HorizontalAlignment.Left);

                y += ItemSpacing;
            }
        }
    }
}
