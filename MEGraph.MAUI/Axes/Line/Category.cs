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
            float maxLabelHeight = 0;
            foreach (var lbl in Labels)
            {
                var size = canvas.GetStringSize(lbl.Content, lbl.Font, lbl.FontSize);
                maxLabelHeight = Math.Max(maxLabelHeight, size.Height + lbl.Margin);
            }
            var labelArea = new RectF(
                plotArea.Left, 
                plotArea.Bottom + Labels.First().Margin, 
                plotArea.Width, 
                maxLabelHeight
                );
            float stepX = plotArea.Width / (Labels.Count - 1);
            for (int i = 0; i < Labels.Count; i++)
            {
                float x = plotArea.Left + i * stepX;
                var lbl = Labels[i];

                if (!lbl.IsVisible) continue;

                canvas.FontSize = lbl.FontSize;
                canvas.FontColor = lbl.FontColor;
                canvas.Font = lbl.Font;

                if (Math.Abs(lbl.Rotation) > 0.01f)
                {
                    canvas.SaveState();
                    canvas.Translate(x, labelArea.Center.Y);
                    canvas.Rotate(lbl.Rotation);
                    canvas.DrawString(lbl.Content, 0, 0, lbl.HorizontalAlignment);
                    canvas.RestoreState();
                }
                else
                {
                    canvas.DrawString(lbl.Content, x, labelArea.Center.Y, lbl.HorizontalAlignment);
                }
            }
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
        protected override float CalculateLabelPosition(AxisLabel label, RectF plotArea)
        {
            if (Labels?.Any() != true) return 0f;

            int index = Labels.IndexOf(label);
            if (index < 0) return 0f;

            float stepX = plotArea.Width / (Labels.Count - 1);
            return plotArea.Left + index * stepX;
        }
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
