using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using Moq;
using Xunit;

namespace MauiApp1.Tests
{
    public class DatabaseRepositoryTests
    {
        private readonly Mock<SqliteConnection> _mockConnection;
        private readonly Mock<SqliteCommand> _mockCommand;
        private readonly DatabaseRepository _repository;

        public DatabaseRepositoryTests()
        {
            _mockConnection = new Mock<SqliteConnection>();
            _mockCommand = new Mock<SqliteCommand>();
            _repository = new DatabaseRepository();

            // Mock DatabaseConnectionManager.GetConnection to return the mock connection
            DatabaseConnectionManager.GetConnection = () => _mockConnection.Object;

            // Setup mock connection to return the mock command
            _mockConnection.Setup(c => c.CreateCommand()).Returns(_mockCommand.Object);
        }

        [Fact]
        public void AddUser_ShouldExecuteCorrectSqlCommand()
        {
            // Arrange
            var parameters = new Dictionary<string, object>
            {
                { "@f_name", "John" },
                { "@l_name", "Doe" },
                { "@address", "123 Main St" },
                { "@role", 1 }
            };

            _mockCommand.Setup(c => c.ExecuteNonQuery()).Returns(1);
            _mockCommand.Setup(c => c.ExecuteScalar()).Returns(42); // Simulate last_insert_rowid()

            // Act
            var userId = _repository.AddUser("John", "Doe", "123 Main St", 1);

            // Assert
            Assert.Equal(42, userId);
            _mockCommand.VerifySet(c => c.CommandText = "INSERT INTO Users (F_Name, L_Name, Address, Role) VALUES (@f_name, @l_name, @address, @role)");
            foreach (var param in parameters)
            {
                _mockCommand.Verify(c => c.Parameters.AddWithValue(param.Key, param.Value), Times.Once);
            }
        }

        [Fact]
        public void GetUsersWithRoles_ShouldExecuteCorrectSqlCommand()
        {
            // Arrange
            var mockReader = new Mock<SqliteDataReader>();
            _mockCommand.Setup(c => c.ExecuteReader()).Returns(mockReader.Object);

            // Act
            var reader = _repository.GetUsersWithRoles();

            // Assert
            Assert.Equal(mockReader.Object, reader);
            _mockCommand.VerifySet(c => c.CommandText = @"SELECT u.User_id, u.F_Name, u.L_Name, u.Address, r.Role_name 
                                   FROM Users u 
                                   LEFT JOIN Role r ON u.Role = r.Role_Id");
        }
    }
}
