using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GraphicsFont = Microsoft.Maui.Graphics.Font;

namespace MEGraph.MAUI.Styles
{
    public class Text
    {
        public string Content { get; set; } = string.Empty;
        public float FontSize { get; set; } = 12;
        public Color FontColor { get; set; } = Colors.Black;
        public float Margin { get; set; } = 10;
        public GraphicsFont Font
        {
            get
            {
                int weight = IsBold ? 700 : 400; // 700 ~ Bold, 400 ~ Regular
                var slant = IsItalic ? FontStyleType.Italic : FontStyleType.Normal;

                return new GraphicsFont("Times New Roman", weight, slant);
            }
        }
        public bool IsBold { get; set; } = false;
        public bool IsItalic { get; set; } = false;
        public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Center;
        public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Center;
        public float Rotation { get; set; } = 0f; 
        public bool IsVisible { get; set; } = true;
    }

    public class AxisTitle : Text
    {
        public AxisTitle(string content = "")
        {
            Content = content;
        }
    }

    public class AxisLabel : Text
    {
        public AxisLabel(string content = "")
        {
            Content = content;
        }
    }
}