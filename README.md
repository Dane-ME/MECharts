# MEGraph.MAUI

Một thư viện charting mạnh mẽ và linh hoạt cho .NET MAUI, được thiết kế để tạo ra các biểu đồ đẹp mắt và tương tác trên tất cả các nền tảng di động và desktop.

## ✨ Tính năng

- 🎯 **Đa dạng loại biểu đồ**: Line Chart, Stacked Line Chart, Column Chart, Pie Chart và nhiều hơn nữa
- �� **Đa nền tảng**: Hỗ trợ Android, iOS, macOS, Windows và Tizen
- �� **Tùy chỉnh cao**: Màu sắc, font chữ, kích thước, style có thể tùy chỉnh hoàn toàn
- 📊 **Data Binding**: Hỗ trợ đầy đủ data binding với ObservableCollection
- ⚡ **Hiệu suất cao**: Được tối ưu hóa cho hiệu suất vẽ và cập nhật dữ liệu
- 🔧 **Dễ sử dụng**: API đơn giản và trực quan

## �� Cài đặt

### NuGet Package
```xml
<PackageReference Include="MEGraph.MAUI" Version="1.0.0" />
```

### Thêm vào MauiProgram.cs
```csharp
using MEGraph.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMEGraph(); // Thêm dòng này
            
        return builder.Build();
    }
}
```

## 📖 Sử dụng cơ bản

### 1. Line Chart đơn giản

```xml
<charts:LineChart 
    Data="{Binding ChartData}"
    Title="Doanh thu theo tháng" />
```

```csharp
// Trong ViewModel
public ObservableCollection<float> ChartData { get; set; } = new()
{
    10, 30, 15, 50, 40, 60, 100, 0, 50
};