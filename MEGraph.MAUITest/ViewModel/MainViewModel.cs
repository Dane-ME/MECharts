using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Axes.Line;
using MEGraph.MAUI.Series;
using MEGraph.MAUI.Series.Line;
using MEGraph.MAUI.Styles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUITest.ViewModel
{
    public class MainViewModel
    {
        private string _chartTitle = "Sales Chart";
        private ObservableCollection<float> _chartData;
        private ObservableCollection<StackedLineSeries> _seriesCollection;
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

        public ObservableCollection<StackedLineSeries> SeriesCollection
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
            SeriesCollection = new ObservableCollection<StackedLineSeries>
            {
                new StackedLineSeries
                {
                    Name = "Sales",
                    Data = ChartData.ToList(),
                    StrokeColor = Colors.Blue,
                    StrokeWidth = 3f
                },
                new StackedLineSeries
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
    }
}
