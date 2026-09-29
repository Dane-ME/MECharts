using MEGraph.MAUI.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Axes.Area
{
    public class Category : Base
    {
        public Category() : base()
        {
            Title = new AxisTitle("Category");
            Orientation = AxisOrientation.X;
            Type = AxisType.Category;
        }
        protected override void DrawAxisLine(ICanvas canvas, RectF plotArea)
        {
            if (Orientation != AxisOrientation.X) return;

            canvas.StrokeColor = StrokeColor;
            canvas.StrokeSize = StrokeSize;
            canvas.DrawLine(plotArea.Left, plotArea.Bottom, plotArea.Right, plotArea.Bottom);
        }

        protected override void DrawLabels(ICanvas canvas, RectF plotArea)
        {
            if (Labels?.Any() != true) return;
            float maxLabelHeight = 0;
            float maxLabelWidth = 0;
            foreach (var lbl in Labels)
            {
                var size = canvas.GetStringSize(lbl.Content, lbl.Font, lbl.FontSize);
                maxLabelHeight = Math.Max(maxLabelHeight, size.Height + lbl.Margin);
                maxLabelWidth = Math.Max(maxLabelWidth, size.Width + lbl.Margin);
            }
            float labelMargin = Labels.FirstOrDefault()?.Margin ?? 4f;
            float labelTop = (float)Math.Round(plotArea.Bottom + labelMargin);
            float labelCenterY = (float)Math.Round(labelTop + maxLabelHeight / 2f);
            var labelArea = new RectF(
                plotArea.Left,
                labelTop,
                plotArea.Width,
                maxLabelHeight
                );

            int totalCount = TotalPoints > 0 ? TotalPoints : Labels.Count;
            if (totalCount < 2)
            {
                if (Labels.Count > 0 && Labels[0].IsVisible)
                {
                    canvas.FontSize = Labels[0].FontSize;
                    canvas.FontColor = Labels[0].FontColor;
                    canvas.Font = Labels[0].Font;
                    canvas.DrawString(Labels[0].Content, plotArea.Center.X, labelCenterY, HorizontalAlignment.Center);
                }
                return;
            }

            float stepX = plotArea.Width / (totalCount - 1);

            int stride = 1;
            if (AutoSkip && maxLabelWidth > 0)
            {
                float minSpacing = maxLabelWidth + 8f;
                if (stepX < minSpacing)
                {
                    stride = (int)Math.Ceiling(minSpacing / stepX);
                    if (stride < 1) stride = 1;
                }
            }

            for (int i = 0; i < Labels.Count; i++)
            {
                if (AutoSkip && stride > 1 && (i % stride != 0) && (i != Labels.Count - 1))
                {
                    continue;
                }

                float x = (float)Math.Round(plotArea.Left + i * stepX);
                var lbl = Labels[i];

                if (!lbl.IsVisible) continue;

                canvas.FontSize = lbl.FontSize;
                canvas.FontColor = lbl.FontColor;
                canvas.Font = lbl.Font;

                if (Math.Abs(lbl.Rotation) > 0.01f)
                {
                    canvas.SaveState();
                    canvas.Translate(x, labelCenterY);
                    canvas.Rotate(lbl.Rotation);
                    canvas.DrawString(lbl.Content, 0, 0, lbl.HorizontalAlignment);
                    canvas.RestoreState();
                }
                else
                {
                    canvas.DrawString(lbl.Content, x, labelCenterY, lbl.HorizontalAlignment);
                }
            }
        }

        protected override void DrawTitle(ICanvas canvas, RectF outerArea, RectF plotArea)
        {
            if (string.IsNullOrWhiteSpace(Title?.Content) || !Title.IsVisible) return;

            var titleSize = canvas.GetStringSize(Title.Content, Title.Font, Title.FontSize);
            float labelBottomOffset = (Labels?.Any() == true)
                ? (Labels.Max(l => canvas.GetStringSize(l.Content, l.Font, l.FontSize).Height + l.Margin) + (Labels.FirstOrDefault()?.Margin ?? 0))
                : 0f;

            var titleArea = new RectF(
                plotArea.Left,
                (float)Math.Round(plotArea.Bottom + labelBottomOffset),
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

        protected override float CalculateLabelPosition(AxisLabel label, RectF plotArea)
        {
            if (Labels?.Any() != true) return 0f;
            int index = Labels.IndexOf(label);
            if (index < 0) return 0f;
            int totalCount = TotalPoints > 0 ? TotalPoints : Labels.Count;
            if (totalCount < 2) return plotArea.Center.X;
            float stepX = plotArea.Width / (totalCount - 1);
            return plotArea.Left + index * stepX;
        }

        protected override void DrawGridLine(ICanvas canvas, float position, RectF plotArea)
        {
            canvas.DrawLine(position, plotArea.Top, position, plotArea.Bottom);
        }
    }
}
