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
        private BaseChart? _baseChart;
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

        // === THUỘC TÍNH VỊ TRÍ ===
        public float Position { get; set; } = 0f;
        public bool IsReversed { get; set; } = false;

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
                DrawGridLines(canvas, plotArea);
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
