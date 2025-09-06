using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Cores.Pipeline
{
    public interface IRenderPipeline : IDrawable
    {
        void Draw(ICanvas canvas, RectF dirtyRect, BaseChart chart);
        RectF CalculatePlotArea(ICanvas canvas, RectF dirtyRect, BaseChart chart);
    }
}
