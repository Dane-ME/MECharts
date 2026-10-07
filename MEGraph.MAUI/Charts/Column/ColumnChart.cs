using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Cores;
using MEGraph.MAUI.Cores.Pipeline;
using MEGraph.MAUI.Series.Column;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace MEGraph.MAUI.Charts.Column
{
    public class ColumnChart : BaseChart
    {
        public ColumnSeries Series { get; private set; }
        public List<ColumnSeries> SeriesList { get; private set; }

        public ColumnChart()
        {
            // Thiết lập Pipeline cho biểu đồ cột
            SetRenderPipeline(new ColumnRenderPipeline(this));

            SeriesList = new List<ColumnSeries>();

            var defaultSeries = new ColumnSeries();
            Series = defaultSeries;

            SeriesList.Add(defaultSeries);
            ((BaseChart)this).Series.Add(defaultSeries);
        }

        public void AddSeries(ColumnSeries series)
        {
            SeriesList.Add(series);
            ((BaseChart)this).Series.Add(series);
            Refresh();
        }

        public void SetData(IEnumerable<float> data)
        {
            Series.Data = data.ToList();
            PlayEntryAnimation();
        }

        #region Support Bindable Data

        public static readonly BindableProperty DataProperty =
            BindableProperty.Create(
                nameof(Data),
                typeof(IEnumerable<float>),
                typeof(ColumnChart),
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
            var chart = (ColumnChart)bindable;
            chart.AttachDataChangedHandler(oldValue as INotifyCollectionChanged, newValue as INotifyCollectionChanged);

            if (newValue is IEnumerable<float> values)
            {
                chart.Series.Data = values.ToList();
                chart.PlayEntryAnimation();
            }
        }

        private void AttachDataChangedHandler(INotifyCollectionChanged? oldData, INotifyCollectionChanged? newData)
        {
            if (oldData != null)
                oldData.CollectionChanged -= OnCollectionChanged;

            if (newData != null)
                newData.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (Data != null)
            {
                Series.Data = Data.ToList();
                PlayEntryAnimation();
            }
        }

        #endregion

        #region Support Bindable Series

        public static readonly BindableProperty SeriesItemsProperty =
            BindableProperty.Create(
                nameof(SeriesItems),
                typeof(ObservableCollection<ColumnSeries>),
                typeof(ColumnChart),
                default(ObservableCollection<ColumnSeries>),
                propertyChanged: OnSeriesItemsChanged
            );

        public ObservableCollection<ColumnSeries> SeriesItems
        {
            get => (ObservableCollection<ColumnSeries>)GetValue(SeriesItemsProperty);
            set => SetValue(SeriesItemsProperty, value);
        }

        private static void OnSeriesItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chart = (ColumnChart)bindable;

            if (oldValue is ObservableCollection<ColumnSeries> oldSeries)
                oldSeries.CollectionChanged -= chart.OnSeriesCollectionChanged;

            if (newValue is ObservableCollection<ColumnSeries> newSeries)
            {
                newSeries.CollectionChanged += chart.OnSeriesCollectionChanged;
                chart.SyncSeriesFromSeriesItems(newSeries);
            }

            chart.Refresh();
        }

        private void OnSeriesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is ObservableCollection<ColumnSeries> items)
            {
                SyncSeriesFromSeriesItems(items);
                Refresh();
            }
        }

        private void SyncSeriesFromSeriesItems(ObservableCollection<ColumnSeries> items)
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
                typeof(ColumnChart),
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
            var chart = (ColumnChart)bindable;

            if (oldValue is ObservableCollection<IAxis> oldAxes)
            {
                oldAxes.CollectionChanged -= chart.OnAxesCollectionChanged;
            }

            if (newValue is ObservableCollection<IAxis> newAxes)
            {
                newAxes.CollectionChanged += chart.OnAxesCollectionChanged;
                chart.SyncAxesFromChartAxes(newAxes);
            }

            chart.Refresh();
        }

        private void OnAxesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is ObservableCollection<IAxis> axes)
            {
                SyncAxesFromChartAxes(axes);
                Refresh();
            }
        }

        private void SyncAxesFromChartAxes(ObservableCollection<IAxis> chartAxes)
        {
            if (Manager.TryGetChart(this.Id, out _))
            {
                for (int i = Axes.Count - 1; i >= 0; i--)
                {
                    if (Axes[i].ChartId == this.Id)
                        Axes.RemoveAt(i);
                }
                foreach (var axis in chartAxes)
                {
                    axis.ChartId = this.Id;
                    Axes.Add(axis);
                }
            }
        }

        #endregion
    }
}
