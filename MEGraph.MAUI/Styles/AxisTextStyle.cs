using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Styles
{
    public class AxisTextStyle
    {
        public string Text { get; set; } = string.Empty;
        public float FontSize { get; set; } = 12;
        public Color FontColor { get; set; } = Colors.Black;
        public float Margin { get; set; } = 10;
    }
    public class AxisTitle : AxisTextStyle
    {
        public AxisTitle(string text)
        {
            Text = text;
        }
    }
    public class AxisLabel : AxisTextStyle
    {
        public AxisLabel(string text)
        {
            Text = text;
        }
    }
}
