using System;
using System.IO;
using Microsoft.Data.Sqlite;
using Moq;
using Xunit;

namespace MauiApp1.Tests
{
    public class DatabaseConnectionManagerTests
    {
        [Fact]
        public void GetConnection_ShouldReturnOpenConnection()
        {
            // Arrange
            var mockInitializer = new Mock<Action>();
            DatabaseInitializer.EnsureDatabaseExists = mockInitializer.Object;

            // Act
            var connection = DatabaseConnectionManager.GetConnection();

            // Assert
            Assert.NotNull(connection);
            Assert.Equal(ConnectionState.Open, connection.State);
            mockInitializer.Verify(i => i(), Times.Once);
        }

        [Fact]
        public void GetDatabasePath_ShouldReturnCorrectPath()
        {
            // Arrange
            var expectedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Assessment2Db.db");

            // Act
            var dbPath = DatabaseConnectionManager.GetDatabasePath();

            // Assert
            Assert.Equal(expectedPath, dbPath);
        }
    }
}
