using Microsoft.Data.Sqlite;
using Microsoft.Data.Sqlite;
using System.IO;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private readonly string _dbPath;

        public MainPage()
        {
            InitializeComponent();
            _dbPath = Path.Combine(FileSystem.AppDataDirectory, "Assessment2Db.db");
        }

        public Entry Username_Txtbox { get; set; }
        public Entry Password_Txtbox { get; set; }
        public Button Login_Btn { get; set; }

        // Method to get the text from the Username Entry
        private string GetUsername() => Username_Txtbox?.Text?.Trim() ?? string.Empty;

        // Method to get the text from the Password Entry
        private string GetPassword() => Password_Txtbox?.Text?.Trim() ?? string.Empty;

        #region Textbox Validation
        private void Username_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != null && System.Text.RegularExpressions.Regex.IsMatch(e.NewTextValue, @"[^a-zA-Z0-9]"))
            {
                DisplayAlert("Invalid Input", "Username cannot contain special characters.", "OK");
                Username_Txtbox.Text = e.OldTextValue; // Revert to the previous valid value
            }
        }

        private void Password_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != null && System.Text.RegularExpressions.Regex.IsMatch(e.NewTextValue, @"[^a-zA-Z0-9]"))
            {
                DisplayAlert("Invalid Input", "Password cannot contain special characters.", "OK");
                Password_Txtbox.Text = e.OldTextValue; // Revert to the previous valid value
            }
        }
        #endregion

        #region Input Validation
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

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please enter both username and password.", "OK");
                return;
            }

            if (!ValidateInput(username, password))
            {
                return;
            }

            try
            {
                bool userExists = await ValUsernameInDatabaseAsync(username, password);
                if (userExists)
                {
                    await DisplayAlert("Success", "User exists in the database.", "OK");
                    await Navigation.PushAsync(new ActivityPage());
                }
                else
                {
                    await DisplayAlert("Error", "User does not exist in the database.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        #region Database Operations
        private async Task<bool> ValUsernameInDatabaseAsync(string username, string password)
        {
            try
            {
                using var conn = new SqliteConnection($"Data Source={_dbPath};");
                await conn.OpenAsync();

                using var command = new SqliteCommand(
                    "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password", conn);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                var result = await command.ExecuteScalarAsync();
                return Convert.ToInt32(result) > 0;
            }
            catch
            {
                throw; // Let the caller handle the exception
            }
        }
        #endregion
    }
}

