using MEGraph.MAUI.Theme;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MEGraph.MAUI.Series.Column
{
    public class ColumnSeries : ISeries
    {
        public string Name { get; set; } = "ColumnSeries";
        public bool IsVisible { get; set; } = true;
        public string? AxisId { get; set; }
        public List<float> Data { get; set; } = new();

        /// <summary>
        /// Màu chính của cột (dùng khi không bật gradient, hoặc dùng làm start color của gradient)
        /// </summary>
        public Color FillColor { get; set; } = ChartColors.DefaultPrimaryColor;

        /// <summary>
        /// Màu viền cột (tùy chọn)
        /// </summary>
        public Color? StrokeColor { get; set; } = null;
        public float StrokeWidth { get; set; } = 0f;

        /// <summary>
        /// Bán kính bo góc trên đầu cột (Top-left & Top-right) mang lại giao diện hiện đại
        /// </summary>
        public float CornerRadius { get; set; } = 6f;

        /// <summary>
        /// Tùy chọn đổ bóng mờ nhẹ hoặc gradient hiện đại
        /// </summary>
        public bool UseGradient { get; set; } = true;
        public Color? GradientEndColor { get; set; } = null;

        /// <summary>
        /// ISeries.Color — dùng cho Legend swatch
        /// </summary>
        public Color Color => FillColor;

        public float GetMinY() => Data?.Any() == true ? Math.Min(0f, Data.Min()) : 0f;
        public float GetMaxY() => Data?.Any() == true ? Data.Max() : 0f;

        public void Draw(ICanvas canvas, RectF plotArea)
        {
            Draw(canvas, plotArea, null, null, 1f, 0, 1);
        }

        public void Draw(ICanvas canvas, RectF plotArea, float? globalMinY, float? globalMaxY)
        {
            Draw(canvas, plotArea, globalMinY, globalMaxY, 1f, 0, 1);
        }

        /// <summary>
        /// Vẽ các cột cho series này trong nhóm cột clustered (group)
        /// </summary>
        /// <param name="canvas">Graphics Canvas</param>
        /// <param name="plotArea">Vùng vẽ biểu đồ</param>
        /// <param name="globalMinY">Giá trị min trục Y</param>
        /// <param name="globalMaxY">Giá trị max trục Y</param>
        /// <param name="progress">Hệ số Animation (0.0 -> 1.0)</param>
        /// <param name="seriesIndex">Vị trí series trong nhóm clustered (0, 1, 2...)</param>
        /// <param name="totalSeriesCount">Tổng số series cột trong cùng cụm</param>
        /// <param name="totalPoints">Số category X</param>
        public void Draw(
            ICanvas canvas,
            RectF plotArea,
            float? globalMinY,
            float? globalMaxY,
            float progress,
            int seriesIndex,
            int totalSeriesCount,
            int? totalPoints = null)
        {
            if (Data == null || Data.Count == 0 || !IsVisible) return;

            int countCategories = (totalPoints.HasValue && totalPoints.Value > 0)
                ? totalPoints.Value
                : Data.Count;

            if (countCategories <= 0) return;

            float minY = globalMinY ?? 0f;
            float maxY = globalMaxY ?? (Data.Any() ? Data.Max() : 100f);
            if (minY > 0) minY = 0f; // Cột dọc luôn bám baseline Y = 0 nếu min > 0
            float rangeY = (maxY - minY == 0) ? 1f : maxY - minY;

            float slotWidth = plotArea.Width / countCategories;
            float groupPadding = slotWidth * 0.2f; // 20% khoảng cách giữa các slot category
            float availableWidthForGroup = slotWidth - groupPadding;

            float barSpacing = 2f; // khoảng cách giữa các cột trong cụm
            float singleBarWidth = Math.Max(2f, (availableWidthForGroup - (totalSeriesCount - 1) * barSpacing) / Math.Max(1, totalSeriesCount));

            // baseline Y = 0
            float zeroY = plotArea.Bottom - ((0f - minY) / rangeY * plotArea.Height);
            zeroY = Math.Clamp(zeroY, plotArea.Top, plotArea.Bottom);

            canvas.Antialias = true;

            LinearGradientPaint? sharedGradient = null;
            if (UseGradient)
            {
                Color endColor = GradientEndColor ?? FillColor.WithAlpha(0.65f);
                sharedGradient = new LinearGradientPaint
                {
                    StartColor = FillColor,
                    EndColor = endColor,
                    StartPoint = new PointF(0, 0),
                    EndPoint = new PointF(0, 1)
                };
            }

            for (int i = 0; i < Data.Count; i++)
            {
                float val = Data[i] * progress;
                float barHeight = Math.Abs(val / rangeY * plotArea.Height);

                float groupStartX = plotArea.Left + (i * slotWidth) + (groupPadding / 2f);
                float barX = groupStartX + seriesIndex * (singleBarWidth + barSpacing);

                float barY;
                if (val >= 0)
                {
                    barY = zeroY - barHeight;
                }
                else
                {
                    barY = zeroY;
                }

                // Giới hạn trong plotArea
                barY = Math.Max(plotArea.Top, barY);
                if (barY + barHeight > plotArea.Bottom)
                {
                    barHeight = Math.Max(0, plotArea.Bottom - barY);
                }

                var barRect = new RectF(barX, barY, singleBarWidth, barHeight);

                // Tô màu / Gradient
                if (sharedGradient != null)
                {
                    canvas.SetFillPaint(sharedGradient, barRect);
                }
                else
                {
                    canvas.FillColor = FillColor;
                }

                // Vẽ bo tròn góc đỉnh cột (rounded top corners)
                float radius = Math.Min(CornerRadius, Math.Min(singleBarWidth / 2f, barHeight / 2f));
                if (radius > 0)
                {
                    var path = CreateRoundedBarPath(barRect, radius, val >= 0);
                    canvas.FillPath(path);

                    if (StrokeColor != null && StrokeWidth > 0)
                    {
                        canvas.StrokeColor = StrokeColor;
                        canvas.StrokeSize = StrokeWidth;
                        canvas.DrawPath(path);
                    }
                }
                else
                {
                    canvas.FillRectangle(barRect);

                    if (StrokeColor != null && StrokeWidth > 0)
                    {
                        canvas.StrokeColor = StrokeColor;
                        canvas.StrokeSize = StrokeWidth;
                        canvas.DrawRectangle(barRect);
                    }
                }
            }
        }

        private PathF CreateRoundedBarPath(RectF rect, float r, bool isPositive)
        {
            var path = new PathF();
            if (isPositive)
            {
                // Bo góc trên: Top-Left và Top-Right
                path.MoveTo(rect.Left, rect.Bottom);
                path.LineTo(rect.Left, rect.Top + r);
                path.QuadTo(rect.Left, rect.Top, rect.Left + r, rect.Top);
                path.LineTo(rect.Right - r, rect.Top);
                path.QuadTo(rect.Right, rect.Top, rect.Right, rect.Top + r);
                path.LineTo(rect.Right, rect.Bottom);
                path.Close();
            }
            else
            {
                // Bo góc dưới khi giá trị âm: Bottom-Left và Bottom-Right
                path.MoveTo(rect.Left, rect.Top);
                path.LineTo(rect.Right, rect.Top);
                path.LineTo(rect.Right, rect.Bottom - r);
                path.QuadTo(rect.Right, rect.Bottom, rect.Right - r, rect.Bottom);
                path.LineTo(rect.Left + r, rect.Bottom);
                path.QuadTo(rect.Left, rect.Bottom, rect.Left, rect.Bottom - r);
                path.Close();
            }
            return path;
        }
    }
}
