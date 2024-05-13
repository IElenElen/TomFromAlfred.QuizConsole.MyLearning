using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.Tests.Tests_Services
{
    public class QuestionServiceTests
    {
        //GetQuestionByNumber metoda do testu
        [Fact]
        public void GetQuestionByNumber_CorrectNumber_ReturnTrue() //test oblany
        {
            //Arrange
            var questionServiceApp = new QuestionServiceApp();
            var allQuestions = new List<Question>();
            {
                new Question(1, "Pytanie 1");
                new Question(2, "Pytanie 2");
                new Question(3, "Pytanie 3");
             }
            int questionNumber = 2;

            //Act
            bool result = questionServiceApp.GetQuestionByNumber(allQuestions, questionNumber);

            //Assert
            Assert.True(result);
        }
    }
}
