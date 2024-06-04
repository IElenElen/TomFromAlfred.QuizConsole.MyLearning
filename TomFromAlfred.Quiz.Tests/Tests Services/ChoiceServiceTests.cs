using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.Tests.Tests_Services
{
    public class ChoiceServiceTests
    {
        [Fact]
        public void AddChoice_ShouldAddNewChoice() //test 3xA //test i testowana klasa do analizy!!!
        {
            // Arrange
            var choiceService = new ChoiceServiceApp();
            var newChoice = new Choice(9, 'a', "Nowy wybór");

            // Act
            //choiceService.Choice9A = newChoice; // Ustaw nowy wybór

            // Assert
            //Assert.Equal(newChoice, choiceService.Choice9A);
        }
    }
}
