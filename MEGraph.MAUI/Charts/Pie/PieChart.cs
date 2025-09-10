using MEGraph.MAUI.Axes;
using MEGraph.MAUI.Cores;
using MEGraph.MAUI.Series.Pie;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Charts.Pie
{
    public class PieChart : BaseChart
    {
        public PieSeries Series { get; private set; }
        public PieChart()
        {
            this.Series = new PieSeries();
            ((BaseChart)this).Series.Add(Series);

            SetRenderPipeline(new Cores.Pipeline.PieRenderPipeline(this));
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
            typeof(PieChart),
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
            var chart = (PieChart)bindable;
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

        #region Support Bindable Axes

        public static readonly BindableProperty ChartAxesProperty =
            BindableProperty.Create(
                nameof(ChartAxes),
                typeof(ObservableCollection<IAxis>),
                typeof(PieChart),
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
            var chart = (PieChart)bindable;

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
            Axes.Clear();
            foreach (var axis in chartAxes)
            {
                Axes.Add(axis);
            }
        }

        #endregion
    }
}
