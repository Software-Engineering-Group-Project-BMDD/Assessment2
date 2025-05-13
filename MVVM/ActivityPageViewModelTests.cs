using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using MauiApp1.MVVM.ViewModels;
using Moq;
using Xunit;

namespace MauiApp1.Tests
{
    public class ActivityPageViewModelTests
    {
        [Fact]
        public void Constructor_ShouldInitializeNavigateCommand()
        {
            // Arrange & Act
            var vm = new ActivityPageViewModel();

            // Assert
            Assert.NotNull(vm.NavigateCommand);
        }

        [Fact]
        public async Task NavigateCommand_ShouldCallShellGoToAsync_WhenPageNameIsNotNullOrEmpty()
        {
            // Arrange
            var vm = new ActivityPageViewModel();
            var mockShell = new Mock<IShellNavigation>();
            var pageName = "TestPage";

            // Replace Shell.Current with a mock (requires abstraction in production code)
            // For demonstration, we simulate the command execution
            bool navigationCalled = false;
            Shell.Current = new MockShell((route) =>
            {
                navigationCalled = route == pageName;
                return Task.CompletedTask;
            });

            // Act
            if (vm.NavigateCommand.CanExecute(pageName))
                vm.NavigateCommand.Execute(pageName);

            // Assert
            Assert.True(navigationCalled);
        }

        [Fact]
        public async Task NavigateCommand_ShouldNotCallShellGoToAsync_WhenPageNameIsNullOrEmpty()
        {
            // Arrange
            var vm = new ActivityPageViewModel();
            var mockShell = new Mock<IShellNavigation>();
            string pageName = "";

            bool navigationCalled = false;
            Shell.Current = new MockShell((route) =>
            {
                navigationCalled = true;
                return Task.CompletedTask;
            });

            // Act
            if (vm.NavigateCommand.CanExecute(pageName))
                vm.NavigateCommand.Execute(pageName);

            // Assert
            Assert.False(navigationCalled);
        }

        // Helper mock shell for testing navigation
        private class MockShell : Shell
        {
            private readonly Func<string, Task> _goToAsync;

            public MockShell(Func<string, Task> goToAsync)
            {
                _goToAsync = goToAsync;
            }

            public override Task GoToAsync(string state, bool animate = true)
            {
                return _goToAsync(state);
            }
        }
    }
}
