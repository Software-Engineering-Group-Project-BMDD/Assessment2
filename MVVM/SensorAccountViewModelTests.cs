using System.ComponentModel;
using System.Threading.Tasks;
using MauiApp1.MVVM.ViewModels;
using Microsoft.Maui.Controls;
using Xunit;

namespace MauiApp1.Tests
{
    public class SensorAccountViewModelTests
    {
        [Fact]
        public void Constructor_InitializesCommandsAndStatus()
        {
            var vm = new SensorAccountViewModel();
            Assert.NotNull(vm.StartFirmwareUpdateCommand);
            Assert.NotNull(vm.NavigateCommand);
            Assert.Equal("Idle", vm.FirmwareUpdateStatus);
        }

        [Fact]
        public void PropertyChanged_Raised_For_FirmwareUpdateProgress()
        {
            var vm = new SensorAccountViewModel();
            bool raised = false;
            vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(vm.FirmwareUpdateProgress))
                    raised = true;
            };
            vm.FirmwareUpdateProgress = 0.5;
            Assert.True(raised);
        }

        [Fact]
        public void PropertyChanged_Raised_For_IsFirmwareUpdating()
        {
            var vm = new SensorAccountViewModel();
            bool raised = false;
            vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(vm.IsFirmwareUpdating))
                    raised = true;
            };
            vm.IsFirmwareUpdating = true;
            Assert.True(raised);
        }

        [Fact]
        public void PropertyChanged_Raised_For_FirmwareUpdateStatus()
        {
            var vm = new SensorAccountViewModel();
            bool raised = false;
            vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(vm.FirmwareUpdateStatus))
                    raised = true;
            };
            vm.FirmwareUpdateStatus = "Testing";
            Assert.True(raised);
        }

        [Fact]
        public async Task StartFirmwareUpdateCommand_UpdatesStatusAndProgress()
        {
            var vm = new SensorAccountViewModel();
            vm.FirmwareUpdateProgress = 0;
            vm.FirmwareUpdateStatus = "Idle";

            await Task.Run(() => vm.StartFirmwareUpdateCommand.Execute(null));

            Assert.False(vm.IsFirmwareUpdating);
            Assert.True(
                vm.FirmwareUpdateStatus == "Update completed successfully!" ||
                vm.FirmwareUpdateStatus == "Firmware is already up to date."
            );
            Assert.InRange(vm.FirmwareUpdateProgress, 0, 1);
        }

        [Fact]
        public async Task NavigateCommand_CallsShellGoToAsync_WhenPageNameIsNotNullOrEmpty()
        {
            var vm = new SensorAccountViewModel();
            string calledRoute = null;
            Shell.Current = new MockShell(route =>
            {
                calledRoute = route;
                return Task.CompletedTask;
            });

            string pageName = "TestPage";
            await Task.Run(() => vm.NavigateCommand.Execute(pageName));

            Assert.Equal(pageName, calledRoute);
        }

        [Fact]
        public async Task NavigateCommand_DoesNotCallShellGoToAsync_WhenPageNameIsNullOrEmpty()
        {
            var vm = new SensorAccountViewModel();
            bool called = false;
            Shell.Current = new MockShell(route =>
            {
                called = true;
                return Task.CompletedTask;
            });

            await Task.Run(() => vm.NavigateCommand.Execute(""));
            Assert.False(called);
        }

        // Helper mock shell for navigation
        private class MockShell : Shell
        {
            private readonly Func<string, Task> _goToAsync;
            public MockShell(Func<string, Task> goToAsync) => _goToAsync = goToAsync;
            public override Task GoToAsync(string state, bool animate = true) => _goToAsync(state);
        }
    }
}
