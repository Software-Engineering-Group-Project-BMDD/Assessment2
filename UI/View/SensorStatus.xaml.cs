using MauiApp1.UI.ViewModel;
namespace MauiApp1.UI.View;


public partial class SensorStatus : ContentPage
{

 	public SensorStatus(SensorStatusViewModel sensorStatusViewModel)
	{
		InitializeComponent();
		BindingContext = sensorStatusViewModel;
		sensorStatusViewModel.Init();
	}
	
}