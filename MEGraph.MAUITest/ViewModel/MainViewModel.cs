using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Styles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUITest.ViewModel
{
    public class MainViewModel
    {
        public ObservableCollection<float> ChartData { get; set; }
        public string Title { get; set; } = "Line Chart Example";
        public ObservableCollection<IAxis> ChartAxes { get; set; }

        public ObservableCollection<MEGraph.MAUI.Series.LineSeries> MySeries { get; set; }
        

        public MainViewModel()
        {
            ChartData = new ObservableCollection<float> { 10, 30, 15, 50, 40, 60, 100, 0, 50 };
            ChartAxes = new ObservableCollection<IAxis>
            {
                new ValueAxis
                {
                    Title = new AxisTitle("Revenue")
                    {
                        FontSize = 16,
                        FontColor = Colors.DarkBlue
                    },
                    Orientation = AxisOrientation.Y
                },
                new CategoryAxis
                {
                    Title = new AxisTitle("Months")
                    {
                        FontSize = 16,
                        FontColor = Colors.DarkRed
                    },
                    Orientation = AxisOrientation.X,
                    Labels = new List<AxisLabel>
                    {
                        new AxisLabel("A") { FontSize = 12, FontColor = Colors.Black },
                        new AxisLabel("B") { FontSize = 12, FontColor = Colors.Black },
                        new AxisLabel("C") { FontSize = 12, FontColor = Colors.Black },
                        new AxisLabel("D") { FontSize = 12, FontColor = Colors.Black },
                        new AxisLabel("E") { FontSize = 12, FontColor = Colors.Black },
                        new AxisLabel("F") { FontSize = 12, FontColor = Colors.Black },
                        new AxisLabel("G") { FontSize = 12, FontColor = Colors.Black },
                        new AxisLabel("H") { FontSize = 12, FontColor = Colors.Black },
                        new AxisLabel("I") { FontSize = 12, FontColor = Colors.Black }
                    }
                }
            };

            MySeries = new ObservableCollection<MEGraph.MAUI.Series.LineSeries>
{
    new MEGraph.MAUI.Series.LineSeries
    {
        Name = "Product A",
        Data = new List<float> { 10, 20, 15, 30, 25 },
        StrokeColor = Colors.Red,
        StrokeWidth = 3f,
    },
    new MEGraph.MAUI.Series.LineSeries
    {
        Name = "Product B",
        Data = new List<float> { 12, 18, 22, 28, 26 },
        StrokeColor = Colors.Blue,
        StrokeWidth = 3f
    }
};

            ChartAxes = new ObservableCollection<IAxis>
{
    new ValueAxis
    {
        Title = new AxisTitle("Revenue")
        {
            FontSize = 16,
            FontColor = Colors.DarkBlue
        },
        Orientation = AxisOrientation.Y
    },
    new CategoryAxis
    {
        Title = new AxisTitle("Months")
        {
            FontSize = 16,
            FontColor = Colors.DarkRed
        },
        Orientation = AxisOrientation.X,
        Labels = new List<AxisLabel>
        {
            new AxisLabel("A") { FontSize = 12, FontColor = Colors.Black },
            new AxisLabel("B") { FontSize = 12, FontColor = Colors.Black },
            new AxisLabel("C") { FontSize = 12, FontColor = Colors.Black },
            new AxisLabel("D") { FontSize = 12, FontColor = Colors.Black },
            new AxisLabel("E") { FontSize = 12, FontColor = Colors.Black }
        }
    }
};
        }
    }
}
