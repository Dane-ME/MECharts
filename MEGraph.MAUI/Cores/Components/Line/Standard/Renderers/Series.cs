using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Series;
using MEGraph.MAUI.Series.Line;
using System.Collections.Generic;
using System.Linq;

namespace MEGraph.MAUI.Cores.Components.Line.Standard.Renderers
{
    public class Series
    {
        public void Draw(ICanvas canvas, RectF plotArea, BaseChart? baseChart)
        {
            if (baseChart == null) return;

            IAxis? defaultValueAxis = null;
            IAxis? categoryAxis = null;
            int maxDataPoints = 0;

            if (baseChart.Axes != null)
            {
                for (int i = 0; i < baseChart.Axes.Count; i++)
                {
                    var a = baseChart.Axes[i];
                    if (a.Orientation == MEGraph.MAUI.Axes.AxisOrientation.Y)
                    {
                        if (a.AxisPosition == MEGraph.MAUI.Axes.AxisPosition.Left || defaultValueAxis == null)
                            defaultValueAxis = a;
                    }
                    else if (a.Orientation == MEGraph.MAUI.Axes.AxisOrientation.X && categoryAxis == null)
                    {
                        categoryAxis = a;
                    }
                }
            }

            for (int i = 0; i < baseChart.Series.Count; i++)
            {
                if (baseChart.Series[i] is LineSeries ls && ls.Data != null && ls.Data.Count > maxDataPoints)
                    maxDataPoints = ls.Data.Count;
            }

            int? totalPoints = (categoryAxis != null && categoryAxis.TotalPoints > 0)
                ? categoryAxis.TotalPoints
                : (maxDataPoints > 0 ? maxDataPoints : null);

            for (int i = 0; i < baseChart.Series.Count; i++)
            {
                var series = baseChart.Series[i];
                if (!series.IsVisible) continue;

                if (series is LineSeries lineSeries)
                {
                    IAxis? targetAxis = defaultValueAxis;
                    if (!string.IsNullOrEmpty(lineSeries.AxisId) && baseChart.Axes != null)
                    {
                        for (int a = 0; a < baseChart.Axes.Count; a++)
                        {
                            if (baseChart.Axes[a].Id == lineSeries.AxisId)
                            {
                                targetAxis = baseChart.Axes[a];
                                break;
                            }
                        }
                    }

                    float? minY = targetAxis?.MinValue;
                    float? maxY = targetAxis?.MaxValue;

                    if (minY == null || maxY == null)
                    {
                        minY = lineSeries.GetMinY();
                        maxY = lineSeries.GetMaxY();
                    }

                    lineSeries.Draw(canvas, plotArea, minY, maxY, baseChart.AnimationProgress, totalPoints);
                }
                else
                {
                    series.Draw(canvas, plotArea);
                }
            }
        }
    }
}
// END - 2.4.0 - EDIT - Fix duplicate min/max
