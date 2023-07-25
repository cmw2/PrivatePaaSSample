using TestConnectionsApp.Util;
using Xunit;

namespace TestConnectionsApp.Tests
{
    public class MaskPasswordTests
    {
        [Fact]
        public void Mask_ReplacesPasswordWithAsterisks()
        {
            // Arrange
            var connectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";
            var expected = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=********;";

            // Act
            var actual = MaskPassword.Mask(connectionString);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Mask_UsesCustomSeparators()
        {
            // Arrange
            var connectionString = "Server=myServerAddress|Database=myDataBase|User Id=myUsername|Password=myPassword|";
            var firstSeparator = "|";
            var secondSeparator = "=";
            var expected = "Server=myServerAddress|Database=myDataBase|User Id=myUsername|Password=********|";

            // Act
            var actual = MaskPassword.Mask(connectionString, firstSeparator, secondSeparator);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Mask_DoesNotReplacePasswordIfKeyIsNotPassword()
        {
            // Arrange
            var connectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Pwd=myPassword;";
            var expected = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Pwd=myPassword;";

            // Act
            var actual = MaskPassword.Mask(connectionString);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Mask_ReturnsEmptyStringIfConnectionStringIsNull()
        {
            // Arrange
            string connectionString = null;
            var expected = "";

            // Act
            var actual = MaskPassword.Mask(connectionString);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}