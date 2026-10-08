using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Axes.Area;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Cores.Components.Area.Standard.Renderers
{
    public class Axes
    {
        public void Draw(ICanvas canvas, RectF outerArea, RectF plotArea, BaseChart baseChart)
        {
            if (baseChart?.Axes == null) return;

            foreach (var axis in baseChart.Axes)
            {
                if (axis.ChartId == baseChart.Id || string.IsNullOrEmpty(axis.ChartId))
                {
                    if (axis.Orientation == AxisOrientation.X || axis.Orientation == AxisOrientation.Y)
                    {
                        axis.Draw(canvas, outerArea, plotArea);
                    }
                }
            }
        }

        public (float Left, float Top, float Right, float Bottom) CalculateMargins(ICanvas canvas, RectF outerArea, ObservableCollection<IAxis> axes, string? chartId = null)
        {
            float top = 0, left = 0, right = 20, bottom = 0;

            foreach (var axis in axes)
            {
                if (chartId != null && !string.IsNullOrEmpty(axis.ChartId) && axis.ChartId != chartId)
                    continue;

                if (axis is Category categoryAxis && categoryAxis.Orientation == AxisOrientation.X)
                {
                    bottom = CalculateCategoryAxisMargin(canvas, categoryAxis, bottom);
                }
                else if (axis is Value valueAxis && valueAxis.Orientation == AxisOrientation.Y)
                {
                    if (valueAxis.AxisPosition == AxisPosition.Right)
                    {
                        right = CalculateValueAxisMargin(canvas, valueAxis, right);
                    }
                    else
                    {
                        left = CalculateValueAxisMargin(canvas, valueAxis, left);
                    }
                }
            }

            return (left, top, right, bottom);
        }

        private float CalculateCategoryAxisMargin(ICanvas canvas, Category categoryAxis, float currentBottom)
        {
            if (categoryAxis.Labels?.Any() != true && string.IsNullOrWhiteSpace(categoryAxis.Title?.Content))
                return currentBottom;

            float maxFontSize = 12f;
            float maxLabelMargin = 4f;

            if (categoryAxis.Labels != null && categoryAxis.Labels.Count > 0)
            {
                var first = categoryAxis.Labels[0];
                maxFontSize = first.FontSize > 0 ? first.FontSize : 12f;
                maxLabelMargin = first.Margin;
            }

            float estimatedLabelHeight = maxFontSize * 1.3f;
            float titleHeight = string.IsNullOrWhiteSpace(categoryAxis.Title?.Content)
                ? 0f
                : (categoryAxis.Title.FontSize > 0 ? categoryAxis.Title.FontSize * 1.3f : 16f) + categoryAxis.Title.Margin;

            float padding = 5f;
            float calculated = estimatedLabelHeight + maxLabelMargin + padding + titleHeight;
            return Math.Max(currentBottom, (float)Math.Ceiling(calculated));
        }

        private float CalculateValueAxisMargin(ICanvas canvas, Value valueAxis, float currentLeft)
        {
            if (valueAxis.Labels?.Any() != true && string.IsNullOrWhiteSpace(valueAxis.Title?.Content)) return currentLeft;

            int maxChars = 0;
            float fontSize = 12f;
            float labelMargin = 6f;

            if (valueAxis.Labels != null)
            {
                foreach (var label in valueAxis.Labels)
                {
                    if (label.Content != null && label.Content.Length > maxChars)
                        maxChars = label.Content.Length;
                }

                if (valueAxis.Labels.Count > 0)
                {
                    fontSize = valueAxis.Labels[0].FontSize > 0 ? valueAxis.Labels[0].FontSize : 12f;
                    labelMargin = valueAxis.Labels[0].Margin;
                }
            }

            float estimatedMaxWidth = Math.Max(20f, maxChars * (fontSize * 0.65f)) + labelMargin;

            float titleHeight = string.IsNullOrWhiteSpace(valueAxis.Title?.Content)
                ? 0f
                : (valueAxis.Title.FontSize > 0 ? valueAxis.Title.FontSize * 1.3f : 16f) + valueAxis.Title.Margin;

            float calculated = estimatedMaxWidth + 10f + titleHeight;
            return Math.Max(currentLeft, (float)Math.Ceiling(calculated));
        }
    }
}
