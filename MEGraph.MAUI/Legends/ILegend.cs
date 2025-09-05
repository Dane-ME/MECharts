using MEGraph.MAUI.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Legends
{
    public interface ILegend
    {
        void Draw(ICanvas canvas, RectF dirtyRect, IEnumerable<ISeries> series);
    }
}
