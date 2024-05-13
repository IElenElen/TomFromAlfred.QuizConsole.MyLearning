﻿using System;
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
        public void GetPointsForAnswer_CorrectAnswer_ReturnTrue()
        {
            //Arrange
            var answerVerifierServiceApp = new AnswerVerifierServiceApp();

            //Act
            bool result = answerVerifierServiceApp.GetPointsForAnswer(1, 'a');

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void GetPointsForAnswer_IncorrectAnswer_ReturnFalse()
        {

            //Arrange
            var answerVerifierServiceApp = new AnswerVerifierServiceApp();
            int questionNumber = 1; // Numer pytania
            char userChoice = 'B'; // Niepoprawna odpowiedź
            char correctAnswer = 'A'; // Poprawna odpowiedź zdefiniowana w słowniku //czy jest sens wprowadzać correct skoro mam metodę powyżej?

            //Act
            bool result = answerVerifierServiceApp.GetPointsForAnswer(questionNumber, userChoice);

            //Assert
            Assert.False(result);
        }
    }
}