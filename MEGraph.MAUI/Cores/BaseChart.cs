using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Legends;
using MEGraph.MAUI.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Cores
{
    public abstract class BaseChart : GraphicsView
    {
        public List<ISeries> Series { get; } = new();
        //public List<IAxis> Axes { get; } = new();
        //public ILegend Legend { get; set; }
        //public ChartOptions Options { get; set; } = new ChartOptions();
        public string Title { get; set; }

        protected BaseChart()
        {
            Drawable = new ChartDrawable(this);
        }

        public void Refresh() => this.Invalidate();
    }
}
