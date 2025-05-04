using MauiApp1.UI.ViewModel;

namespace MauiApp1.UI.View;

public partial class SensorView : ContentPage
{
	public SensorView(SensorViewModel sensorViewModel)
	{
		InitializeComponent();
		BindingContext = sensorViewModel;
		sensorViewModel.Init();
	}
}