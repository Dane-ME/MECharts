using MEGraph.MAUI.Cores;
using MEGraph.MAUI.Series;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEGraph.MAUI.Charts
{
    public class LineChart : BaseChart
    {
        public LineSeries Series { get; private set; }

        public LineChart()
        {
            Series = new LineSeries();
            base.Series.Add(Series);
        }

        public void SetData(IEnumerable<float> data)
        {
            Series.Data = data.ToList();
            Refresh();
        }

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
    }

}
