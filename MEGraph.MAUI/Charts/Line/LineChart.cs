using MEGraph.MAUI.Cores;
using MEGraph.MAUI.Series;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Series.Line;


namespace MEGraph.MAUI.Charts.Line
{
    public class LineChart : BaseChart
    {
        public LineSeries Series { get; private set; }
        public List<LineSeries> SeriesList { get; private set; }

        public LineChart()
        {
            SeriesList = new List<LineSeries>();

            var defaultSeries = new LineSeries();
            Series = defaultSeries;

            SeriesList.Add(defaultSeries);
            ((BaseChart)this).Series.Add(defaultSeries);
        }

        public void AddSeries(LineSeries series)
        {
            SeriesList.Add(series);
            ((BaseChart)this).Series.Add(series);
            Refresh();
        }

        public void SetData(IEnumerable<float> data)
        {
            Series.Data = data.ToList();
            Refresh();
        }

        #region Support Bindable Data

        public static readonly BindableProperty DataProperty =
        BindableProperty.Create(
            nameof(Data),
            typeof(IEnumerable<float>),
            typeof(LineChart),
            default(IEnumerable<float>),
            propertyChanged: OnDataChanged
            );

        public IEnumerable<float> Data
        {
            get => (IEnumerable<float>)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        private static void OnDataChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chart = (LineChart)bindable;
            chart.AttachDataChangedHandler(oldValue as INotifyCollectionChanged, newValue as INotifyCollectionChanged);

            if (newValue is IEnumerable<float> values)
            {
                chart.Series.Data = values.ToList();
                chart.Refresh();
            }
        }

        private void AttachDataChangedHandler(INotifyCollectionChanged oldData, INotifyCollectionChanged newData)
        {
            if (oldData != null)
                oldData.CollectionChanged -= OnCollectionChanged;

            if (newData != null)
                newData.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Data != null)
            {
                Series.Data = Data.ToList();
                Refresh();
            }
        }
        #endregion

        #region Support Bindable Series

        public static readonly BindableProperty SeriesItemsProperty =
            BindableProperty.Create(
            nameof(SeriesItems),
            typeof(ObservableCollection<LineSeries>),
            typeof(LineChart),
            default(ObservableCollection<LineSeries>),
            propertyChanged: OnSeriesChanged
            );

        public ObservableCollection<LineSeries> SeriesItems
        {
            get => (ObservableCollection<LineSeries>)GetValue(SeriesItemsProperty);
            set => SetValue(SeriesItemsProperty, value);
        }

        private static void OnSeriesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chart = (LineChart)bindable;

            if (oldValue is INotifyCollectionChanged oldCollection)
            {
                oldCollection.CollectionChanged -= chart.OnSeriesCollectionChanged;
            }

            if (newValue is INotifyCollectionChanged newCollection)
            {
                newCollection.CollectionChanged += chart.OnSeriesCollectionChanged;
                chart.SyncSeriesFromItems((ObservableCollection<LineSeries>)newCollection);
            }
            else
            {
                // Reset về default series khi newValue là null
                chart.SeriesList.Clear();
                chart.SeriesList.Add(chart.Series);
                ((BaseChart)chart).Series.Clear();
                ((BaseChart)chart).Series.Add(chart.Series);
            }

            chart.Refresh();
        }

        private void OnSeriesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is ObservableCollection<LineSeries> items)
            {
                SyncSeriesFromItems(items);
                Refresh();
            }
        }

        private void SyncSeriesFromItems(ObservableCollection<LineSeries> items)
        {
            SeriesList.Clear();
            ((BaseChart)this).Series.Clear();

            foreach (var s in items)
            {
                SeriesList.Add(s);
                ((BaseChart)this).Series.Add(s);
            }

            if (SeriesList.Count > 0)
            {
                Series = SeriesList[0];
            }
        }
        #endregion

        #region Support Bindable Axes

        public static readonly BindableProperty ChartAxesProperty =
            BindableProperty.Create(
                nameof(ChartAxes),
                typeof(ObservableCollection<IAxis>),
                typeof(LineChart),
                default(ObservableCollection<IAxis>),
                propertyChanged: OnChartAxesChanged
            );

        public ObservableCollection<IAxis> ChartAxes
        {
            get => (ObservableCollection<IAxis>)GetValue(ChartAxesProperty);
            set => SetValue(ChartAxesProperty, value);
        }

        private static void OnChartAxesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chart = (LineChart)bindable;

            if (oldValue is ObservableCollection<IAxis> oldAxes)
            {
                oldAxes.CollectionChanged -= chart.OnAxesCollectionChanged;
            }

            if (newValue is ObservableCollection<IAxis> newAxes)
            {
                newAxes.CollectionChanged += chart.OnAxesCollectionChanged;
                // Sync với BaseChart.Axes
                chart.SyncAxesFromChartAxes(newAxes);
            }

            chart.Refresh();
        }

        private void OnAxesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is ObservableCollection<IAxis> axes)
            {
                SyncAxesFromChartAxes(axes);
                Refresh();
            }
        }

        private void SyncAxesFromChartAxes(ObservableCollection<IAxis> chartAxes)
        {
            // START - 2.1.4 - EDIT - Fix the issue where axes were lost when rendering multiple charts.
            if (Manager.GetBCManager().ContainsKey(this.Id))
            {
                for (int i = Axes.Count - 1; i >= 0; i--)
                {
                    if (Axes[i].ChartId == this.Id)
                    {
                        Axes.RemoveAt(i);
                    }
                }
                foreach (var axis in chartAxes)
                {
                    axis.ChartId = this.Id;
                    Axes.Add(axis);
                }
            }
            // END - 2.1.4 - EDIT - Fix the issue where axes were lost when rendering multiple charts.
        }
        #endregion
    }

}
