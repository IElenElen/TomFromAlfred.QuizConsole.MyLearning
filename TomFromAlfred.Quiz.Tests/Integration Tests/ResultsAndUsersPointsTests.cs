using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;

namespace TomFromAlfred.QuizConsole.Tests.Integration_Tests
{
    public class ResultsAndUsersPointsTests //testowanie współpracy menagera z serwisem dot. punktów i rezultatu
    {
        [Fact]
        public void VerifyAnswer_CorrectAnswer_ReturnsTrue()
        {
            // Arrange
            var answerVerifierService = new AnswerVerifierServiceApp();
            var manager = new ResultsAndUsersPointsManagerApp(answerVerifierService);

            // Act
            var result = manager.VerifyAnswer(0, 'b');

            // Assert
            Assert.True(result);
        }

        [Fact] //do poprawy
        public void DisplayFinalScore_AfterCorrectAnswer_ShouldIncrementTotalPoints()
        {

            //Arrange
            var answerVerifierService = new AnswerVerifierServiceApp();
            var manager = new ResultsAndUsersPointsManagerApp(answerVerifierService);

            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            //Act
            manager.VerifyAnswer(1, 'a');
            manager.DisplayFinalScore();

            //Assert
            Assert.Equal("Twój wynik końcowy: 1 pkt.\r\n", consoleOutput.ToString());
        }
    }
}
