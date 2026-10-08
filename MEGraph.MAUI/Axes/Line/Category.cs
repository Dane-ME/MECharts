using MEGraph.MAUI.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Axes.Line
{
    public class Category : Base
    {
        // === CONSTRUCTOR ===
        public Category() : base()
        {
            Title = new AxisTitle("Category");
            Orientation = AxisOrientation.X;
            Type = AxisType.Category;
        }
        // === OVERRIDE ABSTRACT METHODS ===
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

            float fontSize = Labels[0].FontSize > 0 ? Labels[0].FontSize : 12f;
            float maxLabelHeight = fontSize * 1.3f;
            float labelMargin = Labels[0].Margin;
            float labelTop = (float)Math.Round(plotArea.Bottom + labelMargin);
            float labelCenterY = (float)Math.Round(labelTop + maxLabelHeight / 2f);

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
            if (AutoSkip)
            {
                // Ước tính chiều rộng nhãn dài nhất: maxChars * (fontSize * 0.65f)
                int maxChars = 0;
                for (int i = 0; i < Labels.Count; i++)
                {
                    if (Labels[i].Content != null && Labels[i].Content.Length > maxChars)
                        maxChars = Labels[i].Content.Length;
                }
                float estimatedMaxLabelWidth = Math.Max(16f, maxChars * (fontSize * 0.65f));
                float minSpacing = estimatedMaxLabelWidth + 8f;
                if (stepX < minSpacing)
                {
                    stride = (int)Math.Ceiling(minSpacing / stepX);
                    if (stride < 1) stride = 1;
                }
            }

            _currentStride = stride;

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

            float labelFontSize = (Labels?.Count > 0 && Labels[0].FontSize > 0) ? Labels[0].FontSize : 12f;
            float labelBottomOffset = (Labels?.Any() == true)
                ? (labelFontSize * 1.3f + (Labels[0].Margin * 2f))
                : 0f;

            float titleFontSize = Title.FontSize > 0 ? Title.FontSize : 14f;
            float titleHeight = titleFontSize * 1.3f;

            var titleArea = new RectF(
                plotArea.Left,
                (float)Math.Round(plotArea.Bottom + labelBottomOffset),
                plotArea.Width,
                titleHeight + Title.Margin
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

        // START - 2.6.1 - ADD - Synchronize gridlines with label auto-skip stride
        private int _currentStride = 1;
        // END - 2.6.1 - ADD

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

        // START - 2.6.1 - ADD - Override DrawGridLines to only draw gridlines matching visible ticks
        protected override void DrawGridLines(ICanvas canvas, RectF plotArea)
        {
            if (!ShowGridLines || Labels?.Any() != true) return;

            int totalCount = TotalPoints > 0 ? TotalPoints : Labels.Count;
            if (totalCount < 2) return;

            canvas.StrokeColor = GridColor;
            canvas.StrokeSize = GridLineWidth;

            for (int i = 0; i < Labels.Count; i++)
            {
                if (AutoSkip && _currentStride > 1 && (i % _currentStride != 0) && (i != Labels.Count - 1))
                {
                    continue;
                }

                var lbl = Labels[i];
                if (!lbl.IsVisible) continue;

                float position = CalculateLabelPosition(lbl, plotArea);
                DrawGridLine(canvas, position, plotArea);
            }
        }
        // END - 2.6.1 - ADD

        protected override void DrawGridLine(ICanvas canvas, float position, RectF plotArea)
        {
            canvas.DrawLine(position, plotArea.Top, position, plotArea.Bottom);
        }

        // === CATEGORY SPECIFIC METHODS ===
        public void SetCategories(params string[] categories)
        {
            Labels.Clear();
            foreach (var category in categories)
            {
                Labels.Add(new AxisLabel(category));
            }
            OnAxisChanged(nameof(Labels), null, Labels);
        }

        public void AddCategory(string category)
        {
            Labels.Add(new AxisLabel(category));
            OnAxisChanged(nameof(Labels), null, Labels);
        }

        public void RemoveCategory(string category)
        {
            var item = Labels.FirstOrDefault(l => l.Content == category);
            if (item != null)
            {
                Labels.Remove(item);
                OnAxisChanged(nameof(Labels), null, Labels);
            }
        }
    }
}
