# Hướng Dẫn Sử Dụng MEGraph.MAUI / MEGraph.MAUI Tutorials

Tài liệu này cung cấp hướng dẫn chi tiết và ví dụ thực tế về cách sử dụng các tính năng, API và cấu hình mới nhất trong thư viện **MEGraph.MAUI** (phiên bản 2.6.1+).  
*This document provides comprehensive guides and practical examples for using the latest features, APIs, and configurations in the **MEGraph.MAUI** library (version 2.6.1+).*

---

## Mục Lục / Table of Contents
1. [Hệ Thống Bảng Màu Mặc Định / Default Color Palette (ChartColors)](#1-hệ-thống-bảng-màu-mặc-định--default-color-palette-chartcolors)
2. [Cấu Hình và Đồng Bộ Trục Tọa Độ / Axes & Gridlines Configuration](#2-cấu-hình-và-đồng-bộ-trục-tọa-độ--axes--gridlines-configuration)
   - [Trục X (Category Axis) và Auto-Skip Ticks / Category Axis & Auto-Skip Ticks](#trục-x-category-axis-và-auto-skip-ticks--category-axis--auto-skip-ticks)
   - [Trục Y (Value Axis) và AutoRange / Value Axis & AutoRange](#trục-y-value-axis-và-autorange--value-axis--autorange)
3. [Đường Cong Mềm Mại / Smooth Spline Curves](#3-đường-cong-mềm-mại--smooth-spline-curves)
4. [Tô Màu Gradient Diện Tích / Gradient Area Fill](#4-tô-màu-gradient-diện-tích--gradient-area-fill)
5. [Ẩn / Hiện Series Động / Toggle Series Visibility](#5-ẩn--hiện-series-động--toggle-series-visibility)
6. [Hiệu Ứng Hoạt Họa Vẽ Vào / Draw-In Entry Animation](#6-hiệu-ứng-hoạt-họa-vẽ-vào--draw-in-entry-animation)
7. [Ví Dụ Tổng Hợp Đầy Đủ / Full Working Example](#7-ví-dụ-tổng-hợp-đầy-đủ--full-working-example)

---

## 1. Hệ Thống Bảng Màu Mặc Định / Default Color Palette (ChartColors)

Từ phiên bản 2.6.0+, MEGraph tích hợp sẵn bảng màu tiêu chuẩn **Tableau 10** qua lớp static `MEGraph.MAUI.Theme.ChartColors`.  
*From version 2.6.0+, MEGraph includes the standard **Tableau 10** color palette via the static class `MEGraph.MAUI.Theme.ChartColors`.*

### Các màu có sẵn / Available Colors:
```csharp
using MEGraph.MAUI.Theme;

Color blue   = ChartColors.Blue;    // #4e79a7 (Default primary color / Màu chính mặc định)
Color orange = ChartColors.Orange;  // #f28e2c
Color red    = ChartColors.Red;     // #e15759
Color teal   = ChartColors.Teal;    // #76b7b2
Color green  = ChartColors.Green;   // #59a14f
Color yellow = ChartColors.Yellow;  // #edc949
Color purple = ChartColors.Purple;  // #af7aa1
Color pink   = ChartColors.Pink;    // #ff9da7
Color brown  = ChartColors.Brown;   // #9c755f
Color gray   = ChartColors.Gray;    // #bab0ab
```

### Các hàm hỗ trợ / Helper Methods:
- **`ChartColors.GetColor(int index)`**:
  - **VI**: Lấy màu tuần hoàn theo chỉ số vòng lặp. Rất tiện khi tạo nhiều Series tự động.
  - **EN**: Retrieves colors cyclically by index. Useful when generating multiple Series dynamically.
  ```csharp
  for (int i = 0; i < mySeriesList.Count; i++)
  {
      mySeriesList[i].StrokeColor = ChartColors.GetColor(i);
  }
  ```
- **`ChartColors.Palette`**:
  - **VI**: Danh sách `IReadOnlyList<Color>` chứa toàn bộ 10 màu.
  - **EN**: An `IReadOnlyList<Color>` containing all 10 palette colors.
- **`ChartColors.GetDistinctColors(ICollection<Color> existing, int count)`**:
  - **VI**: Sinh danh sách các màu không trùng lặp (thường dùng cho Pie/Donut Chart slices).
  - **EN**: Generates non-repeating distinct colors (commonly used for Pie/Donut slices).

---

## 2. Cấu Hình và Đồng Bộ Trục Tọa Độ / Axes & Gridlines Configuration

MEGraph v2.6.1 cải tiến toàn diện cơ chế vẽ lưới:
- **Trục Y (Value)**: Vẽ các đường dóng **ngang** chuẩn xác.
- **Trục X (Category)**: Vẽ các đường dóng **dọc** đồng bộ 100% với nhãn hiển thị.
- **Auto-Clipping**: Toàn bộ đường lưới được bao bọc trong `canvas.ClipRectangle(plotArea)`, đảm bảo không bao giờ bị lem ra ngoài lề hay đè lên tiêu đề.

*MEGraph v2.6.1 comprehensively upgrades gridline rendering:*
- *__Y-Axis (Value)__: Accurately draws __horizontal__ gridlines.*
- *__X-Axis (Category)__: Accurately draws __vertical__ gridlines that stay 100% synchronized with visible labels.*
- *__Auto-Clipping__: Gridlines are bounded inside `canvas.ClipRectangle(plotArea)` to prevent them from leaking into margins or headers.*

---

### Trục X (Category Axis) và Auto-Skip Ticks / Category Axis & Auto-Skip Ticks

```csharp
using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Axes.Line;

var categoryAxis = new Category
{
    Orientation = AxisOrientation.X,
    ShowGridLines = true,                          // VI: Bật/tắt đường dóng dọc | EN: Toggle vertical gridlines
    GridColor = Color.FromArgb("#E5E7EB"),         // VI: Màu đường lưới mờ      | EN: Faded grid color
    GridLineWidth = 1f,                            // VI: Độ dày đường lưới      | EN: Grid line thickness
    AutoSkip = true,                               // VI: Tự động thưa nhãn      | EN: Auto-skip dense labels
    Labels = new List<AxisLabel>
    {
        new AxisLabel("00:00"),
        new AxisLabel("04:00"),
        new AxisLabel("08:00"),
        new AxisLabel("12:00"),
        new AxisLabel("16:00"),
        new AxisLabel("20:00"),
        new AxisLabel("24:00")
    }
};
```

> **Cơ chế tự động đồng bộ (v2.6.1) / Auto-Sync Mechanism (v2.6.1):**  
> - **VI**: Khi `AutoSkip = true`, nếu màn hình nhỏ hoặc có nhiều nhãn, các nhãn sẽ tự động nhảy bước (`stride`). Các đường dọc (Gridlines) sẽ tự động ẩn các vạch tương ứng, không còn tình trạng hàng chục đường dọc ken đặc gây mờ màn hình.  
> - **EN**: When `AutoSkip = true`, labels automatically step (`stride`) if space is tight. Vertical gridlines automatically match this stride, preventing dense clutter across the plot area.

---

### Trục Y (Value Axis) và AutoRange / Value Axis & AutoRange

```csharp
using MEGraph.MAUI.Axes.Line;

// Cách 1 / Approach 1: Tự động tính khoảng giá trị theo Data (Khuyên dùng)
// Auto-scale value range based on data (Recommended)
var autoValueAxis = new Value
{
    Orientation = AxisOrientation.Y,
    IsAutoRange = true,                            // VI: Tự động tính Min/Max  | EN: Auto-calculate Min/Max
    ShowGridLines = true,                          // VI: Bật đường dóng ngang   | EN: Enable horizontal gridlines
    GridColor = Color.FromArgb("#E5E7EB"),
    GridLineWidth = 1f
};

// Cách 2 / Approach 2: Cố định khoảng giá trị thủ công
// Fixed custom range
var fixedValueAxis = new Value
{
    Orientation = AxisOrientation.Y,
    IsAutoRange = false,                           // VI: Tắt tự động tính       | EN: Disable auto-range
    MinValue = -2f,
    MaxValue = 10f,
    TickCount = 5                                  // VI: Số vạch chia trục tung | EN: Number of Y ticks
};
```

---

## 3. Đường Cong Mềm Mại / Smooth Spline Curves

- **VI**: Loại bỏ các góc nhọn trên đường vẽ, tự động bo cong mềm mại với giải thuật Fritsch-Carlson bảo toàn độ dốc (không bị lọt/lẹm trục).
- **EN**: Eliminates sharp corners and renders organic, smooth curves using the Fritsch-Carlson Monotone Cubic Spline algorithm without overshoot.

```csharp
using MEGraph.MAUI.Series.Line;
using MEGraph.MAUI.Theme;

var smoothSeries = new LineSeries
{
    Name = "Temperature",
    StrokeColor = ChartColors.Orange,
    StrokeWidth = 3f,
    
    // VI: Bật đường cong Monotone Cubic Spline | EN: Enable Monotone Cubic Spline curve
    IsSmooth = true,
    
    // VI: Độ căng đường cong (0.0f - 0.5f, mặc định 0.25f) | EN: Curve tension (0.0f - 0.5f, default 0.25f)
    SmoothTension = 0.25f,
    
    Data = new List<float> { 22f, 24f, 28f, 33f, 31f, 27f, 23f }
};
```

---

## 4. Tô Màu Gradient Diện Tích / Gradient Area Fill

- **VI**: Sử dụng trong `AreaChart` hoặc `AreaSeries` để tạo hiệu ứng diện tích mờ dần từ đường đỉnh xuống đáy.
- **EN**: Used in `AreaChart` or `AreaSeries` to create beautiful downward fading fill gradients.

```csharp
using MEGraph.MAUI.Series.Area;
using MEGraph.MAUI.Theme;

var areaSeries = new AreaSeries
{
    Name = "Power Output",
    StrokeColor = ChartColors.Teal,
    StrokeWidth = 3f,
    IsSmooth = true,
    
    // VI: Bật gradient chuyển sắc mờ dần | EN: Enable vertical fade gradient fill
    UseGradientFill = true,
    
    Data = new List<float> { 0f, 1.2f, 3.5f, 4.8f, 2.1f, 0.4f }
};
```

---

## 5. Ẩn / Hiện Series Động / Toggle Series Visibility

- **VI**: Khi người dùng nhấn vào Legend hoặc nút bấm tùy chỉnh để bật/tắt hiển thị từng đường series.
- **EN**: Allows users to interactively show or hide individual series when clicking on legends or buttons.

### Các hàm hỗ trợ trong `BaseChart` / Built-in Methods in `BaseChart`:
```csharp
// 1. Đảo trạng thái hiển thị (Bật -> Tắt, Tắt -> Bật) / Toggle visibility
myChart.ToggleSeries("SeriesName");

// 2. Thiết lập trạng thái hiển thị cụ thể / Set explicit visibility state
myChart.SetSeriesVisibility("SeriesName", isVisible: false);
```

### Ví dụ xử lý sự kiện trong XAML & C# / Code Example:

**XAML:**
```xml
<Border x:Name="BtnTogglePv" BackgroundColor="White" Padding="12,8">
    <Border.GestureRecognizers>
        <TapGestureRecognizer Tapped="OnTogglePvClicked" />
    </Border.GestureRecognizers>
    <Label Text="Solar PV" />
</Border>

<charts:LineChart x:Name="MyChart" HeightRequest="300" />
```

**C# Code-Behind:**
```csharp
private void OnTogglePvClicked(object sender, EventArgs e)
{
    // VI: Bật/tắt Series "PV" | EN: Toggle series "PV"
    MyChart.ToggleSeries("PV");
    
    // VI: Cập nhật độ mờ nút bấm theo trạng thái | EN: Update button opacity accordingly
    var pvSeries = MyChart.Series.FirstOrDefault(s => s.Name == "PV");
    BtnTogglePv.Opacity = (pvSeries?.IsVisible == true) ? 1.0 : 0.4;
}
```

> **Lợi ích trong phiên bản 2.6.1 / Improvements in v2.6.1:**
> - **VI**: Khi ẩn/hiện Series, Canvas được dọn dẹp sạch sẽ (không bị bóng ma/chồng hình cũ), và các đường dóng ngang/dọc tự động đồng bộ theo khoảng dữ liệu mới.
> - **EN**: Toggling series cleanly repaints without ghosting artifacts, and gridlines seamlessly re-adapt to the new data bounds.

---

## 6. Hiệu Ứng Hoạt Họa Vẽ Vào / Draw-In Entry Animation

- **VI**: Biểu đồ tự động thực hiện hiệu ứng vẽ dần từ trái sang phải mượt mà khi vừa hiển thị hoặc khi thay đổi dữ liệu.
- **EN**: Charts smoothly draw series paths from left to right upon initial load or data transitions.

### Cấu hình qua XAML hoặc Code / Configuration:
```xml
<charts:LineChart x:Name="MyChart"
                  AnimationDuration="1500" /> <!-- 1500ms = 1.5s -->
```

### Kích hoạt lại hiệu ứng / Triggering Programmatically:
```csharp
// VI: Chạy lại hiệu ứng với thời gian mặc định | EN: Replay with default duration
MyChart.PlayEntryAnimation();

// VI: Hoặc chỉ định thời gian mới (ms)          | EN: Or specify duration in milliseconds
MyChart.PlayEntryAnimation(duration: 2000);
```

---

## 7. Ví Dụ Tổng Hợp Đầy Đủ / Full Working Example

Dưới đây là ví dụ hoàn chỉnh khởi tạo một biểu đồ đường đa chuỗi với đầy đủ các tính năng:  
*A complete working example configuring a multi-series line chart with all modern features:*

```csharp
using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Axes.Line;
using MEGraph.MAUI.Series.Line;
using MEGraph.MAUI.Theme;
using System.Collections.ObjectModel;

private void SetupMyChart()
{
    // 1. Dọn dẹp dữ liệu cũ (nếu có) / Clear previous series if any
    MyChart.SeriesList.Clear();
    ((MEGraph.MAUI.Cores.BaseChart)MyChart).Series.Clear();

    // 2. Tạo Series 1 (PV) / Create Series 1 (Solar PV)
    var pvSeries = new LineSeries
    {
        Name = "PV",
        StrokeColor = ChartColors.Orange,
        StrokeWidth = 3f,
        IsSmooth = true,
        Data = new List<float> { 0f, 0f, 0.8f, 2.2f, 4.2f, 2.5f, 0f }
    };

    // 3. Tạo Series 2 (Lưới điện) / Create Series 2 (Grid Power)
    var gridSeries = new LineSeries
    {
        Name = "Grid",
        StrokeColor = ChartColors.Blue,
        StrokeWidth = 3f,
        IsSmooth = true,
        Data = new List<float> { 1.5f, 1.2f, 0.2f, 0f, 0.5f, 1.8f, 2.0f }
    };

    MyChart.AddSeries(pvSeries);
    MyChart.AddSeries(gridSeries);

    // 4. Thiết lập Trục X (Category Axis) / Configure X-Axis (Category)
    var xAxis = new Category
    {
        Orientation = AxisOrientation.X,
        AutoSkip = true,
        ShowGridLines = true,
        GridColor = Color.FromArgb("#F3F4F6"),
        Labels = new List<AxisLabel>
        {
            new AxisLabel("00:00"),
            new AxisLabel("04:00"),
            new AxisLabel("08:00"),
            new AxisLabel("12:00"),
            new AxisLabel("16:00"),
            new AxisLabel("20:00"),
            new AxisLabel("24:00")
        }
    };

    // 5. Thiết lập Trục Y (Value Axis) / Configure Y-Axis (Value)
    var yAxis = new Value
    {
        Orientation = AxisOrientation.Y,
        IsAutoRange = true, // VI: Tự động co dãn theo Data | EN: Auto-scale with data
        ShowGridLines = true,
        GridColor = Color.FromArgb("#F3F4F6")
    };

    MyChart.Axes = new ObservableCollection<IAxis> { xAxis, yAxis };

    // 6. Kích hoạt hiệu ứng vẽ / Play draw-in animation
    MyChart.PlayEntryAnimation(1500);
}
```
