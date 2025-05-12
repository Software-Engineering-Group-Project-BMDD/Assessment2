using MauiApp1.UI.ViewModel;

namespace MauiApp1.UI.View;

public partial class TrendReportView : ContentPage
{

	public TrendReportView(TrendReportViewModel trendReportViewModel)
	{

		InitializeComponent();
		BindingContext = trendReportViewModel;
		trendReportViewModel.Init();
	}
}