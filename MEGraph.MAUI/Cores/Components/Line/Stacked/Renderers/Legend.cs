using MEGraph.MAUI.Legends;
using MEGraph.MAUI.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Cores.Components.Line.Stacked.Renderers
{
    public class Legend : ILegend
    {
        private readonly DefaultLegend _default = new();

        public void Draw(ICanvas canvas, RectF dirtyRect, IEnumerable<ISeries> series)
            => _default.Draw(canvas, dirtyRect, series);
    }
}
