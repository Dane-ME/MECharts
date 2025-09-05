using MEGraph.MAUI.Axes;
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

        public MainViewModel()
        {
            ChartData = new ObservableCollection<float> { 10, 30, 15, 50, 40, 60, 100, 0, 50 };
            ChartAxes = new ObservableCollection<IAxis>
            {
                new ValueAxis { Title = "Y Axis" },
                new CategoryAxis { Title = "X Axis", Labels = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I" } }
            };
        }
    }
}
