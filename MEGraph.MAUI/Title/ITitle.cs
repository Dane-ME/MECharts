using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Title
{
    public interface ITitle
    {
        void Draw(ICanvas canvas, RectF dirtyRect, string title);
    }
}
