using MEGraph.MAUI.Cores;
using MEGraph.MAUI.Styles;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AxisTextStyle = MEGraph.MAUI.Styles.Text;

namespace MEGraph.MAUI.Axes
{
    public enum AxisOrientation
    {
        X,
        Y,
        Radial,     // Cho Radar chart
        Angular,    // Cho Pie chart
        Time        // Cho Stock chart
    }

    public enum AxisType
    {
        Category,    // Trục phân loại
        Value,       // Trục giá trị
        Radial,      // Trục hướng tâm
        Angular,     // Trục góc
        Time,        // Trục thời gian
        Logarithmic  // Trục logarit
    }

    public interface IAxis
    {
        // === THUỘC TÍNH CƠ BẢN ===
        AxisTitle Title { get; set; }
        List<AxisLabel> Labels { get; set; }
        AxisOrientation Orientation { get; set; }
        AxisType Type { get; set; }
        Color StrokeColor { get; set; }

        // === THUỘC TÍNH HIỂN THỊ ===
        bool IsVisible { get; set; }
        float StrokeSize { get; set; }
        bool ShowGridLines { get; set; }
        Color GridColor { get; set; }
        float GridLineWidth { get; set; }

        // === THUỘC TÍNH GIÁ TRỊ ===
        float MinValue { get; set; }
        float MaxValue { get; set; }
        float TickInterval { get; set; }
        int TickCount { get; set; }

        // === THUỘC TÍNH VỊ TRÍ ===
        float Position { get; set; }  // Vị trí trên trục (0-1)
        bool IsReversed { get; set; } // Đảo ngược hướng trục

        // === METHODS ===
        void Draw(ICanvas canvas, RectF outerArea, RectF plotArea);
        void Draw(ICanvas canvas, RectF outerArea, RectF plotArea, BaseChart chart);
        void DrawOverlay(ICanvas canvas, RectF dirtyRect, RectF plotArea);
        void CalculateTicks();
        void UpdateLabels();
        void SetRange(float min, float max);
        void SetTickInterval(float interval);
        void SetTickCount(int count);

        // === EVENTS ===
        event EventHandler<AxisChangedEventArgs> AxisChanged;
    }

    // === EVENT ARGS ===
    public class AxisChangedEventArgs : EventArgs
    {
        public string PropertyName { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }

        public AxisChangedEventArgs(string propertyName, object oldValue, object newValue)
        {
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}