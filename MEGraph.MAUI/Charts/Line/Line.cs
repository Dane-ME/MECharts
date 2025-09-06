using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Axes.Line;
using MEGraph.MAUI.Cores;
using MEGraph.MAUI.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BaseL = MEGraph.MAUI.Charts.Line.Base;
using LineL = MEGraph.MAUI.Charts.Line.Line;

namespace MEGraph.MAUI.Charts.Line
{
    public abstract class Line : BaseL
    {

        // === AXES PROPERTIES ===
        public Category XAxis { get; private set; }
        public Value YAxis { get; private set; }

        public Line()
        {
            XAxis = new Category();
            YAxis = new Value();

            Axes.Add(XAxis);
            Axes.Add(YAxis);
        }
        #region Support Bindable Data

        public static readonly BindableProperty DataProperty =
        BindableProperty.Create(
            nameof(Data),
            typeof(IEnumerable<float>),
            typeof(LineL),
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
            var chart = (LineL)bindable;
            chart.AttachDataChangedHandler(oldValue as INotifyCollectionChanged, newValue as INotifyCollectionChanged);

            if (newValue is IEnumerable<float> values)
            {
                chart.UpdateDataAndAxes(values);
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
                UpdateDataAndAxes(Data);
            }
        }

        private void UpdateDataAndAxes(IEnumerable<float> data)
        {
            var dataList = data.ToList();

            // Cập nhật series data
            if (Series.Any())
            {
                var lineSeries = Series.OfType<LineSeries>().FirstOrDefault();
                if (lineSeries != null)
                {
                    lineSeries.Data = dataList;
                }
            }

            // Cập nhật X-axis categories
            var categories = dataList.Select((_, index) => $"Point {index + 1}").ToArray();
            XAxis.SetCategories(categories);

            // Cập nhật Y-axis range
            if (dataList.Any())
            {
                float minValue = dataList.Min();
                float maxValue = dataList.Max();

                // Thêm padding 10% cho Y-axis
                float padding = (maxValue - minValue) * 0.1f;
                YAxis.SetValueRange(minValue - padding, maxValue + padding);
            }

            Refresh();
        }
        #endregion

        #region Support Bindable Series

        public static readonly BindableProperty SeriesItemsProperty =
            BindableProperty.Create(
            nameof(SeriesItems),
            typeof(ObservableCollection<LineSeries>),
            typeof(Line),
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
            var chart = (Line)bindable;

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
                chart.Series.Clear();
                chart.Series.Add(chart.CreateDefaultSeries());
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
            Series.Clear();
            foreach (var s in items)
            {
                Series.Add(s);
            }
        }

        #endregion

        #region Support Bindable Axes

        public static readonly BindableProperty ChartAxesProperty =
            BindableProperty.Create(
                nameof(ChartAxes),
                typeof(ObservableCollection<IAxis>),
                typeof(Line),
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
            var chart = (Line)bindable;

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
            Axes.Clear();
            foreach (var axis in chartAxes)
            {
                Axes.Add(axis);
            }
        }

        #endregion

        #region Public Methods

        public void SetData(IEnumerable<float> data)
        {
            Data = data;
        }

        public void SetData(IEnumerable<float> data, IEnumerable<string> categories)
        {
            Data = data;
            if (categories != null)
            {
                XAxis.SetCategories(categories.ToArray());
            }
        }

        public void SetData(IEnumerable<float> data, IEnumerable<string> categories, float minValue, float maxValue)
        {
            Data = data;
            if (categories != null)
            {
                XAxis.SetCategories(categories.ToArray());
            }
            YAxis.SetValueRange(minValue, maxValue);
        }

        public void AddSeries(LineSeries series)
        {
            Series.Add(series);
            Refresh();
        }

        public void RemoveSeries(LineSeries series)
        {
            Series.Remove(series);
            Refresh();
        }

        public void SetXAxisCategories(params string[] categories)
        {
            XAxis.SetCategories(categories);
        }

        public void SetYAxisRange(float min, float max)
        {
            YAxis.SetValueRange(min, max);
        }

        public void SetYAxisRange(float min, float max, int tickCount)
        {
            YAxis.SetValueRange(min, max, tickCount);
        }

        public void SetYAxisRange(float min, float max, float tickInterval)
        {
            YAxis.SetValueRange(min, max, tickInterval);
        }

        #endregion

        #region Abstract Methods

        protected abstract LineSeries CreateDefaultSeries();

        #endregion
    }
}
