using MEGraph.MAUI.Legends;
using MEGraph.MAUI.Series;
using System.Collections.Generic;

namespace MEGraph.MAUI.Cores.Components.Line.Standard.Renderers
{
    public class Legend : ILegend
    {
        private readonly DefaultLegend _default = new();

        public void Draw(ICanvas canvas, RectF dirtyRect, IEnumerable<ISeries> series)
            => _default.Draw(canvas, dirtyRect, series);
    }
}
