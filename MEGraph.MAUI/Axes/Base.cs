using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEGraph.MAUI.Cores;
using MEGraph.MAUI.Styles;


namespace MEGraph.MAUI.Axes
{
    public abstract class Base : IAxis
    {
        protected BaseChart? _baseChart;
        // === THUỘC TÍNH CƠ BẢN ===
        public AxisTitle Title { get; set; } = new AxisTitle("");
        public List<AxisLabel> Labels { get; set; } = new();
        public AxisOrientation Orientation { get; set; } = AxisOrientation.X;
        public AxisType Type { get; set; } = AxisType.Category;
        public Color StrokeColor { get; set; } = Colors.Black;

        // === THUỘC TÍNH HIỂN THỊ ===
        public bool IsVisible { get; set; } = true;
        public float StrokeSize { get; set; } = 1f;
        public bool ShowGridLines { get; set; } = true;
        public Color GridColor { get; set; } = Colors.LightGray;
        public float GridLineWidth { get; set; } = 0.5f;

        // === THUỘC TÍNH GIÁ TRỊ ===
        public float MinValue { get; set; } = 0f;
        public float MaxValue { get; set; } = 100f;
        public float TickInterval { get; set; } = 10f;
        public int TickCount { get; set; } = 5;
        public bool IsAutoRange { get; set; } = true;
        public bool AutoSkip { get; set; } = true;
        public int TotalPoints { get; set; } = 0;

        // === THUỘC TÍNH VỊ TRÍ ===
        public float Position { get; set; } = 0f;
        public AxisPosition AxisPosition { get; set; } = AxisPosition.Left;
        public bool IsReversed { get; set; } = false;
        // START - 2.1.4 - ADD - Fix the issue where axes were lost when rendering multiple charts.
        public string Id { get; set; }
        public string ChartId { get; set; }
        // END - 2.1.4 - ADD - Fix the issue where axes were lost when rendering multiple charts.

        // === EVENTS ===
        public event EventHandler<AxisChangedEventArgs>? AxisChanged;

        // === CONSTRUCTOR ===
        protected Base()
        {
            
        }

        // === METHODS CHUNG ===
        public virtual void Draw(ICanvas canvas, RectF outerArea, RectF plotArea, BaseChart baseChart)
        {
            if (baseChart == null) { return; }
            _baseChart = baseChart;
            Draw(canvas, outerArea, plotArea);
        }
        public virtual void Draw(ICanvas canvas, RectF outerArea, RectF plotArea)
        {
            if (!IsVisible) return;

            DrawAxisLine(canvas, plotArea);
            DrawLabels(canvas, plotArea);
            DrawTitle(canvas, outerArea, plotArea);

            if (ShowGridLines)
            {
                // START - 2.6.1 - ADD - Clip plotArea to prevent gridlines from leaking outside bounds
                canvas.SaveState();
                canvas.ClipRectangle(plotArea);
                DrawGridLines(canvas, plotArea);
                canvas.RestoreState();
                // END - 2.6.1 - ADD
            }
        }

        public virtual void CalculateTicks()
        {
            if (TickCount <= 0) return;

            Labels.Clear();
            float step = (MaxValue - MinValue) / (TickCount - 1);

            for (int i = 0; i < TickCount; i++)
            {
                float value = MinValue + (i * step);
                Labels.Add(new AxisLabel(value.ToString("F1")));
            }
        }

        public virtual void UpdateLabels()
        {
            CalculateTicks();
        }

        public virtual void ApplyAutoRange(float dataMin, float dataMax, int targetTickCount = 5)
        {
            if (!IsAutoRange) return;

            // Xử lý edge case khi data min == max hoặc không hợp lệ
            if (float.IsNaN(dataMin) || float.IsInfinity(dataMin) ||
                float.IsNaN(dataMax) || float.IsInfinity(dataMax))
            {
                dataMin = 0f;
                dataMax = 100f;
            }

            if (Math.Abs(dataMax - dataMin) < 1e-4f)
            {
                if (Math.Abs(dataMin) < 1e-4f)
                {
                    dataMin = 0f;
                    dataMax = 10f;
                }
                else
                {
                    float pad = Math.Abs(dataMin) * 0.2f;
                    dataMin -= pad;
                    dataMax += pad;
                }
            }

            // Neo đáy về 0 nếu dữ liệu toàn số dương và điểm min không quá xa 0
            if (dataMin >= 0f && dataMin < dataMax * 0.5f)
            {
                dataMin = 0f;
            }

            float rawRange = dataMax - dataMin;
            targetTickCount = Math.Max(2, targetTickCount);
            float rawSpacing = rawRange / (targetTickCount - 1);

            double exponent = Math.Floor(Math.Log10(rawSpacing));
            double fraction = rawSpacing / Math.Pow(10, exponent);

            double niceFraction;
            if (fraction <= 1.2) niceFraction = 1.0;
            else if (fraction <= 2.5) niceFraction = 2.0;
            else if (fraction <= 7.0) niceFraction = 5.0;
            else niceFraction = 10.0;

            float niceSpacing = (float)(niceFraction * Math.Pow(10, exponent));
            if (niceSpacing <= 0) niceSpacing = 1f;

            float niceMin = (float)(Math.Floor(dataMin / niceSpacing) * niceSpacing);
            float niceMax = (float)(Math.Ceiling(dataMax / niceSpacing) * niceSpacing);

            if (niceMax <= niceMin)
            {
                niceMax = niceMin + niceSpacing;
            }

            MinValue = niceMin;
            MaxValue = niceMax;
            TickInterval = niceSpacing;

            Labels.Clear();
            string format = (niceSpacing >= 1f && Math.Abs(niceSpacing - Math.Round(niceSpacing)) < 0.001) ? "F0" : "F1";

            for (float val = niceMin; val <= niceMax + (niceSpacing * 0.01f); val += niceSpacing)
            {
                Labels.Add(new AxisLabel(val.ToString(format)));
            }

            TickCount = Labels.Count;
            OnAxisChanged(nameof(Labels), null, Labels);
        }

        public virtual void SetRange(float min, float max)
        {
            MinValue = min;
            MaxValue = max;
            OnAxisChanged(nameof(MinValue), MinValue, min);
            OnAxisChanged(nameof(MaxValue), MaxValue, max);
        }

        public virtual void SetTickInterval(float interval)
        {
            TickInterval = interval;
            OnAxisChanged(nameof(TickInterval), TickInterval, interval);
        }

        public virtual void SetTickCount(int count)
        {
            TickCount = count;
            OnAxisChanged(nameof(TickCount), TickCount, count);
        }

        public (float Left, float Top, float Right, float Bottom) MeasureMargins(ICanvas canvas, RectF outerArea, ObservableCollection<IAxis> axes)
        {
            return CalculateMargins(canvas, outerArea, axes);
        }
        // === ABSTRACT METHODS ===
        protected abstract void DrawAxisLine(ICanvas canvas, RectF plotArea);
        protected abstract void DrawLabels(ICanvas canvas, RectF plotArea);
        protected abstract void DrawTitle(ICanvas canvas, RectF outerArea, RectF plotArea);
        //START - 2.1.3 - ADD - hit-test, cache and state hover  
        // OVERLAY HOOK
        public virtual void DrawOverlay(ICanvas canvas,RectF outerArea, RectF plotArea)
        {
            // Implementation cơ bản - override trong derived classes
        }
        //END - 2.1.3 - ADD - hit-test, cache and state hover
        // === VIRTUAL METHODS ===
        protected virtual void DrawGridLines(ICanvas canvas, RectF plotArea)
        {
            if (!ShowGridLines) return;

            canvas.StrokeColor = GridColor;
            canvas.StrokeSize = GridLineWidth;

            foreach (var label in Labels)
            {
                float position = CalculateLabelPosition(label, plotArea);
                DrawGridLine(canvas, position, plotArea);
            }
        }

        protected virtual float CalculateLabelPosition(AxisLabel label, RectF plotArea)
        {
            // Implementation cơ bản - override trong derived classes
            return 0f;
        }

        protected virtual (float Left, float Top, float Right, float Bottom) CalculateMargins(ICanvas canvas, RectF outerArea, ObservableCollection<IAxis> axes)
        {
            // Implementation cơ bản - override trong derived classes
            return (0, 0, 0, 0);
        }

        protected virtual void DrawGridLine(ICanvas canvas, float position, RectF plotArea)
        {
            // Implementation cơ bản - override trong derived classes
        }

        // === EVENT HANDLING ===
        protected virtual void OnAxisChanged(string propertyName, object oldValue, object newValue)
        {
            AxisChanged?.Invoke(this, new AxisChangedEventArgs(propertyName, oldValue, newValue));
        }
    }
}
