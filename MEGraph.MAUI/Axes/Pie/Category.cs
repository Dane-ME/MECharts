using MEGraph.MAUI.Styles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Axes.Pie
{
    public class Category : Base
    {
        public Category() : base()
        {
            Title = new AxisTitle("Category");
            Orientation = AxisOrientation.Angular;
            Type = AxisType.Category;
        }
        protected override void DrawAxisLine(ICanvas canvas, RectF plotArea)
        {
            if (Orientation != AxisOrientation.Angular) return;

            var x = (plotArea.Right - plotArea.Left) / 2 + plotArea.Left;
            var y = (plotArea.Bottom - plotArea.Top) / 2 + plotArea.Top;
            var radius = Math.Min(plotArea.Width / 2, plotArea.Height / 2);


            canvas.StrokeColor = StrokeColor;
            canvas.StrokeSize = StrokeSize;
            canvas.DrawCircle(x, y, radius);
        }
        protected override void DrawLabels(ICanvas canvas, RectF plotArea)
        {
            
        }

        protected override void DrawTitle(ICanvas canvas, RectF outerArea, RectF plotArea)
        {
            if (string.IsNullOrWhiteSpace(Title?.Content) || !Title.IsVisible) return;

            var titleSize = canvas.GetStringSize(Title.Content, Title.Font, Title.FontSize);
            var titleArea = new RectF(
                plotArea.Left,
                plotArea.Bottom + Labels.FirstOrDefault()?.Margin ?? 0 +
                (Labels?.Any() == true ? Labels.Max(l => canvas.GetStringSize(l.Content, l.Font, l.FontSize).Height + l.Margin) : 0),
                plotArea.Width,
                titleSize.Height + Title.Margin
            );

            canvas.FontSize = Title.FontSize;
            canvas.FontColor = Title.FontColor;
            canvas.Font = Title.Font;
            canvas.DrawString(
                Title.Content,
                titleArea,
                Title.HorizontalAlignment,
                Title.VerticalAlignment
            );
        }

        protected override (float Left, float Top, float Right, float Bottom) CalculateMargins(ICanvas canvas, RectF outerArea, ObservableCollection<IAxis> axes)
        {
            float top = 0, left = 0, right = 0, bottom = 0;

            var categoryAxis = axes
                .OfType<Category>()
                .FirstOrDefault(a => a.Orientation == AxisOrientation.Angular);

            if (categoryAxis?.Labels.FirstOrDefault() is not AxisLabel element)
                return (0, 0, 0, 0);

            var size = canvas.GetStringSize(element.Content, element.Font, element.FontSize);

            var titleSize = string.IsNullOrWhiteSpace(Title?.Content)
                ? new SizeF(0, 0)
                : canvas.GetStringSize(Title.Content, Title.Font, Title.FontSize);

            const float padding = 5;
            bottom = Math.Max(
                bottom,
                size.Height + element.Margin + padding + titleSize.Height + (Title?.Margin ?? 0));

            return (top, left, right, bottom);
        }
    }
}
