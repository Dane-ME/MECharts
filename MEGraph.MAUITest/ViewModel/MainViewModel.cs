using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Axes.Pie;
using MEGraph.MAUI.Series.Pie;
using MEGraph.MAUI.Styles;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MEGraph.MAUITest.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<float> _pieData;
        private ObservableCollection<IAxis> _pieAxes;

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