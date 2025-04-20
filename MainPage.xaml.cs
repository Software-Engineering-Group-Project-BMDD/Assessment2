using Microsoft.Data.Sqlite;
using System.Data.SQLite;
using System.IO;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private readonly string _dbPath; // Declare the _dbPath field  

        public MainPage()
        {
            InitializeComponent();
            _dbPath = Path.Combine(FileSystem.AppDataDirectory, "Assessment2Db.db");
        }
        
        public Entry Username_Txtbox;

        public Entry Password_Txtbox;

        public Button Login_Btn;

        public Entry Password_Btn;

        // Method to get the text from the Username Entry  
        public string GetUsername()
        {
            return Username_Txtbox?.Text ?? string.Empty;
        }

        // Method to get the text from the Password Entry  
        public string GetPassword()
        {
            return Password_Txtbox?.Text ?? string.Empty;
        }

        #region Txtboxes
        private void Username_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string username = string.Empty; // Initialize the variable

            if (e.NewTextValue != null)
            {
                username = e.NewTextValue;
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(username, @"[^a-zA-Z0-9]"))
            {
                DisplayAlert("Invalid Input", "Username cannot contain special characters.", "OK");
                Username_Txtbox.Text = e.OldTextValue; // Revert to the previous valid value
            }
            else
            {
                username = string.Empty;
            }
        }

        public void Password_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string password = string.Empty; // Initialize the variable
            if (e.NewTextValue != null)
            {
                password = e.NewTextValue;
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[^a-zA-Z0-9]"))
            {
                DisplayAlert("Invalid Input", "Password cannot contain special characters.", "OK");
                Password_Txtbox.Text = e.OldTextValue; // Revert to the previous valid value
            }
            else
            {
                password = string.Empty;
            }
        } 
        #endregion

        #region Valdation
        private bool ValidateInput(string username, string password)
        {
            if (username.Length < 5 || username.Length > 20)
            {
                DisplayAlert("Invalid Input", "Username must be between 5 and 20 characters.", "OK");
                return false;
            }
            if (password.Length < 8)
            {
                DisplayAlert("Invalid Input", "Password must be at least 8 characters long.", "OK");
                return false;
            }
            return true;
        }
        #endregion


        public async void Login_Btn_Clicked(object sender, EventArgs e)
        {
            var username = GetUsername();
            var password = GetPassword();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Please enter both username and password.", "OK");
                return;
            }

            if (!ValidateInput(username, password))
            {
                return;
            }

            bool userExists = await ValUsernameInDatabaseAsync(username);
            if (userExists)
            {
                await DisplayAlert("Success", "User exists in the database.", "OK");
                // Navigate to ActivityPage.xaml
                await Navigation.PushAsync(new ActivityPage());
            }
            else
            {
                await DisplayAlert("Error", "User does not exist in the database.", "OK");
            }
        }

        // Method to check if username exists in the database  
        public async void CheckDatabaseConnection()
        {
            var dbChecker = new DatabaseConnectionChecker(Path.Combine(FileSystem.AppDataDirectory, "Assessment2Db.db"));
            bool isConnected = await dbChecker.CheckConnectionAsync();

            if (isConnected)
            {
                await DisplayAlert("Success", "Database connection is successful.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Failed to connect to the database.", "OK");
            }
        }

        #region Db checkInput
        public async Task<bool> ValUsernameInDatabaseAsync(string username)
        {
            // sets up try/catch for error handling  
            try
            {
                using var conn = new SqliteConnection($"Data Source={_dbPath};Version3;");
                await conn.OpenAsync();
                // creates a command to check if the username exists in the database  
                using var command = new SqliteCommand(
                    "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password",
                    conn);
                // adds the parameters to the command  
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", GetPassword());

                var result = await command.ExecuteScalarAsync();
                return Convert.ToInt32(result) > 0;
            }
            catch (Exception ex) // Correctly capture the exception object  
            {
                Console.WriteLine($"Database Error : {ex.Message}"); // Use the captured exception object  
                return false;
            }
        } 
        #endregion
    }
}
