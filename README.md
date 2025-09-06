# MEGraph.MAUI

A powerful and flexible charting library for .NET MAUI applications, built with modern architecture and pipeline-based rendering.

## 🚀 Features

- **📊 Multiple Chart Types**: Line, Bar, Column, Pie, Area, Radar, Stock charts
- **🎨 Customizable Styling**: Colors, fonts, themes, and visual effects
- **📱 Cross-Platform**: Android, iOS, macOS, Windows support
- **⚡ High Performance**: Pipeline-based rendering for smooth animations
- **🔗 Data Binding**: Full MVVM support with ObservableCollection
- **🎯 Flexible Axes**: Category and Value axes with custom labels
- **�� Multiple Series**: Support for multiple data series in one chart
- **�� Themes**: Built-in light and dark themes

## 📦 Installation

Add the NuGet package to your .NET MAUI project:

```xml
<PackageReference Include="MEGraph.MAUI" Version="2.0.0" />
```

## ��️ Architecture

### Core Components

MEGraph.MAUI/
├── Cores/ # Core rendering engine
│ ├── BaseChart.cs # Base chart control
│ ├── Pipeline/ # Rendering pipeline
│ │ ├── IRenderPipeline.cs
│ │ └── LineRenderPipeline.cs
│ └── Components/ # Render components
│ └── Line/
│ └── Renderers/
│ ├── Axes.cs
│ ├── Series.cs
│ ├── Title.cs
│ └── Legend.cs
├── Charts/ # Chart implementations
│ ├── LineChart.cs
│ └── Line/
│ ├── Base.cs
│ └── Line.cs
├── Series/ # Data series
│ ├── ISeries.cs
│ └── LineSeries.cs
├── Axes/ # Axis system
│ ├── IAxis.cs
│ ├── CategoryAxis.cs
│ ├── ValueAxis.cs
│ └── Line/
│ ├── Category.cs
│ └── Value.cs
├── Legends/ # Legend system
│ ├── ILegend.cs
│ └── DefaultLegend.cs
└── Title/ # Title system
└── ITitle.cs

### Pipeline Architecture

The library uses a modern pipeline-based rendering system:

1. **Pipeline Layer**: Orchestrates the rendering process
2. **Component Layer**: Individual renderers for different chart elements
3. **Data Layer**: Series and axes data management

## �� Quick Start

### 1. Basic Line Chart

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:charts="clr-namespace:MEGraph.MAUI.Charts"
             x:Class="MyApp.MainPage">
    
    <Grid>
        <charts:LineChart x:Name="MyChart" 
                          Title="Sales Data"
                          Data="{Binding ChartData}" />
    </Grid>
</ContentPage>
```

```csharp
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        
        // Set data
        MyChart.SetData(new float[] { 100, 120, 90, 150, 200, 180, 220 });
    }
}