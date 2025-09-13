using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Cores;
using MEGraph.MAUI.Series.Line;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MEGraph.MAUI.Charts.Line
{
    public class StackedLineChart : BaseChart
    {
        public StackedLineSeries Series { get; private set; }
        public List<StackedLineSeries> SeriesList { get; private set; }

        public StackedLineChart()
        {
            SeriesList = new List<StackedLineSeries>();

            var defaultSeries = new StackedLineSeries();
            Series = defaultSeries;

            SeriesList.Add(defaultSeries);
            ((BaseChart)this).Series.Add(defaultSeries);

            SetRenderPipeline(new Cores.Pipeline.StackedLineRenderPipeline(this));
        }

        public void AddSeries(StackedLineSeries series)
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
            typeof(StackedLineChart),
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
            var chart = (StackedLineChart)bindable;
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
            typeof(ObservableCollection<StackedLineSeries>),
            typeof(StackedLineChart),
            default(ObservableCollection<StackedLineSeries>),
            propertyChanged: OnSeriesChanged
            );

        public ObservableCollection<StackedLineSeries> SeriesItems
        {
            get => (ObservableCollection<StackedLineSeries>)GetValue(SeriesItemsProperty);
            set => SetValue(SeriesItemsProperty, value);
        }

        private static void OnSeriesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chart = (StackedLineChart)bindable;

            if (oldValue is INotifyCollectionChanged oldCollection)
            {
                oldCollection.CollectionChanged -= chart.OnSeriesCollectionChanged;
            }

            if (newValue is INotifyCollectionChanged newCollection)
            {
                newCollection.CollectionChanged += chart.OnSeriesCollectionChanged;
                chart.SyncSeriesFromItems((ObservableCollection<StackedLineSeries>)newCollection);
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
            if (sender is ObservableCollection<StackedLineSeries> items)
            {
                SyncSeriesFromItems(items);
                Refresh();
            }
        }

        private void SyncSeriesFromItems(ObservableCollection<StackedLineSeries> items)
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
                typeof(StackedLineChart),
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
            var chart = (StackedLineChart)bindable;

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
            // END - 2.1.4 - ADD - Fix the issue where axes were lost when rendering multiple charts.
        }

        #endregion
    }
}
