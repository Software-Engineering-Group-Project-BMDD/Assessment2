using System.Globalization;
// using Microsoft.Data.SqlClient; // this is updated and newest package

using MauiApp1.UI.ViewModel;
using MauiApp1.UI.Model;

namespace MauiApp1.UI.View;


public partial class SensorStatus : ContentPage
{

 	public SensorStatus(SensorStatusViewModel SensorStatusViewModel)
	{
		InitializeComponent();
		BindingContext = SensorStatusViewModel;
		SensorStatusViewModel.Init();
	}
	
}