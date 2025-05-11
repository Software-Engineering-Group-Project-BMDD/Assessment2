namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MauiApp1.MVVM.ViewModels.MainPageViewModel();
        }



       
    }
}
