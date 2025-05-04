using System.Threading.Tasks;
using MauiApp1.UI.Model;
using Microsoft.Maui.Platform;

namespace MauiApp1.Tests;

public class UnitTest1
{

    private SensorDatabase database = new SensorDatabase();

    [Fact]
    public void ValidateSensorModel()
    {
        var sensor = new Sensor
        {
            Type = "Test",
            Longitude = 5,
            Latitude = 10
        };

        Assert.True(sensor.Longitude == 5 && sensor.Latitude == 10);
    }

    [Fact]
    public async Task ValidateDatabasePersists()
    {
        await database.Populate();
        var sensors = await database.GetSensorsAsync();

        Assert.True(sensors.Count() > 0);
    }

    [Fact]
    public async Task ValidateBackup()
    {
        File.Delete(Constants.BackupPath);

        await database.Populate();
        await database.Backup();

        Assert.True(File.Exists(Constants.BackupPath));
    }

}