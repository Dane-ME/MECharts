using MEGraph.MAUI.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Axes
{
    public enum AxisOrientation
    {
        X,
        Y
    }
    public interface IAxis
    {
        AxisTitle Title { get; set; }
        List<AxisLabel> Labels { get; set; }
        AxisOrientation Orientation { get; set; }
        Color StrokeColor { get; set; }
        void Draw(ICanvas canvas, RectF outerArea, RectF plotArea);
    }
}
