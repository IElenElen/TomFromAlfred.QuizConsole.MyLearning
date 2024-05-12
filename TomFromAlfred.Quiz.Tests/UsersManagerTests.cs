using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;

namespace TomFromAlfred.Quiz.Tests
{
    public class UsersManagerTests
    {
        [Fact]
        public void GetUserChoice_ValidChoice_ReturnsChoice()
        {
            // Arrange
            var fakeInputReader = new FakeUserInputReader(new ConsoleKeyInfo('b', ConsoleKey.B, false, false, false));
            var usersManagerApp = new UsersManagerApp(fakeInputReader);

            // Act
            var result = usersManagerApp.GetUserChoice();

            // Assert
            Assert.Equal('b', result);
        }

        [Fact]
        public void GetUserChoice_InvalidChoice_ReturnsDefaultChoice() //problem z 2 testem
        {
            // Arrange
            var fakeInputReader1 = new FakeUserInputReader1('p'); // Symulujemy wprowadzenie nieprawidłowego wyboru
            var usersManagerApp = new UsersManagerApp(fakeInputReader1);

            // Act
            var result = usersManagerApp.GetUserChoice();

            // Assert
            Assert.Equal(default(char), result);

            // Debug
            Console.WriteLine("Test completed.");
        }
    }
}
