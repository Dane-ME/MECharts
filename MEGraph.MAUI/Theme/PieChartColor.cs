using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Theme
{
    public static class PieChartColor
    {
        private static readonly List<Color> DefaultColors = new List<Color>
        {
            Color.FromArgb("#5B9BD5"), // Blue
            Color.FromArgb("#ED7D31"), // Orange
            Color.FromArgb("#A5A5A5"), // Gray
            Color.FromArgb("#FFC000"), // Gold
            Color.FromArgb("#4472C4"), // Dark Blue
            Color.FromArgb("#70AD47"), // Green
            Color.FromArgb("#255E91"), // Darker Blue
            Color.FromArgb("#9E480E"), // Brown
            Color.FromArgb("#636363"), // Dark Gray
            Color.FromArgb("#997300"), // Dark Gold
            Color.FromArgb("#264478"), // Navy Blue
            Color.FromArgb("#43682B")  // Dark Green
        };

        public static IEnumerable<Color> GetSliceColorsDistinct(ICollection<Color> existing, int count)
        {
            var palette = DefaultColors;
            var result = new List<Color>();

            foreach (var color in palette)
            {
                if (!existing.Contains(color))
                {
                    result.Add(color);
                    if (result.Count == count)
                        break;
                }
            }
            while (result.Count < count)
            {
                var newColor = GenerateRandomColor();
                if (!existing.Contains(newColor) && !result.Contains(newColor))
                {
                    result.Add(newColor);
                }
            }

            return result;
        }
        private static Color GenerateRandomColor()
        {
            var rnd = new Random();
            return Color.FromRgb(
                (byte)rnd.Next(256),
                (byte)rnd.Next(256),
                (byte)rnd.Next(256));
        }
    }
}
