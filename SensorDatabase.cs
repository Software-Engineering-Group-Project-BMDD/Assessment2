using System;
using MauiApp1.Model;
using MauiApp1.UI.Model;
using SQLite;

namespace MauiApp1;

public class SensorDatabase
{

    SQLiteAsyncConnection database;

    public async Task Populate()
    {
        //var sensors = await GetSensorsAsync();
        //if (sensors.Count() == 0)
        //{
        //    await SaveItemAsync(new Sensor { Type = "Weather", Latitude = 55.008785, Longitude = -3.5856323 });
        //    await SaveItemAsync(new Sensor { Type = "Air", Latitude = 55.94476, Longitude = -3.183991 });
        //    await SaveItemAsync(new Sensor { Type = "Water", Latitude = 55.8632306, Longitude = -3.2547047 });
        //}
        
        readSampleData readS = new readSampleData(this);

        // await DeleteAllSensorsAsync();
        await readS.initializeFullAirQuality();
        await readS.initializeFullWaterQuality();
        await readS.initializeFullWeatherData();

        // adds the base users
        await SaveUserAsync(new UserModel{UserName="EnScientist", Password="En1234",UserType="ES"});
        await SaveUserAsync(new UserModel{UserName="OpManager", Password="Op234",UserType="OM"});
        await SaveUserAsync(new UserModel{UserName="Administrator", Password="Ad1234",UserType="Ad"});

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
        var resultReadings = await database.CreateTableAsync<SensorReading>();
        var resultUsers = await database.CreateTableAsync<UserModel>();
        var resultincidents = await database.CreateTableAsync<LoginIncedentModel>();
    }

    public async Task<List<Sensor>> GetSensorsAsync()
    {
        await Init();
        return await database.Table<Sensor>().ToListAsync();
    }

    public async Task<bool> DoesSensorExist(string Quantity)
    {
        await Init();

        // gets all the sensors of the quantity
        List<Sensor> sensors = await database.Table<Sensor>().Where(s => s.Sensor_Quantity == Quantity).ToListAsync();
    
        // counts the amount
        int count = sensors.Count();
        if(count >0)
            return true;
        else
            return false;

    }

    public async Task<Sensor> GetSpecificSensor(string Quantity)
    {
        await Init();
        // just gets all the snesors of the quantity and gets the first one
        List<Sensor> sensors = await database.Table<Sensor>().Where(s => s.Sensor_Quantity == Quantity).ToListAsync();
        Sensor specificSensor = sensors.ElementAt(0);

        return specificSensor;
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
    public async Task<int> UpdateSensorDataAsync(Sensor sensor)
    {
        await Init();
      
        return await database.UpdateAsync(sensor);
        
    }

    public async Task<int> DeleteItemAsync(Sensor sensor)
    {
        await Init();
        return await database.DeleteAsync(sensor);
    }
    public async Task<int> DeleteAllSensorsAsync()
    {
        await Init();

        var sensors = await GetSensorsAsync();
        if (sensors.Count() > 0)
        {
            foreach (Sensor sen in sensors)
                await database.DeleteAsync(sen);
        }
        return 1;
    }
    public async Task<List<SensorReading>> GetSensorReadingsAsync()
    {
        await Init();
        return await database.Table<SensorReading>().ToListAsync();
    }
    public async Task<int> SaveReadingAsync(SensorReading sensorR)
    {
        await Init();
        // if sensor is not 0, then the autoincrement has worked, meaning it should already be somewhere in the database
        if (sensorR.ID != 0)
        {
            return await database.UpdateAsync(sensorR);
        }
        else
        {
            return await database.InsertAsync(sensorR);
        }
    }

    public async Task<bool> DoesSensorReadingExist(string rtimestamp, string Quantity)
    {
        await Init();

        // gets all the sensors of the quantity
        List<SensorReading> Readings = await database.Table<SensorReading>().Where(s => s.timestamp == rtimestamp && s.Sensor_Quantity == Quantity).ToListAsync();
    
        // counts the amount
        int count = Readings.Count();
        if(count >0)
            return true;
        else
            return false;

    }
    public async Task<List<SensorReading>> GetAllReadingsOfQuantity(string quantity)
    {
        // this was originally created for the trend report
        await Init();

        return await database.Table<SensorReading>().Where(s =>  s.Sensor_Quantity == quantity).ToListAsync();
    }
    public async Task<SensorReading> GetFinalSensorReadingAsync(string Quantity)
    {
        // 
        await Init();
        return await database.Table<SensorReading>().Where(s => s.Sensor_Quantity == Quantity)
                    .OrderByDescending(s => s.ID)
                    .FirstOrDefaultAsync();
    }
    public async Task<int> SaveUserAsync(UserModel user)
    {
        // saves a new user
        await Init();
        if (DoesUserExistAsync(user.UserName).Result)
        {
            return await database.UpdateAsync(user);
        }
        else
        {
            return await database.InsertAsync(user);
        }
    }
     public async Task<bool> DoesUserExistAsync(string UserName)
    {
        await Init();
        List<UserModel> users = await database.Table<UserModel>().Where(u => u.UserName == UserName).ToListAsync();
      
        // that means the user exists
        if (users.Count>0)
        return true;
        else
        return false;
    }
     public async Task<bool> DoesUserExistAsync(string UserName, string Password)
    {
        // returns a bool on if the user exists
        await Init();
        List<UserModel> users = await database.Table<UserModel>().Where(u => u.UserName == UserName && u.Password == Password).ToListAsync();
      
        // that means the user exists
        if (users.Count>0)
        return true;
        else
        return false;
    }
    public async Task<UserModel> GetUserAsync(string UserName, string Password)
    {
        // returns a bool on if the login is correct
        await Init();
        List<UserModel> users = await database.Table<UserModel>().Where(u => u.UserName == UserName && u.Password == Password).ToListAsync();
      
        // that means the user exists
        return users.ElementAt(0);
      

    }
    public async Task<List<UserModel>> GetAllUsers()
    {
        // returns all users
        await Init();
        return await database.Table<UserModel>().ToListAsync();
    }
    
}
