using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract.AbstractForManager;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract.AbstractForService;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.EntityService;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.ServiceSupport;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;
using TomFromAlfred.QuizConsole.Tests.SupportForTests;
using Xunit.Abstractions;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Manager
{
    // Oblane: 0 / 8

    // Klasa testów dla Helpera managerskiego

    public class AdditionalTestsForTheManager
    {
        // 1 
        [Fact] // Zaliczony
        public void Constructor_ShouldThrowException_WhenQuestionsAreNull() // Konstruktor: podaje wyjątek, jeśli pytania = null
        {
            var act = () => new ManagerHelper(default!, Mock.Of<IQuizService>());

            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("questions");
        }

        // 2
        [Fact] // Zaliczony
        public void HasNext_ShouldReturnTrue_WhenQuestionsAreAvailable() // Czy ma następne? : zwraca prawdę, gdy są kolejne pytania
        {
            var helper = CreateHelper(new Question(1, "Q1"), new Question(2, "Q2"));

            helper.HasNext().Should().BeTrue();
        }

        // 3
        [Fact] // Zaliczony
        public void HasNext_ShouldReturnFalse_WhenNoMoreQuestions() // // Czy ma następne? : zwraca fałszę, gdy brak pytań
        {
            var helper = CreateHelper(new Question(1, "Q1"), new Question(2, "Q2"));
            helper.NextQuestion();

            helper.HasNext().Should().BeFalse();
        }

        // 4
        [Fact] // Zaliczony
        public void GetCurrentQuestion_ShouldReturnCurrentQuestion() // Podaje: bieżące pytanie
        {
            var helper = CreateHelper(new Question(1, "P1"), new Question(2, "P2"));

            helper.GetCurrentQuestion().QuestionId.Should().Be(1);
            helper.NextQuestion();
            helper.GetCurrentQuestion().QuestionId.Should().Be(2);
        }

        // 5
        [Fact] // Zaliczony
        public void GetCurrentQuestion_ShouldThrowException_WhenQuestionListIsEmpty() // Podaje pytanie: wyrzuca wyjątek jeśli lista pytań pusta
        {
            var helper = CreateHelper(); // pusty

            var act = () => helper.GetCurrentQuestion();

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*Brak kolejnych pytań*");
        }

        // 6
        [Fact] // Zaliczony
        public void NextQuestion_ShouldMoveToNextQuestion() // Następne: powinien przejść do kolejnego
        {
            var helper = CreateHelper(new Question(1, "P1"));

            var act = () => helper.NextQuestion();

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*brak kolejnych pytań*");
        }

        // 7
        [Fact] // Zaliczony
        public void Shuffle_ShouldRandomizeListOrder() // Losowanie: losuje pytania
        {
            var list = new List<int> { 1, 2, 3, 4, 5 };
            var shuffled = ManagerHelper.Shuffle(list);

            shuffled.Should().NotEqual(list)
                     .And.HaveCount(list.Count)
                     .And.Contain(list);
        }

        // 8
        [Fact] // Zaliczony
        public void ShuffleQuestions_ShouldChangeOrderOfQuestions() // Losowanie: (powinien) zmienia kolejność
        {
            var questions = new List<Question>
            {
                new Question(1, "Q1"), // Nie upraszczać
                new Question(2, "Q2"), // -II-
                new Question(3, "Q3") // - II -
            };

            var helper = new ManagerHelper(questions, Mock.Of<IQuizService>());
            var originalIds = questions.Select(q => q.QuestionId).ToList();

            helper.ShuffleQuestions();
            var shuffledIds = questions.Select(q => q.QuestionId).ToList();

            shuffledIds.Should().NotEqual(originalIds)
                        .And.HaveCount(originalIds.Count)
                        .And.Contain(originalIds);
        }

        private static ManagerHelper CreateHelper(params Question[] questions)
        {
            return new ManagerHelper(questions.ToList(), Mock.Of<IQuizService>()); // Nie upraszczać
        }
    }
}
