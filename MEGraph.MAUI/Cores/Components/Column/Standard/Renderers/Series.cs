using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Series.Column;
using Microsoft.Maui.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace MEGraph.MAUI.Cores.Components.Column.Standard.Renderers
{
    public class Series
    {
        public void Draw(ICanvas canvas, RectF plotArea, BaseChart? baseChart)
        {
            if (baseChart == null) return;

            var columnSeriesList = baseChart.Series.OfType<ColumnSeries>().Where(s => s.IsVisible).ToList();
            if (!columnSeriesList.Any()) return;

            var yAxes = baseChart.Axes?.Where(a => a.Orientation == AxisOrientation.Y).ToList();
            var defaultYAxis = yAxes?.FirstOrDefault(a => a.AxisPosition == AxisPosition.Left) ?? yAxes?.FirstOrDefault();

            var categoryAxis = baseChart.Axes?.FirstOrDefault(a => a.Orientation == AxisOrientation.X);
            int? totalPoints = null;
            if (categoryAxis != null && categoryAxis.TotalPoints > 0)
            {
                totalPoints = categoryAxis.TotalPoints;
            }
            else if (columnSeriesList.Any())
            {
                totalPoints = columnSeriesList.Max(s => s.Data?.Count ?? 0);
            }

            int totalSeriesCount = columnSeriesList.Count;

            for (int i = 0; i < columnSeriesList.Count; i++)
            {
                var colSeries = columnSeriesList[i];

                // Tìm trục Y tương ứng
                var targetAxis = !string.IsNullOrEmpty(colSeries.AxisId)
                    ? yAxes?.FirstOrDefault(a => a.Id == colSeries.AxisId)
                    : defaultYAxis;

                float? minY = targetAxis?.MinValue;
                float? maxY = targetAxis?.MaxValue;

                if (minY == null || maxY == null)
                {
                    minY = colSeries.GetMinY();
                    maxY = colSeries.GetMaxY();
                }

                colSeries.Draw(
                    canvas,
                    plotArea,
                    minY,
                    maxY,
                    baseChart.AnimationProgress,
                    i,
                    totalSeriesCount,
                    totalPoints
                );
            }
        }
    }
}
