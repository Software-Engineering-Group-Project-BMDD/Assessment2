using System.Threading.Tasks;
using MauiApp1.Model;
using MauiApp1.UI.Model;
using MauiApp1.UI.ViewModel;


namespace MauiApp1.Test;

public class UnitTest1
{
     // test pickable struct
    [Fact]
    public void ValidatePickableStruct()
    {
        SensorStatusViewModel.sensorPickable testPick = new SensorStatusViewModel.sensorPickable("TestText");
        Assert.True(testPick.DisplayText == "TestText");
    }
    // test  sensor 
    [Fact]
    public void ValidateSensorModel()
    {
        Sensor sensor = new Sensor{ Sensor_Quantity = "Test Quantity"};

        Assert.True(sensor.Sensor_Quantity == "Test Quantity");
    }
    // test  reading
    [Fact]
    public void ValidateSensorReadingModel()
    {
        SensorReading reading = new SensorReading{Sensor_Quantity ="Test Quantity", sensor_value = 10f};
        Assert.True(reading.Sensor_Quantity=="Test Quantity" && reading.sensor_value == 10f);
    }
    [Fact]
    public void ValidateIncidentModel()
    {
        LoginIncedentModel incident = new LoginIncedentModel{User="user", incidentType="Failure", timeStamp=DateTime.Now.ToString("F")};
        Assert.True(incident.User == "user" && incident.incidentType == "Failure");
    }
    [Fact]
    public void ValidateUserModel()
    {
        UserModel user = new UserModel{UserName = "user", Password = "simplepass", UserType="type"};
        Assert.True(user.UserName=="user" && user.Password == "simplepass" && user.UserType == "type");
    }
}