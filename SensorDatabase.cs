using System;
using MauiApp1.UI.Model;
using SQLite;

namespace MauiApp1;

public class SensorDatabase
{

    SQLiteAsyncConnection database;

    public async Task Populate()
    {
        var sensors = await GetSensorsAsync();
        if (sensors.Count() > 0)
        {
            return;
        }
        //readSampleData readsD = new readSampleData();

        //readsD.initializeFullAirQuality();
        await SaveItemAsync(new Sensor { Type = "Weather", Latitude = 55.008785, Longitude = -3.5856323 });
        await SaveItemAsync(new Sensor { Type = "Air", Latitude = 55.94476, Longitude = -3.183991 });
        await SaveItemAsync(new Sensor { Type = "Water", Latitude = 55.8632306, Longitude = -3.2547047 });
    }

    public async Task Backup()
    {
        try
        {
            if (File.Exists(Constants.DatabasePath))
            {
                File.Copy(Constants.DatabasePath, Constants.BackupPath, overwrite: true);
            }
            else
            {
                Console.WriteLine("Database file does not exist");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while backing up the database: " + ex.Message);
        }
    }

    async Task Init()
    {
        if (database is not null)
            return;

        database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        var result = await database.CreateTableAsync<Sensor>();
    }

    public async Task<List<Sensor>> GetSensorsAsync()
    {
        await Init();
        return await database.Table<Sensor>().ToListAsync();
    }


    public async Task<int> SaveItemAsync(Sensor sensor)
    {
        await Init();
        if (sensor.ID != 0)
        {
            return await database.UpdateAsync(sensor);
        }
        else
        {
            return await database.InsertAsync(sensor);
        }
    }

    public async Task<int> DeleteItemAsync(Sensor sensor)
    {
        await Init();
        return await database.DeleteAsync(sensor);
    }

}
