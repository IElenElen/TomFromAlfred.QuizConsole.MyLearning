using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;

namespace TomFromAlfred.Quiz.Tests
{
    public class AnswerVerifierTests
    {
        [Fact]
        public void GetPointsForAnswer_CorrectAnswer_ReturnsTrue() // czy metoda zwróci true dla poprawnej odpowiedzi?
        {
            // Arrange
            var answerVerifierServiceApp = new AnswerVerifierServiceApp();

            // Act
            bool result = answerVerifierServiceApp.GetPointsForAnswer(1, 'a');

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetPointsForAnswer_IncorrectAnswer_ReturnsFalse() // czy metoda zwróci false dla niepoprawnej odpowiedzi?
        {
            // Arrange
            var answerVerifierServiceApp = new AnswerVerifierServiceApp();

            // Act
            bool result = answerVerifierServiceApp.GetPointsForAnswer(1, 'B');

            // Assert
            Assert.False(result);
        }
    }
}
