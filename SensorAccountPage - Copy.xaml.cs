namespace MauiApp1;

public partial class SensorAccountPage : ContentPage
{
	public SensorAccountPage()
	{
		InitializeComponent();
	}

	private void BtnFirstSensorPage(object sender, EventArgs e)
	{
        // Navigate to the first sensor page
        Application.Current.MainPage = new NavigationPage(new FirstSensorPage());
    }

	private void BtnSecondSensorPage(object sender, EventArgs e)
	{
		Application.Current.MainPage = new NavigationPage(new SecondSensorPage());
    }

	private void BtnThirdSensorPage(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new ThirdSensorPage());
    }



}