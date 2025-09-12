using MEGraph.MAUI.Styles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Font = Microsoft.Maui.Graphics.Font;

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
        //START - 2.1.3 - ADD - hit-test, cache and state hover  
        public override void DrawOverlay(ICanvas canvas,RectF outerArea, RectF plotArea)
        {
            if(this._baseChart == null) return;
            var chart = this._baseChart;

            var pie = chart.Series
                .OfType<Series.Pie.PieSeries>()
                .SingleOrDefault();
            if (pie == null || pie.SelectIndex < 0 || pie.SelectIndex >= pie.Data.Count) return;

            var (cx, cy, r) = pie.GetGeometry();
            var mid = pie.GetSliceMidAngle(pie.SelectIndex);
            var angle = (mid - 0f + (-90f));
            float rad = angle * (float)(Math.PI / 180f);    

            float labelRadius = r * 0.65f;
            float lx = cx + labelRadius * (float)Math.Cos(rad);
            float ly = cy + labelRadius * (float)Math.Sin(rad);

            string text = pie.Data[pie.SelectIndex].ToString("0.##");

            var label = Labels?.FirstOrDefault();
            var font = label?.Font ?? Font.Default;
            var fontSize = label?.FontSize ?? 12f;
            var fontColor = label?.FontColor ?? Colors.Black;

            var size = canvas.GetStringSize(text, font, fontSize);
            float padding = 4f;
            var rect = new RectF(
                lx - size.Width / 2 - padding, 
                ly - size.Height / 2 - padding, 
                size.Width + 2 * padding, 
                size.Height + 2 * padding
                );

            canvas.Font = font;
            canvas.FontSize = fontSize;
            canvas.FontColor = fontColor;

            canvas.FillColor = Color.FromRgba(255, 255, 255, 200);
            canvas.FillRoundedRectangle(rect, 6f);

            canvas.StrokeColor = StrokeColor;
            canvas.StrokeSize = 1f;
            canvas.DrawRoundedRectangle(rect, 6f);

            canvas.DrawString(text, rect, HorizontalAlignment.Center, VerticalAlignment.Center);
        }
        //END - 2.1.3 - ADD - hit-test, cache and state hover
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
