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
        public void GetUserChoice_ValidChoice_ReturnsChoice() //test 3xA z fake
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
        public void GetUserChoice_InvalidChoice_ShouldRepeatUntilValidChoice() //test 3xA z fake
        {
            // Arrange
            var fakeInputReader1 = new FakeUserInputReader2(new List<ConsoleKeyInfo> 
            {
                new ConsoleKeyInfo('p', ConsoleKey.P, false, false, false), // symulacja: wprowadzenie nieprawidłowego wyboru
                new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false) // teraz poprawny wybór
            });
            var usersManagerApp = new UsersManagerApp(fakeInputReader1);

            // Act
            var result = usersManagerApp.GetUserChoice();

            // Assert
            Assert.NotEqual(default(char), result);

            // Debug
            Console.WriteLine("Test completed.");
        }
    }
}
