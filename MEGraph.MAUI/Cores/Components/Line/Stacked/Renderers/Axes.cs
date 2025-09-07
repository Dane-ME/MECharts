using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Axes.Line;
using MEGraph.MAUI.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;

namespace MEGraph.MAUI.Cores.Components.Line.Stacked.Renderers
{
    public class Axes
    {
        public void Draw(ICanvas canvas, RectF outerArea, RectF plotArea, BaseChart baseChart)
        {
            if (baseChart?.Axes == null) return;

            foreach (var axis in baseChart.Axes)
            {
                axis.Draw(canvas, outerArea, plotArea);
            }
        }

        public (float Left, float Top, float Right, float Bottom) CalculateMargins(ICanvas canvas, RectF outerArea, ObservableCollection<IAxis> axes)
        {
            float top = 0, left = 0, right = 20, bottom = 0;

            foreach (var axis in axes)
            {
                if (axis is Category categoryAxis && categoryAxis.Orientation == AxisOrientation.X)
                {
                    bottom = CalculateCategoryAxisMargin(canvas, categoryAxis, bottom);
                }
                else if (axis is Value valueAxis && valueAxis.Orientation == AxisOrientation.Y)
                {
                    left = CalculateValueAxisMargin(canvas, valueAxis, left);
                }
            }

            return (left, top, right, bottom);
        }

        private float CalculateCategoryAxisMargin(ICanvas canvas, Category categoryAxis, float currentBottom)
        {
            float maxLabelHeight = 0;
            float maxLabelMargin = 0;

            if (categoryAxis.Labels?.Any() == true)
            {
                foreach (var label in categoryAxis.Labels)
                {
                    var size = canvas.GetStringSize(label.Content, label.Font, label.FontSize);
                    maxLabelHeight = Math.Max(maxLabelHeight, size.Height + label.Margin);
                    maxLabelMargin = Math.Max(maxLabelMargin, label.Margin);
                }
            }

            var titleSize = string.IsNullOrWhiteSpace(categoryAxis.Title?.Content)
                ? new SizeF(0, 0)
                : canvas.GetStringSize(categoryAxis.Title.Content, categoryAxis.Title.Font, categoryAxis.Title.FontSize);

            float padding = 5;
            return Math.Max(currentBottom, maxLabelHeight + maxLabelMargin + padding + titleSize.Height + categoryAxis.Title.Margin);
        }

        private float CalculateValueAxisMargin(ICanvas canvas, Value valueAxis, float currentLeft)
        {
            if (valueAxis.Labels?.Any() != true && string.IsNullOrWhiteSpace(valueAxis.Title.Content)) return currentLeft;

            // Tính toán width của labels
            float maxWidth = 0;
            foreach (var label in valueAxis.Labels)
            {
                var size = canvas.GetStringSize(label.Content, label.Font, label.FontSize);
                maxWidth = Math.Max(maxWidth, size.Width + label.Margin);
            }

            // Tính toán height của title (vì Y-axis title được rotate)
            var titleSize = string.IsNullOrWhiteSpace(valueAxis.Title?.Content)
                ? new SizeF(0, 0)
                : canvas.GetStringSize(valueAxis.Title.Content, valueAxis.Title.Font, valueAxis.Title.FontSize);

            return Math.Max(currentLeft, maxWidth + 10 + titleSize.Height);
        }
    }
}