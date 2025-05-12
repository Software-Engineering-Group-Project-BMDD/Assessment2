using MauiApp1.UI.ViewModel;

namespace MauiApp1.UI.View;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel loginViewModel)
	{
		InitializeComponent();
		BindingContext = loginViewModel;
		loginViewModel.Init();
	}
}