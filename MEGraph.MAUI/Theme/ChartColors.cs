using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;

namespace MEGraph.MAUI.Theme
{
    public static class ChartColors
    {
        public static readonly Color Blue = Color.FromArgb("#4e79a7");
        public static readonly Color Orange = Color.FromArgb("#f28e2c");
        public static readonly Color Red = Color.FromArgb("#e15759");
        public static readonly Color Teal = Color.FromArgb("#76b7b2");
        public static readonly Color Green = Color.FromArgb("#59a14f");
        public static readonly Color Yellow = Color.FromArgb("#edc949");
        public static readonly Color Purple = Color.FromArgb("#af7aa1");
        public static readonly Color Pink = Color.FromArgb("#ff9da7");
        public static readonly Color Brown = Color.FromArgb("#9c755f");
        public static readonly Color Gray = Color.FromArgb("#bab0ab");

        public static readonly IReadOnlyList<Color> Palette = new List<Color>
        {
            Blue,
            Orange,
            Red,
            Teal,
            Green,
            Yellow,
            Purple,
            Pink,
            Brown,
            Gray
        };

        public static Color DefaultPrimaryColor => Blue;

        public static Color GetColor(int index)
        {
            if (index < 0) index = 0;
            return Palette[index % Palette.Count];
        }

        public static IEnumerable<Color> GetDistinctColors(ICollection<Color> existing, int count)
        {
            var result = new List<Color>();

            foreach (var color in Palette)
            {
                if (!existing.Contains(color))
                {
                    result.Add(color);
                    if (result.Count == count)
                        return result;
                }
            }

            var rnd = new Random();
            while (result.Count < count)
            {
                var newColor = Color.FromRgb(
                    (byte)rnd.Next(256),
                    (byte)rnd.Next(256),
                    (byte)rnd.Next(256));

                if (!existing.Contains(newColor) && !result.Contains(newColor))
                {
                    result.Add(newColor);
                }
            }

            return result;
        }
    }
}
