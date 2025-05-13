using System.Linq;
using MauiApp1.MVVM.ViewModels;
using Xunit;

namespace MauiApp1.Tests
{
    public class AirQualityViewModelTests
    {
        [Fact]
        public void Constructor_ShouldLoadInitialData()
        {
            // Arrange & Act
            var vm = new AirQualityViewModel();

            // Assert
            Assert.NotNull(vm.AirQualityItems);
            Assert.Equal(2, vm.AirQualityItems.Count);
            Assert.Contains(vm.AirQualityItems, x => x.Date == "2025-05-11" && x.Time == "10:00");
            Assert.Contains(vm.AirQualityItems, x => x.Date == "2025-05-11" && x.Time == "11:00");
        }

        [Fact]
        public void RefreshCommand_ShouldUpdateAirQualityItemsAndIsRefreshing()
        {
            // Arrange
            var vm = new AirQualityViewModel();
            Assert.False(vm.IsRefreshing);

            // Act
            vm.RefreshCommand.Execute(null);

            // Assert
            Assert.False(vm.IsRefreshing);
            Assert.Equal(2, vm.AirQualityItems.Count);
            Assert.Contains(vm.AirQualityItems, x => x.Date == "2025-05-11" && x.Time == "12:00");
            Assert.Contains(vm.AirQualityItems, x => x.Date == "2025-05-11" && x.Time == "13:00");
        }

        [Fact]
        public void IsRefreshing_PropertyChanged_ShouldRaiseNotification()
        {
            // Arrange
            var vm = new AirQualityViewModel();
            bool propertyChangedRaised = false;
            vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(vm.IsRefreshing))
                    propertyChangedRaised = true;
            };

            // Act
            vm.IsRefreshing = !vm.IsRefreshing;

            // Assert
            Assert.True(propertyChangedRaised);
        }
    }
}
