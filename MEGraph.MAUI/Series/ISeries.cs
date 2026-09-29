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
        void Draw(ICanvas canvas, RectF plotArea);
    }
}