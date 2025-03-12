using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.ServiceSupport;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.QuizConsole.Tests.SupportForTests
{
    public class FakeQuizService : QuizService
    {
        private List<Question> _testQuestions = new();
        private Dictionary<int, List<Choice>> _testChoices = new();
        private Dictionary<int, string> _testCorrectAnswers = new();

        public FakeQuizService()
            : base(new Mock<QuestionService>().Object,
                   new Mock<ChoiceService>().Object,
                   new Mock<CorrectAnswerService>().Object,
                   new Mock<JsonCommonClass>().Object,
                   new Mock<IFileWrapper>().Object)
        { }

        public void SetTestData(List<Question> questions, Dictionary<int, List<Choice>> choices, Dictionary<int, string> correctAnswers)
        {
            _testQuestions = questions;
            _testChoices = choices;
            _testCorrectAnswers = correctAnswers;
        }

        // Nadpisanie GetAllQuestions() w QuizService
        public override List<Question> GetAllQuestions()
        {
            return _testQuestions;
        }

        public override List<Choice> GetShuffledChoicesForQuestion(int questionId, out Dictionary<char, char> letterMapping)
        {
            letterMapping = new Dictionary<char, char> { { 'A', 'A' }, { 'B', 'B' }, { 'C', 'C' } };

            return new List<Choice>
            {
                new Choice(1, 'A', "Tomek Wilmowski"),
                new Choice(1, 'B', "Staś Tarkowski"),
                new Choice(1, 'C', "Michał Wołodyjowski")
            };
        }

        public override bool CheckAnswer(int questionId, char userChoiceLetter, Dictionary<char, char> letterMapping)
        {
            return userChoiceLetter == 'A';
        }
    }
}
