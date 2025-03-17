using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract.AbstractForService;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.QuizConsole.Tests.SupportForTests
{
    public class FakeQuizService : IQuizService
    {
        private List<Question> _testQuestions = new();
        private Dictionary<int, List<Choice>> _testChoices = new();
        private Dictionary<int, string> _testCorrectAnswers = new();

        public void SetTestData(List<Question> questions, Dictionary<int, List<Choice>> choices, Dictionary<int, string> correctAnswers)
        {
            _testQuestions = questions;
            _testChoices = choices;
            _testCorrectAnswers = correctAnswers;
        }

        public void AddQuestionToJson(Question question)
        {
            throw new NotImplementedException();
        }

        public bool CheckAnswer(int questionId, char userChoiceLetter, Dictionary<char, char> letterMapping)
        {
            if (!_testCorrectAnswers.TryGetValue(questionId, out var correctLetter))
                return false; // Brak poprawnej odpowiedzi

            return letterMapping.TryGetValue(userChoiceLetter, out char mappedLetter) && mappedLetter.ToString() == correctLetter;

        }

        public IEnumerable<Question> GetAllQuestions()
        {
            return _testQuestions;
        }

        public string? GetAnswerContentFromLetter(char answerLetter, int questionId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Choice> GetChoicesForQuestion(int questionId)
        {
            return _testChoices.TryGetValue(questionId, out var choices) ? choices : new List<Choice>();
        }

        public IEnumerable<Choice> GetShuffledChoicesForQuestion(int questionId, out Dictionary<char, char> letterMapping)
        {
            letterMapping = new Dictionary<char, char> { { 'A', 'A' }, { 'B', 'B' }, { 'C', 'C' } };
            return GetChoicesForQuestion(questionId);
        }

        public void InitializeJsonService(JsonCommonClass jsonService, string jsonFilePath)
        {
            throw new NotImplementedException();
        }

        public void LoadChoicesFromJson()
        {
            throw new NotImplementedException();
        }

        public void LoadCorrectSetFromJson()
        {
            throw new NotImplementedException();
        }

        public void LoadQuestionsFromJson(string filePath)
        {
            throw new NotImplementedException();
        }

        public void SaveQuestionsToJson()
        {
            throw new NotImplementedException();
        }
    }
}
