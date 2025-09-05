using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Axes
{
    public interface IAxis
    {
        string Title { get; set; }
        void Draw(ICanvas canvas, RectF dirtyRect);
    }
}
