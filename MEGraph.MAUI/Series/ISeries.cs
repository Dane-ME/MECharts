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
        void Draw(ICanvas canvas, RectF plotArea);
    }
}