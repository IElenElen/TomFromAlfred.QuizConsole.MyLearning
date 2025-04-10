﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.EntityService;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.ServiceSupport;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.QuizConsole.Tests.SupportForTests
{
    // Na potrzeby testu

    class MockQuizService : QuizService
    {
        public MockQuizService(                      // Nie upraszczać
            QuestionService questionService,
            ChoiceService choiceService,
            CorrectAnswerService correctAnswerService,
            JsonCommonClass jsonService,
            IFileWrapper fileWrapper)
            : base(questionService, choiceService, correctAnswerService, jsonService, fileWrapper)
        {
        }

        // Zatrzymuję ładowanie plików
        public override void LoadQuestionsFromJson(string filePath) { }
        public override void LoadChoicesFromJson() { }
        public override void LoadCorrectSetFromJson() { }

        // Pozwalam wstrzyknąć dane testowe
        public void SetTestData(
            List<Question> questions,
            Dictionary<int, List<Choice>> choices,
            Dictionary<int, string> correctAnswers)
        {
            this._jsonQuestions = questions;
            this._jsonChoices = choices;
            this._correctAnswers = correctAnswers;
        }
    }
}
