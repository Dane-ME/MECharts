using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Series
{
    public interface ISeries
    {
        string Name { get; }
        /// <summary>Màu đại diện cho series — dùng để vẽ legend swatch.</summary>
        Color Color { get; }
        /// <summary>Cờ xác định series có được vẽ hay không (mặc định là true).</summary>
        bool IsVisible { get; set; }
        /// <summary>Id của trục Y gán với series (hỗ trợ Dual Y-axis hoặc Multi-axis). Mặc định null (dùng trục chính).</summary>
        string? AxisId { get; set; }
        void Draw(ICanvas canvas, RectF plotArea);
    }
}