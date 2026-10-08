using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Theme
{
    public static class PieChartColor
    {
        private static readonly List<Color> DefaultColors = ChartColors.Palette.ToList();

        public static IEnumerable<Color> GetSliceColorsDistinct(ICollection<Color> existing, int count)
        {
            return ChartColors.GetDistinctColors(existing, count);
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
