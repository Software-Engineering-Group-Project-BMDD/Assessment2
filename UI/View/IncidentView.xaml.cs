using MauiApp1.UI.ViewModel;

namespace MauiApp1.UI.View;

public partial class IncidentView : ContentPage
{
	public IncidentView(IncidentViewModel incidentViewModel)
	{
		InitializeComponent();

		BindingContext = incidentViewModel;
		incidentViewModel.Init();
	}
}