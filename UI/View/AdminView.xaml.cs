using MauiApp1.UI.ViewModel;

namespace MauiApp1.UI.View;

public partial class AdminView : ContentPage
{
	public AdminView(AdminViewModel adminViewModel)
	{
		InitializeComponent();
		BindingContext = adminViewModel;

		MessagingCenter.Subscribe<AdminViewModel, string>(this, "ShowPopup", async (sender, message) =>
		{
			await DisplayAlert("Backup", message, "OK");
		});
	}
}