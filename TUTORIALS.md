# Cẩm Nang Lập Trình & Hướng Dẫn Sử Dụng MEGraph.MAUI (MECharts)
> **Dành cho Lập trình viên và AI Coding Agents (System Prompt & Developer Cheatsheet)**  
> **Phiên bản thư viện**: 2.6.2+  
> **Nền tảng**: .NET MAUI (.NET 8.0 - .NET 10.0), C# 12  
> **Động cơ đồ họa**: `Microsoft.Maui.Graphics` (Canvas thuần đa nền tảng, không phụ thuộc wrapper bên ngoài)

---

## 📌 QUY TẮC BẮT BUỘC DÀNH CHO AI AGENT KHI VIẾT CODE MEGRAPH
Khi tạo hoặc chỉnh sửa code liên quan đến MEGraph, Agent **PHẢI TUÂN THỦ** các nguyên tắc kiến trúc sau:
1. **Namespace chuẩn mực**:
   - Biểu đồ: `MEGraph.MAUI.Charts.Line`, `MEGraph.MAUI.Charts.Column`, `MEGraph.MAUI.Charts.Area`, `MEGraph.MAUI.Charts.Pie`
   - Chuỗi dữ liệu: `MEGraph.MAUI.Series.Line`, `MEGraph.MAUI.Series.Column`, `MEGraph.MAUI.Series.Area`, `MEGraph.MAUI.Series.Pie`
   - Trục tọa độ: `MEGraph.MAUI.Axes`, `MEGraph.MAUI.Axes.Line`, `MEGraph.MAUI.Styles`
   - Bảng màu: `MEGraph.MAUI.Theme.ChartColors`
2. **Cơ chế thêm Series vào Chart**:
   - Luôn sử dụng hàm `chart.AddSeries(series)` hoặc bind qua `ObservableCollection` (`SeriesItems`).
   - Nếu khởi tạo lại hoặc reset dữ liệu code-behind: luôn gọi `chart.SeriesList.Clear(); ((BaseChart)chart).Series.Clear();` trước khi add để tránh bị nhân bản series mặc định.
3. **Cơ chế Trục (Axes) & Đa Trục Y (Dual Y-Axes)**:
   - Trục X dùng `Category` với `Orientation = AxisOrientation.X`.
   - Trục Y dùng `Value` với `Orientation = AxisOrientation.Y`.
   - Khi có **2 trục Y khác đơn vị**:
     - Trục trái: `AxisPosition = AxisPosition.Left`, gán `Id = "left-axis"`.
     - Trục phải: `AxisPosition = AxisPosition.Right`, gán `Id = "right-axis"`. Đặt `ShowGridLines = false` để tránh đè vạch ngang.
     - Series tương ứng: gán `series.AxisId = "left-axis"` hoặc `series.AxisId = "right-axis"`.
4. **Tiêu đề (Chart Title)**:
   - Thuộc tính `Title` trên `BaseChart` là tùy chọn. Nếu không muốn hiển thị tiêu đề trong chart, hãy đặt `Title = ""` (hoặc không set trong XAML).
5. **Tính năng thẩm mỹ hiện đại (Modern Aesthetics)**:
   - Dùng `IsSmooth = true` cho `LineSeries` để kích hoạt Fritsch-Carlson Spline.
   - Dùng `UseGradient = true`, `GradientEndColor`, và `CornerRadius = 6f` cho `ColumnSeries`.
   - Dùng `ChartColors` (Tableau 10) thay vì các màu cứng `Colors.Red`, `Colors.Blue`.

---

## Mục Lục / Table of Contents
1. [Hệ Thống Bảng Màu Tiêu Chuẩn (ChartColors)](#1-hệ-thống-bảng-màu-tiêu-chuẩn-chartcolors)
2. [Cấu Hình Trục Tọa Độ & Đa Trục Y (Dual Y-Axes)](#2-cấu-hình-trục-tọa-độ--đa-trục-y-dual-y-axes)
   - [Trục X (Category Axis) & AutoSkip](#21-trục-x-category-axis--autoskip)
   - [Trục Y Đơn (Single Value Axis) & AutoRange](#22-trục-y-đơn-single-value-axis--autorange)
   - [Trục Y Kép (Dual Y-Axes) cho 2 đơn vị khác nhau](#23-trục-y-kép-dual-y-axes-cho-2-đơn-vị-khác-nhau)
3. [Biểu Đồ Cột Dọc (ColumnChart) & Cột Nhóm (Clustered Bars)](#3-biểu-đồ-cột-dọc-columnchart--cột-nhóm-clustered-bars)
4. [Biểu Đồ Đường (LineChart) & Đường Cong Spline Mềm Mại](#4-biểu-đồ-đường-linechart--đường-cong-spline-mềm-mại)
5. [Biểu Đồ Miền Diện Tích (AreaChart) & Gradient Fill](#5-biểu-đồ-miền-diện-tích-areachart--gradient-fill)
6. [Tương Tác Ẩn / Hiện Series (Toggle Series Visibility)](#6-tương-tác-ẩn--hiện-series-toggle-series-visibility)
7. [Tiêu Đề Biểu Đồ (Title: Có hoặc Không có)](#7-tiêu-đề-biểu-đồ-title-có-hoặc-không-có)
8. [Hiệu Ứng Hoạt Họa (Draw-in Entry Animation)](#8-hiệu-ứng-hoạt-họa-draw-in-entry-animation)
9. [Cú Pháp XAML & MVVM Binding Hoàn Chỉnh](#9-cú-pháp-xaml--mvvm-binding-hoàn-chỉnh)

---

## 1. Hệ Thống Bảng Màu Tiêu Chuẩn (ChartColors)

Lớp tĩnh `MEGraph.MAUI.Theme.ChartColors` cung cấp bảng màu **Tableau 10** chuẩn quốc tế:

```csharp
using MEGraph.MAUI.Theme;

Color blue   = ChartColors.Blue;    // #4e79a7 (Màu chính mặc định)
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

### Hàm tiện ích:
```csharp
// Lấy màu tuần hoàn theo index (rất tiện khi duyệt danh sách series)
Color color = ChartColors.GetColor(i);

// Lấy danh sách màu không trùng lặp (dùng cho PieChart slices)
var distinctColors = ChartColors.GetDistinctColors(existingColors, count: 5);
```

---

## 2. Cấu Hình Trục Tọa Độ & Đa Trục Y (Dual Y-Axes)

### 2.1. Trục X (Category Axis) & AutoSkip
Dùng để hiển thị nhãn danh mục (thời gian, tháng, danh mục sản phẩm):
```csharp
using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Axes.Line;
using MEGraph.MAUI.Styles;

var xAxis = new Category
{
    Orientation = AxisOrientation.X,
    ShowGridLines = true,                          // Bật đường dóng dọc
    GridColor = Color.FromArgb("#F1F5F9"),         // Màu lưới mờ
    GridLineWidth = 1f,
    AutoSkip = true,                               // Tự động thưa nhãn khi màn hình hẹp
    Labels = new List<AxisLabel>
    {
        new AxisLabel("Tháng 1"),
        new AxisLabel("Tháng 2"),
        new AxisLabel("Tháng 3"),
        new AxisLabel("Tháng 4"),
        new AxisLabel("Tháng 5"),
        new AxisLabel("Tháng 6")
    },
    TotalPoints = 6                                // Khớp với số lượng điểm dữ liệu
};
```

---

### 2.2. Trục Y Đơn (Single Value Axis) & AutoRange
```csharp
// Cách 1: Tự động tính Min/Max theo dữ liệu (Khuyên dùng)
var autoYAxis = new Value
{
    Orientation = AxisOrientation.Y,
    AxisPosition = AxisPosition.Left,
    IsAutoRange = true,
    ShowGridLines = true,
    GridColor = Color.FromArgb("#F1F5F9")
};

// Cách 2: Cố định khoảng giá trị tùy chỉnh
var fixedYAxis = new Value
{
    Orientation = AxisOrientation.Y,
    AxisPosition = AxisPosition.Left,
    IsAutoRange = false,
    MinValue = 0f,
    MaxValue = 100f,
    TickCount = 5,
    ShowGridLines = true
};
```

---

### 2.3. Trục Y Kép (Dual Y-Axes) cho 2 đơn vị khác nhau
Dùng khi biểu đồ cần mô tả 2 đại lượng khác nhau (ví dụ: Cột sản lượng `kWh` ở trục trái và Cột/Đường doanh thu `Triệu VNĐ` ở trục phải):

```csharp
// 1. Trục Y Trái: Đại lượng 1 (kWh)
var leftYAxis = new Value
{
    Id = "axis-kwh",
    Orientation = AxisOrientation.Y,
    AxisPosition = AxisPosition.Left,
    Title = new AxisTitle("kWh"),
    MinValue = 0f,
    MaxValue = 500f,
    TickCount = 6,
    ShowGridLines = true,
    GridColor = Color.FromArgb("#F1F5F9")
};

// 2. Trục Y Phải: Đại lượng 2 (Triệu VNĐ)
var rightYAxis = new Value
{
    Id = "axis-vnd",
    Orientation = AxisOrientation.Y,
    AxisPosition = AxisPosition.Right,
    Title = new AxisTitle("Tr.đ"),
    MinValue = 0f,
    MaxValue = 50f,
    TickCount = 6,
    ShowGridLines = false  // QUAN TRỌNG: Tắt gridline trục phải để tránh đè vạch
};

// Gán trục vào Chart
myChart.Axes = new ObservableCollection<IAxis> { xAxis, leftYAxis, rightYAxis };
```

---

## 3. Biểu Đồ Cột Dọc (ColumnChart) & Cột Nhóm (Clustered Bars)

`ColumnChart` hỗ trợ vẽ nhiều cột nhóm (clustered group) trong cùng một category, bo góc tròn và tô màu gradient hiện đại.

```csharp
using MEGraph.MAUI.Charts.Column;
using MEGraph.MAUI.Series.Column;

// 1. Khởi tạo Series 1 (gắn vào Trục Trái qua AxisId)
var colSeries1 = new ColumnSeries
{
    Name = "Sản lượng",
    AxisId = "axis-kwh",
    FillColor = Color.FromArgb("#2563EB"),
    GradientEndColor = Color.FromArgb("#60A5FA"),
    UseGradient = true,               // Bật chuyển sắc mờ hiện đại
    CornerRadius = 6f,                // Bo tròn đỉnh cột
    Data = new List<float> { 280f, 350f, 420f, 390f, 480f, 440f }
};

// 2. Khởi tạo Series 2 (gắn vào Trục Phải qua AxisId)
var colSeries2 = new ColumnSeries
{
    Name = "Doanh thu",
    AxisId = "axis-vnd",
    FillColor = Color.FromArgb("#F59E0B"),
    GradientEndColor = Color.FromArgb("#FDE68A"),
    UseGradient = true,
    CornerRadius = 6f,
    Data = new List<float> { 18f, 24f, 35f, 31f, 45f, 39f }
};

// Thêm vào ColumnChart
columnChart.AddSeries(colSeries1);
columnChart.AddSeries(colSeries2);
```

---

## 4. Biểu Đồ Đường (LineChart) & Đường Cong Spline Mềm Mại

Dùng `IsSmooth = true` để áp dụng thuật toán Fritsch-Carlson Monotone Spline (không gây overshoot vượt quá trục):

```csharp
using MEGraph.MAUI.Charts.Line;
using MEGraph.MAUI.Series.Line;

var lineSeries = new LineSeries
{
    Name = "Nhiệt độ",
    StrokeColor = Color.FromArgb("#10B981"),
    StrokeWidth = 3f,
    IsSmooth = true,                  // Bật đường cong mềm mại
    SmoothTension = 0.25f,            // Độ căng đường cong (0.0f - 0.5f, mặc định 0.25f)
    Data = new List<float> { 22f, 24f, 28f, 33f, 31f, 27f }
};

lineChart.AddSeries(lineSeries);
```

---

## 5. Biểu Đồ Miền Diện Tích (AreaChart) & Gradient Fill

```csharp
using MEGraph.MAUI.Charts.Area;
using MEGraph.MAUI.Series.Area;

var areaSeries = new AreaSeries
{
    Name = "Công suất",
    StrokeColor = Color.FromArgb("#06B6D4"),
    StrokeWidth = 3f,
    IsSmooth = true,
    UseGradientFill = true,           // Mờ dần diện tích từ đỉnh xuống trục hoành
    Data = new List<float> { 1.2f, 2.5f, 4.8f, 3.1f, 1.5f, 0.5f }
};

areaChart.AddSeries(areaSeries);
```

---

## 6. Tương Tác Ẩn / Hiện Series (Toggle Series Visibility)

Hỗ trợ click nút/legend bên ngoài XAML để ẩn hoặc hiện từng Series:

```csharp
// Bật/tắt trạng thái hiển thị của Series theo tên
myChart.ToggleSeries("Sản lượng");

// Hoặc đặt rõ trạng thái true/false
myChart.SetSeriesVisibility("Sản lượng", isVisible: false);
```

**Ví dụ Code-Behind kết hợp cập nhật Opacity giao diện nút:**
```csharp
private void OnToggleSeriesClicked(object sender, EventArgs e)
{
    myChart.ToggleSeries("Sản lượng");
    
    var s = myChart.Series.FirstOrDefault(x => x.Name == "Sản lượng");
    BtnToggle.Opacity = (s?.IsVisible == true) ? 1.0 : 0.35;
}
```

---

## 7. Tiêu Đề Biểu Đồ (Title: Có hoặc Không có)

Biểu đồ MEGraph hỗ trợ linh hoạt cả 2 trường hợp:

### Trường hợp 1: Không dùng Title (Ẩn hoàn toàn)
Khi không truyền hoặc truyền chuỗi rỗng `""`, renderer sẽ bỏ qua việc vẽ tiêu đề, giúp đồ thị có không gian hiển thị tối đa:
```xml
<!-- XAML: Bỏ trống Title -->
<colCharts:ColumnChart x:Name="ColChart" Title="" HeightRequest="300" />
```
```csharp
// C#: Đặt rỗng hoặc null
myChart.Title = "";
```

### Trường hợp 2: Có Title tích hợp
```xml
<colCharts:ColumnChart x:Name="ColChart" Title="Báo Cáo Sản Lượng 2026" HeightRequest="300" />
```
```csharp
myChart.Title = "Báo Cáo Sản Lượng 2026";
```

---

## 8. Hiệu Ứng Hoạt Họa (Draw-in Entry Animation)

Biểu đồ tự động chạy hiệu ứng vẽ vào (draw-in) khi tải trang. Có thể kích hoạt lại bất kỳ lúc nào:

```xml
<colCharts:ColumnChart x:Name="ColChart"
                       AnimationDuration="1500" /> <!-- Thời lượng ms -->
```

```csharp
// Chạy lại hiệu ứng
myChart.PlayEntryAnimation();

// Hoặc truyền thời gian tùy chỉnh (ms)
myChart.PlayEntryAnimation(duration: 2000);
```

---

## 9. Cú Pháp XAML & MVVM Binding Hoàn Chỉnh

### 9.1. Khai báo Namespace XAML chuẩn
```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:lineCharts="clr-namespace:MEGraph.MAUI.Charts.Line;assembly=MEGraph.MAUI"
             xmlns:colCharts="clr-namespace:MEGraph.MAUI.Charts.Column;assembly=MEGraph.MAUI"
             xmlns:areaCharts="clr-namespace:MEGraph.MAUI.Charts.Area;assembly=MEGraph.MAUI"
             xmlns:pieCharts="clr-namespace:MEGraph.MAUI.Charts.Pie;assembly=MEGraph.MAUI"
             x:Class="MyApp.MainPage">

    <ScrollView>
        <VerticalStackLayout Padding="16" Spacing="20">

            <!-- Biểu đồ cột trục kép -->
            <colCharts:ColumnChart x:Name="MyColumnChart"
                                   HeightRequest="300"
                                   AnimationDuration="1200"
                                   BackgroundColor="Transparent" />

            <!-- Biểu đồ đường cong -->
            <lineCharts:LineChart x:Name="MyLineChart"
                                  HeightRequest="280"
                                  AnimationDuration="1200"
                                  BackgroundColor="Transparent" />

        </VerticalStackLayout>
    </ScrollView>
</ContentPage>
```

### 9.2. Code-behind / ViewModel thiết lập chuẩn mẫu (Boilerplate)
```csharp
using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Axes.Line;
using MEGraph.MAUI.Charts.Column;
using MEGraph.MAUI.Series.Column;
using MEGraph.MAUI.Styles;
using System.Collections.ObjectModel;

public void InitializeDashboard()
{
    // 1. Reset các series cũ nếu có
    MyColumnChart.SeriesList.Clear();
    ((MEGraph.MAUI.Cores.BaseChart)MyColumnChart).Series.Clear();

    // 2. Tạo 2 series cho 2 đơn vị khác biệt
    var col1 = new ColumnSeries
    {
        Name = "Điện năng (kWh)",
        AxisId = "axis-left",
        FillColor = Color.FromArgb("#2563EB"),
        GradientEndColor = Color.FromArgb("#60A5FA"),
        UseGradient = true,
        CornerRadius = 6f,
        Data = new List<float> { 120, 250, 310, 420, 380, 490 }
    };

    var col2 = new ColumnSeries
    {
        Name = "Chi phí (Tr.đ)",
        AxisId = "axis-right",
        FillColor = Color.FromArgb("#F59E0B"),
        GradientEndColor = Color.FromArgb("#FDE68A"),
        UseGradient = true,
        CornerRadius = 6f,
        Data = new List<float> { 12, 25, 30, 45, 39, 52 }
    };

    MyColumnChart.AddSeries(col1);
    MyColumnChart.AddSeries(col2);

    // 3. Tạo trục hoành X
    var xAxis = new Category
    {
        Orientation = AxisOrientation.X,
        Labels = new List<AxisLabel>
        {
            new AxisLabel("T1"), new AxisLabel("T2"), new AxisLabel("T3"),
            new AxisLabel("T4"), new AxisLabel("T5"), new AxisLabel("T6")
        },
        TotalPoints = 6
    };

    // 4. Tạo 2 trục tung Y
    var yLeft = new Value
    {
        Id = "axis-left",
        Orientation = AxisOrientation.Y,
        AxisPosition = AxisPosition.Left,
        Title = new AxisTitle("kWh"),
        MinValue = 0, MaxValue = 600, TickCount = 6,
        ShowGridLines = true
    };

    var yRight = new Value
    {
        Id = "axis-right",
        Orientation = AxisOrientation.Y,
        AxisPosition = AxisPosition.Right,
        Title = new AxisTitle("Tr.đ"),
        MinValue = 0, MaxValue = 60, TickCount = 6,
        ShowGridLines = false
    };

    MyColumnChart.Axes = new ObservableCollection<IAxis> { xAxis, yLeft, yRight };
    MyColumnChart.PlayEntryAnimation(1500);
}
```
