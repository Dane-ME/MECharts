MyCharts/
├── Core/
│   ├── BaseChart.cs
│   ├── ChartOptions.cs
│   ├── IChartDrawable.cs
│   └── Utils/
├── Axes/
│   ├── IAxis.cs
│   ├── XAxis.cs
│   ├── YAxis.cs
│   ├── CategoryAxis.cs
│   └── ValueAxis.cs
├── Legends/
│   ├── ILegend.cs
│   ├── DefaultLegend.cs
│   └── CustomLegend.cs
├── Series/
│   ├── ISeries.cs
│   ├── LineSeries.cs
│   ├── ColumnSeries.cs
│   ├── PieSeries.cs
│   ├── StackedLineSeries.cs
│   └── ...
├── Charts/
│   ├── LineChart.cs
│   ├── ColumnChart.cs
│   ├── PieChart.cs
│   └── CompositeChart.cs
├── Themes/
│   ├── DefaultTheme.cs
│   └── DarkTheme.cs
└── Samples/
    ├── LineChartSample.xaml
    ├── ColumnChartSample.xaml
    └── PieChartSample.xaml