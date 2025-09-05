using MEGraph.MAUI.Cores;
using MEGraph.MAUI.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Charts
{
    public class LineChart : BaseChart
    {
        public LineSeries Series1 { get; private set; }

        public LineChart()
        {
            Series1 = new LineSeries();
            Series.Add(Series1);
        }

        public void SetData(IEnumerable<float> data)
        {
            Series1.Data = data.ToList();
            Refresh();
        }
    }
}
