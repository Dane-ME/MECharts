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
        string Title { get; set; }
        AxisOrientation Orientation { get; set; }
        void Draw(ICanvas canvas, RectF outerArea, RectF plotArea);
    }
}
