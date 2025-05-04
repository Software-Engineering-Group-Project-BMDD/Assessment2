using MauiApp1.UI.ViewModel;

namespace MauiApp1.UI.View;

public partial class MapView : ContentPage
{
	public MapView(MapViewModel mapViewModel)
	{
		InitializeComponent();
		BindingContext = mapViewModel;
		mapViewModel.Init();
	}
}