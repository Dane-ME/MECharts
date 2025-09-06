using MEGraph.MAUI.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;

namespace MEGraph.MAUI.Axes.Line
{
    public class Value : Base
    {
        // === CONSTRUCTOR ===
        public Value() : base()
        {
            Title = new AxisTitle("Value");
            Orientation = AxisOrientation.Y;
            Type = AxisType.Value;
            MinValue = 0f;
            MaxValue = 100f;
            TickCount = 5;
        }

        // === OVERRIDE ABSTRACT METHODS ===
        protected override void DrawAxisLine(ICanvas canvas, RectF plotArea)
        {
            canvas.StrokeColor = StrokeColor;
            canvas.StrokeSize = StrokeSize;

            if (Orientation == AxisOrientation.Y)
            {
                canvas.DrawLine(plotArea.Left, plotArea.Top, plotArea.Left, plotArea.Bottom);
            }
            else if (Orientation == AxisOrientation.X)
            {
                canvas.DrawLine(plotArea.Left, plotArea.Bottom, plotArea.Right, plotArea.Bottom);
            }
        }

        protected override void DrawLabels(ICanvas canvas, RectF plotArea)
        {
            if (Labels?.Any() != true) return;

            foreach (var label in Labels)
            {
                if (!label.IsVisible) continue;

                float position = CalculateLabelPosition(label, plotArea);

                canvas.FontSize = label.FontSize;
                canvas.FontColor = label.FontColor;
                canvas.Font = label.Font;

                if (Orientation == AxisOrientation.Y)
                {
                    // Draw Y-axis labels
                    if (Math.Abs(label.Rotation) > 0.01f)
                    {
                        canvas.SaveState();
                        canvas.Translate(plotArea.Left - label.Margin, position);
                        canvas.Rotate(label.Rotation);
                        canvas.DrawString(label.Content, 0, 0, label.HorizontalAlignment);
                        canvas.RestoreState();
                    }
                    else
                    {
                        canvas.DrawString(label.Content, plotArea.Left - label.Margin, position, label.HorizontalAlignment);
                    }
                }
                else if (Orientation == AxisOrientation.X)
                {
                    // Draw X-axis labels
                    if (Math.Abs(label.Rotation) > 0.01f)
                    {
                        canvas.SaveState();
                        canvas.Translate(position, plotArea.Bottom + label.Margin);
                        canvas.Rotate(label.Rotation);
                        canvas.DrawString(label.Content, 0, 0, label.HorizontalAlignment);
                        canvas.RestoreState();
                    }
                    else
                    {
                        canvas.DrawString(label.Content, position, plotArea.Bottom + label.Margin, label.HorizontalAlignment);
                    }
                }
            }
        }

        protected override void DrawTitle(ICanvas canvas, RectF outerArea, RectF plotArea)
        {
            if (string.IsNullOrWhiteSpace(Title?.Content) || !Title.IsVisible) return;

            canvas.FontSize = Title.FontSize;
            canvas.FontColor = Title.FontColor;
            canvas.Font = Title.Font;

            if (Orientation == AxisOrientation.Y)
            {
                // Y-axis title (rotated)
                var titleArea = new RectF(
                    outerArea.Left,
                    plotArea.Top,
                    plotArea.Left - outerArea.Left,
                    plotArea.Height
                );

                canvas.SaveState();
                canvas.Translate(titleArea.Center.X, titleArea.Center.Y);
                canvas.Rotate(-90);
                canvas.DrawString(Title.Content, 0, 0, Title.HorizontalAlignment);
                canvas.RestoreState();
            }
            else if (Orientation == AxisOrientation.X)
            {
                // X-axis title
                var titleArea = new RectF(
                    plotArea.Left,
                    plotArea.Bottom,
                    plotArea.Width,
                    outerArea.Bottom - plotArea.Bottom
                );

                canvas.DrawString(
                    Title.Content,
                    titleArea,
                    Title.HorizontalAlignment,
                    Title.VerticalAlignment
                );
            }
        }

        // === OVERRIDE VIRTUAL METHODS ===
        protected override float CalculateLabelPosition(AxisLabel label, RectF plotArea)
        {
            if (Labels?.Any() != true) return 0f;

            int index = Labels.IndexOf(label);
            if (index < 0) return 0f;

            if (Orientation == AxisOrientation.Y)
            {
                float stepY = plotArea.Height / (Labels.Count - 1);
                return plotArea.Bottom - (index * stepY);
            }
            else if (Orientation == AxisOrientation.X)
            {
                float stepX = plotArea.Width / (Labels.Count - 1);
                return plotArea.Left + (index * stepX);
            }

            return 0f;
        }

        protected override void DrawGridLine(ICanvas canvas, float position, RectF plotArea)
        {
            if (Orientation == AxisOrientation.Y)
            {
                canvas.DrawLine(position, plotArea.Top, position, plotArea.Bottom);
            }
            else if (Orientation == AxisOrientation.X)
            {
                canvas.DrawLine(plotArea.Left, position, plotArea.Right, position);
            }
        }

        // === VALUE SPECIFIC METHODS ===
        public void SetValueRange(float min, float max, int tickCount = 5)
        {
            SetRange(min, max);
            SetTickCount(tickCount);
            CalculateTicks();
        }

        public void SetValueRange(float min, float max, float tickInterval)
        {
            SetRange(min, max);
            SetTickInterval(tickInterval);
            CalculateTicks();
        }

        public float GetValueAtPosition(float position, RectF plotArea)
        {
            if (Orientation == AxisOrientation.Y)
            {
                float ratio = (plotArea.Bottom - position) / plotArea.Height;
                return MinValue + ratio * (MaxValue - MinValue);
            }
            else if (Orientation == AxisOrientation.X)
            {
                float ratio = (position - plotArea.Left) / plotArea.Width;
                return MinValue + ratio * (MaxValue - MinValue);
            }
            return 0f;
        }

        public float GetPositionForValue(float value, RectF plotArea)
        {
            if (Orientation == AxisOrientation.Y)
            {
                float ratio = (value - MinValue) / (MaxValue - MinValue);
                return plotArea.Bottom - (ratio * plotArea.Height);
            }
            else if (Orientation == AxisOrientation.X)
            {
                float ratio = (value - MinValue) / (MaxValue - MinValue);
                return plotArea.Left + (ratio * plotArea.Width);
            }
            return 0f;
        }
    }
}