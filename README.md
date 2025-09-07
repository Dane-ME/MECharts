# MEGraph.MAUI

A powerful and flexible charting library for .NET MAUI applications, built with modern architecture and pipeline-based rendering.

Note: Currently, only the standard, stacked line chart is functional. Other features will be added in future updates.
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
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:charts="clr-namespace:MEGraph.MAUI.Charts;assembly=MEGraph.MAUI"
             xmlns:viewmodel="clr-namespace:MEGraph.MAUITest.ViewModel;assembly=MEGraph.MAUITest"
             x:Class="MEGraph.MAUITest.MainPage">
    <ContentPage.Resources>
        <ResourceDictionary>
            <viewmodel:MainViewModel x:Key="MainViewModel"/>
        </ResourceDictionary>
    </ContentPage.Resources>
    <ContentPage.BindingContext>
        <StaticResource Key="MainViewModel"/>
    </ContentPage.BindingContext>
    
    <charts:LineChart 
        x:Name="lineChart"
        Title="{Binding ChartTitle}"
        SeriesItems="{Binding SeriesCollection}"
        ChartAxes="{Binding AxesCollection}"
        BackgroundColor="White"
        Margin="16"
        />
</ContentPage>

```

```csharp
private string _chartTitle = "Sales Chart";
        private ObservableCollection<float> _chartData;
        private ObservableCollection<LineSeries> _seriesCollection;
        private ObservableCollection<IAxis> _axesCollection;

        public string ChartTitle
        {
            get => _chartTitle;
            set => SetProperty(ref _chartTitle, value);
        }

        public ObservableCollection<float> ChartData
        {
            get => _chartData;
            set => SetProperty(ref _chartData, value);
        }

        public ObservableCollection<LineSeries> SeriesCollection
        {
            get => _seriesCollection;
            set => SetProperty(ref _seriesCollection, value);
        }

        public ObservableCollection<IAxis> AxesCollection
        {
            get => _axesCollection;
            set => SetProperty(ref _axesCollection, value);
        }
        public MainViewModel()
        {
            // Dữ liệu mẫu
            ChartData = new ObservableCollection<float>
            {
                100, 120, 90, 150, 200, 180, 220, 190, 250, 300, 280, 320
            };

            var ChartData1 = new ObservableCollection<float>
            {
                140, 120, 150, 150, 200, 190, 250, 190, 260, 310, 300, 330
            };

            // Tạo series
            SeriesCollection = new ObservableCollection<LineSeries>
            {
                new LineSeries
                {
                    Name = "Sales",
                    Data = ChartData.ToList(),
                    StrokeColor = Colors.Blue,
                    StrokeWidth = 3f
                },
                new LineSeries
                {
                    Name = "Sales1",
                    Data = ChartData1.ToList(),
                    StrokeColor = Colors.Red,
                    StrokeWidth = 3f
                }
            };

            // Tạo axes
            AxesCollection = new ObservableCollection<IAxis>
            {
                new Category
                {
                    Orientation = AxisOrientation.X,
                    Labels = new List<AxisLabel>
                    {
                        new AxisLabel { Content = "Jan" },
                        new AxisLabel { Content = "Feb" },
                        new AxisLabel { Content = "Mar" },
                        new AxisLabel { Content = "Apr" },
                        new AxisLabel { Content = "May" },
                        new AxisLabel { Content = "Jun" },
                        new AxisLabel { Content = "Jul" },
                        new AxisLabel { Content = "Aug" },
                        new AxisLabel { Content = "Sep" },
                        new AxisLabel { Content = "Oct" },
                        new AxisLabel { Content = "Nov" },
                        new AxisLabel { Content = "Dec" }
                    }
                },
                new Value
                {
                    Orientation = AxisOrientation.Y,
                    Title = new AxisTitle { Content = "Sales ($)" }
                }
            };
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion