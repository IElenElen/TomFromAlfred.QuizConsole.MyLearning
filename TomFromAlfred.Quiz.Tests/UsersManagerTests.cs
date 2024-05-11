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
            //Arrange
            var usersManagerApp = new UsersManagerApp();
            
            //Act
            //var result = usersManagerApp.GetUserChoice('b'); tu mi blokuje metodę Get
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Console.SetIn(new StringReader("b"));

                // Act
                var result = usersManagerApp.GetUserChoice();
                //Assert
                Assert.Equal('b', result);
            }
        }

        [Fact]
        public void GetUserChoice_InvalidChoice_ReturnsDefaultChoice()
        {
            //Arrange
            var usersManagerApp = new UsersManagerApp();

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Console.SetIn(new StringReader("p"));
            //Act
            var result = usersManagerApp.GetUserChoice();
            //Assert
            Assert.Equal(default(char), result);
            }
        }
    }
}
