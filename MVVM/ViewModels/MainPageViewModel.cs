using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage; // For FileSystem
using Microsoft.Data.Sqlite; // For SQLite database operations

namespace MauiApp1.MVVM.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private string _username = string.Empty; // Initialize to avoid CS8618
        private string _password = string.Empty; // Initialize to avoid CS8618
        private readonly string _dbPath;

        public MainPageViewModel()
        {
            _dbPath = Path.Combine(FileSystem.AppDataDirectory, "Assessment2Db.db");
            LoginCommand = new Command(async () => await LoginAsync());
        }

        // Properties for data binding
        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        // Command for the Login button
        public ICommand LoginCommand { get; }

        // Login logic
        private async Task LoginAsync()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                await Shell.Current.DisplayAlert("Error", "Please enter both username and password.", "OK");
                return;
            }

            if (!ValidateInput(Username, Password))
            {
                return;
            }

            bool userExists = await ValUsernameInDatabaseAsync(Username);
            if (userExists)
            {
                await Shell.Current.DisplayAlert("Success", "User exists in the database.", "OK");
                // Navigate to ActivityPage.xaml
                await Shell.Current.GoToAsync("//ActivityPage");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "User does not exist in the database.", "OK");
            }
        }

        // Validation logic
        private bool ValidateInput(string username, string password)
        {
            if (username.Length < 5 || username.Length > 20)
            {
                Shell.Current.DisplayAlert("Invalid Input", "Username must be between 5 and 20 characters.", "OK");
                return false;
            }
            if (password.Length < 8)
            {
                Shell.Current.DisplayAlert("Invalid Input", "Password must be at least 8 characters long.", "OK");
                return false;
            }
            return true;
        }

        // Database validation
        private async Task<bool> ValUsernameInDatabaseAsync(string username)
        {
            try
            {
                using var conn = new SqliteConnection($"Data Source={_dbPath};Version=3;");
                await conn.OpenAsync();
                using var command = new SqliteCommand(
                    "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password",
                    conn);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", Password);

                var result = await command.ExecuteScalarAsync();
                return Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
                return false;
            }
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler? PropertyChanged; // Nullable to match the interface

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
