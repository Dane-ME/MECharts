MECharts/
│
├── Core/             # Thành phần cốt lõi của thư viện
│   ├── BaseChart.cs  # Lớp trừu tượng cho tất cả các loại biểu đồ.
│   ├── ChartOptions.cs # Cấu hình chung cho biểu đồ như lề (margin), màu nền và chủ đề.
│   ├── IChartDrawable.cs # Giao diện (interface) để xử lý việc vẽ biểu đồ.
│   └── Utils/          # Các hàm tiện ích hỗ trợ, ví dụ: tính toán tỷ lệ, xử lý màu sắc, và các hàm toán học.
│
├── Axes/             # Các lớp cho trục tọa độ
│   ├── IAxis.cs      # Giao diện cơ bản cho các trục.
│   ├── XAxis.cs      # Trục hoành.
│   ├── YAxis.cs      # Trục tung.
│   ├── CategoryAxis.cs # Trục cho dữ liệu dạng danh mục.
│   └── ValueAxis.cs  # Trục cho dữ liệu dạng giá trị.
│
├── Legends/          # Các lớp cho phần chú giải (legend)
│   ├── ILegend.cs    # Giao diện cơ bản cho chú giải.
│   ├── DefaultLegend.cs # Chú giải mặc định.
│   └── CustomLegend.cs # Chú giải tùy chỉnh.
│
├── Series/           # Các lớp đại diện cho các chuỗi dữ liệu
│   ├── ISeries.cs    # Giao diện cho chuỗi dữ liệu (đã bao gồm logic vẽ).
│   ├── LineSeries.cs # Chuỗi dữ liệu cho biểu đồ đường.
│   ├── ColumnSeries.cs # Chuỗi dữ liệu cho biểu đồ cột.
│   ├── PieSeries.cs  # Chuỗi dữ liệu cho biểu đồ tròn.
│   └── ...           # Và nhiều loại series khác.
│
├── Charts/           # Các lớp biểu đồ hoàn chỉnh
│   ├── LineChart.cs    # Biểu đồ đường, sử dụng LineSeries.
│   ├── ColumnChart.cs  # Biểu đồ cột.
│   ├── PieChart.cs     # Biểu đồ tròn.
│   └── CompositeChart.cs # Biểu đồ phức hợp, cho phép kết hợp nhiều loại series.
│
├── Themes/           # Các chủ đề giao diện
│   ├── DefaultTheme.cs # Chủ đề mặc định.
│   └── DarkTheme.cs    # Chủ đề tối.
│
└── Samples/          # Các ví dụ minh họa
    ├── LineChartSample.xaml # Ví dụ cho biểu đồ đường.
    ├── ColumnChartSample.xaml # Ví dụ cho biểu đồ cột.
    └── PieChartSample.xaml # Ví dụ cho biểu đồ tròn.