using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Legends;
using MEGraph.MAUI.Series;
using MEGraph.MAUI.Styles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Cores
{
    public abstract class BaseChart : GraphicsView, IDisposable
    {
        private ChartDrawable _drawable;
        public List<ISeries> Series { get; } = new();
        public ObservableCollection<IAxis> Axes
        {
            get => (ObservableCollection<IAxis>)GetValue(AxesProperty);
            set => SetValue(AxesProperty, value);
        }
        public ILegend Legend
        {
            get => (ILegend)GetValue(LegendProperty);
            set => SetValue(LegendProperty, value);
        }
        //public ChartOptions Options { get; set; } = new ChartOptions();
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public BaseChart()
        {
            _drawable = new ChartDrawable(this);
            Drawable = _drawable;
            Unloaded += (s, e) => Dispose();

            Title = "Chart Title";
            Axes = new ObservableCollection<IAxis>
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
                        FontSize = 14,
                        FontColor = Colors.DarkRed
                    },
                    Orientation = AxisOrientation.X,
                    Labels = new List<AxisLabel>
                    {
                        new AxisLabel("Jan") { FontSize = 12, FontColor = Colors.Black },
                        new AxisLabel("Feb") { FontSize = 12, FontColor = Colors.Black },
                        new AxisLabel("Mar") { FontSize = 12, FontColor = Colors.Black },
                        new AxisLabel("Apr") { FontSize = 12, FontColor = Colors.Black }
                    }
                }
            };
        }

        public void Refresh() => this.Invalidate();

        public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(BaseChart),
        default(string),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((BaseChart)bindable).Refresh();
        });

        public static readonly BindableProperty LegendProperty =
        BindableProperty.Create(
        nameof(Legend),
        typeof(ILegend),
        typeof(BaseChart),
        default(ILegend),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((BaseChart)bindable).Refresh();
        });

        public static readonly BindableProperty AxesProperty =
        BindableProperty.Create(
        nameof(Axes),
        typeof(ObservableCollection<IAxis>),
        typeof(BaseChart),
        new ObservableCollection<IAxis>(),
        propertyChanged: OnAxesChanged);

        private static void OnAxesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chart = (BaseChart)bindable;

            if (oldValue is ObservableCollection<IAxis> oldAxes)
                oldAxes.CollectionChanged -= (_, __) => chart.Refresh();

            if (newValue is ObservableCollection<IAxis> newAxes)
                newAxes.CollectionChanged += (_, __) => chart.Refresh();

            chart.Refresh();
        }

        public void Dispose()
        {
            if (_drawable != null)
            {
                _drawable.Dispose();
                Drawable = null;
            }
        }
    }
}
