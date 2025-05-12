using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Moq;
using Xunit;

namespace MauiApp1.Tests
{
    public class DatabaseConnectionCheckerTests
    {
        [Fact]
        public async Task CheckConnectionAsync_ShouldReturnTrue_WhenDatabaseExistsAndConnectionSucceeds()
        {
            // Arrange
            var dbPath = "test.db";
            var mockFile = new Mock<IFileSystem>();
            mockFile.Setup(f => f.Exists(dbPath)).Returns(true);

            var mockConnection = new Mock<SqliteConnection>($"Data Source={dbPath};Version=3;");
            mockConnection.Setup(c => c.OpenAsync(default)).Returns(Task.CompletedTask);

            var checker = new DatabaseConnectionChecker(dbPath);

            // Act
            var result = await checker.CheckConnectionAsync();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CheckConnectionAsync_ShouldReturnFalse_WhenDatabaseDoesNotExist()
        {
            // Arrange
            var dbPath = "nonexistent.db";
            var mockFile = new Mock<IFileSystem>();
            mockFile.Setup(f => f.Exists(dbPath)).Returns(false);

            var checker = new DatabaseConnectionChecker(dbPath);

            // Act
            var result = await checker.CheckConnectionAsync();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CheckConnectionAsync_ShouldReturnFalse_WhenConnectionFails()
        {
            // Arrange
            var dbPath = "test.db";
            var mockFile = new Mock<IFileSystem>();
            mockFile.Setup(f => f.Exists(dbPath)).Returns(true);

            var mockConnection = new Mock<SqliteConnection>($"Data Source={dbPath};Version=3;");
            mockConnection.Setup(c => c.OpenAsync(default)).ThrowsAsync(new Exception("Connection failed"));

            var checker = new DatabaseConnectionChecker(dbPath);

            // Act
            var result = await checker.CheckConnectionAsync();

            // Assert
            Assert.False(result);
        }
    }
}
