using MauiApp1.UI.ViewModel;

namespace MauiApp1.UI.View;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}