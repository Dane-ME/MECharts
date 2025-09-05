namespace MEGraph.MAUITest
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            var data = new List<float> { 10, 30, 15, 50, 40, 70, 20 };
            lineChart.SetData(data);
        }
    }

}
