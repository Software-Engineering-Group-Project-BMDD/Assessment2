using System.ComponentModel;
using System.Threading.Tasks;
using MauiApp1.MVVM.ViewModels;
using Microsoft.Maui.Controls;
using Moq;
using Xunit;

namespace MauiApp1.Tests
{
    public class MainPageViewModelTests
    {
        [Fact]
        public void Constructor_ShouldInitializeLoginCommand()
        {
            // Arrange & Act
            var vm = new MainPageViewModel();

            // Assert
            Assert.NotNull(vm.LoginCommand);
        }

        [Fact]
        public void Username_Setter_ShouldRaisePropertyChanged()
        {
            // Arrange
            var vm = new MainPageViewModel();
            bool raised = false;
            vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(vm.Username))
                    raised = true;
            };

            // Act
            vm.Username = "testuser";

            // Assert
            Assert.True(raised);
            Assert.Equal("testuser", vm.Username);
        }

        [Fact]
        public void Password_Setter_ShouldRaisePropertyChanged()
        {
            // Arrange
            var vm = new MainPageViewModel();
            bool raised = false;
            vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(vm.Password))
                    raised = true;
            };

            // Act
            vm.Password = "testpass123";

            // Assert
            Assert.True(raised);
            Assert.Equal("testpass123", vm.Password);
        }

        [Fact]
        public async Task LoginAsync_ShouldShowError_WhenUsernameOrPasswordIsEmpty()
        {
            // Arrange
            var vm = new MainPageViewModel();
            vm.Username = "";
            vm.Password = "";
            var shellMock = new MockShell();
            Shell.Current = shellMock;

            // Act
            await vm.LoginCommand.Execute(null);

            // Assert
            Assert.Equal("Error", shellMock.LastAlertTitle);
            Assert.Equal("Please enter both username and password.", shellMock.LastAlertMessage);
        }

        [Fact]
        public async Task LoginAsync_ShouldShowError_WhenInputIsInvalid()
        {
            // Arrange
            var vm = new MainPageViewModel();
            vm.Username = "abc";
            vm.Password = "123";
            var shellMock = new MockShell();
            Shell.Current = shellMock;

            // Act
            await vm.LoginCommand.Execute(null);

            // Assert
            Assert.Equal("Invalid Input", shellMock.LastAlertTitle);
        }

        // Helper mock shell for testing DisplayAlert and GoToAsync
        private class MockShell : Shell
        {
            public string LastAlertTitle { get; private set; }
            public string LastAlertMessage { get; private set; }
            public string LastAlertCancel { get; private set; }
            public string LastGoToRoute { get; private set; }

            public override Task<bool> DisplayAlert(string title, string message, string cancel)
            {
                LastAlertTitle = title;
                LastAlertMessage = message;
                LastAlertCancel = cancel;
                return Task.FromResult(true);
            }

            public override Task GoToAsync(string state, bool animate = true)
            {
                LastGoToRoute = state;
                return Task.CompletedTask;
            }
        }
    }
}

