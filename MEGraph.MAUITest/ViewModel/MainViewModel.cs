using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Axes.Pie;
using MEGraph.MAUI.Series.Pie;
using MEGraph.MAUI.Styles;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MEGraph.MAUITest.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<float> _pieData;
        private ObservableCollection<IAxis> _pieAxes;
        private ObservableCollection<IAxis> _lineAxis;

        public ObservableCollection<float> PieData
        {
            get => _pieData;
            set => SetProperty(ref _pieData, value);
        }

        public ObservableCollection<IAxis> PieAxes
        {
            get => _pieAxes;
            set => SetProperty(ref _pieAxes, value);
        }

        public ObservableCollection<IAxis> LineAxes
        {
            get => _lineAxis;
            set => SetProperty(ref _lineAxis, value);
        }
        public ICommand AddPieDataCommand => new Command(() =>
        {
            PieData = new ObservableCollection<float> { 50, 20, 10, 25, 5, 5 };
        });
        public ICommand AddAxesCommand => new Command(() =>
        {
            PieAxes = new ObservableCollection<IAxis>
            {
                new Category
                {
                    Orientation = AxisOrientation.Angular,
                    Title = new AxisTitle("hohoohohoho")
                    {
                        Margin = 10,
                        FontSize = 30,
                        FontColor = Colors.Red,
                        IsItalic = true,
                        IsBold = true,

                    },
                    Labels = new List<AxisLabel>
                    {
                        new AxisLabel { Content = "A" },
                        new AxisLabel { Content = "B" },
                        new AxisLabel { Content = "A" },
                        new AxisLabel { Content = "B" },
                        new AxisLabel { Content = "A" },
                        new AxisLabel { Content = "B" }
                    },
                    StrokeSize = 0,
                }
            };
        });
        public MainViewModel()
        {
            // Dữ liệu mẫu cho Pie
            PieData = new ObservableCollection<float> { 30, 20, 10, 45, 5, 5 };

            // Trục cho Pie (Angular Category)
            PieAxes = new ObservableCollection<IAxis>
            {
                new Category
                {
                    Orientation = AxisOrientation.Angular,
                    Title = new AxisTitle("Hehehehe")
                    {
                        Margin = 10,
                        FontSize = 30,
                        FontColor = Colors.Red,
                        IsItalic = true,
                        IsBold = true,

                    },
                    Labels = new List<AxisLabel>
                    {
                        new AxisLabel { Content = "A" },
                        new AxisLabel { Content = "B" },
                        new AxisLabel { Content = "A" },
                        new AxisLabel { Content = "B" },
                        new AxisLabel { Content = "A" },
                        new AxisLabel { Content = "B" }
                    },
                    StrokeSize = 0,
                }
            };

            LineAxes = new ObservableCollection<IAxis>
            {
                new MEGraph.MAUI.Axes.Line.Category
                {
                    Orientation = AxisOrientation.X,
                    Title = new AxisTitle("Category Axis")
                    {
                        Margin = 10,
                        FontSize = 20,
                        FontColor = Colors.Blue,
                        IsBold = true,
                    },
                    Labels = new List<AxisLabel>
                    {
                        new AxisLabel { Content = "Jan", FontSize=12, Margin=5 },
                        new AxisLabel { Content = "Feb", FontSize=12, Margin=5 },
                        new AxisLabel { Content = "Mar", FontSize=12, Margin=5 },
                        new AxisLabel { Content = "Apr", FontSize=12, Margin=5 },
                        new AxisLabel { Content = "May", FontSize=12, Margin=5 },
                        new AxisLabel { Content = "Jun", FontSize=12, Margin=5 },
                    },
                    StrokeSize = 1,
                },
                new MEGraph.MAUI.Axes.Line.Value
                {
                    Orientation = AxisOrientation.Y,
                    Title = new AxisTitle("Value Axis")
                    {
                        Margin = 10,
                        FontSize = 20,
                        FontColor = Colors.Green,
                        IsBold = true,
                    },
                    MinValue = 0,
                    MaxValue = 100,
                    TickInterval = 20,
                    StrokeSize = 1,
                }
            };
        }

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
    }
}