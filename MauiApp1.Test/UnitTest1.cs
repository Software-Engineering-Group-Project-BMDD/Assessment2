using System.Threading.Tasks;
using MauiApp1.Model;
using MauiApp1.UI.Model;
using MauiApp1.UI.ViewModel;


namespace MauiApp1.Test;

public class UnitTest1
{
    private SensorDatabase database = new SensorDatabase();

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
    // database related tests, there is an issue that doesnt let these work
    [Fact]
    public async Task TestSensorDOESExist()
    {
        bool exists = await database.DoesSensorExist("Nitrogen dioxide");
        Assert.True(exists);
    }
    [Fact]
    public async Task TestSensorDoesNOTExist()
    {
        bool exists = await database.DoesSensorExist("Nitrogen poxic");
        Assert.True(!exists);
    }
    // perform analyis
    [Fact]
    public async Task performAnalysisCorrect()
    {

        List<SensorReading> readings = new List<SensorReading>()
        {
            new SensorReading{sensor_value = 90},
            new SensorReading{sensor_value = 100},
            new SensorReading{sensor_value = 120},
            new SensorReading{sensor_value = 100},
            new SensorReading{sensor_value = 32},
            new SensorReading{sensor_value = 103},
            new SensorReading{sensor_value = 124},
            new SensorReading{sensor_value = 122},
            new SensorReading{sensor_value = 99}
        };
        TrendReportViewModel trvm = new TrendReportViewModel(database);
        TrendReportViewModel.Analysis analysis = trvm.performAnalysis(readings).Result;

        Assert.True(analysis.Highest == 124 && analysis.Lowest == 32);
    }
    // perform analyis
    [Fact]
    public async Task performAnalysisIncorrect()
    {

        List<SensorReading> readings = new List<SensorReading>()
        {
            new SensorReading{sensor_value = 90},
            new SensorReading{sensor_value = 100},
            new SensorReading{sensor_value = 120},
            new SensorReading{sensor_value = 100},
            new SensorReading{sensor_value = 32},
            new SensorReading{sensor_value = 103},
            new SensorReading{sensor_value = 124},
            new SensorReading{sensor_value = 122},
            new SensorReading{sensor_value = 99}
        };
        TrendReportViewModel trvm = new TrendReportViewModel(database);
        TrendReportViewModel.Analysis analysis = trvm.performAnalysis(readings).Result;

        Assert.True(analysis.Highest == 114 && analysis.Lowest == 332);
    }
}