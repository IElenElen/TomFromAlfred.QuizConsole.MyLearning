using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.Abstract.AbstractForService
{
    public interface IQuizService
    {
        void InitializeJsonService(JsonCommonClass jsonService, string jsonFilePath);

        IEnumerable<Question> GetAllQuestions();
        IEnumerable<Choice> GetChoicesForQuestion(int questionId);
        IEnumerable<Choice> GetShuffledChoicesForQuestion(int questionId, out Dictionary<char, char> letterMapping);

        bool CheckAnswer(int questionId, char userChoiceLetter, Dictionary<char, char> letterMapping);
        string? GetAnswerContentFromLetter(char answerLetter, int questionId);

        void LoadQuestionsFromJson(string filePath);
        void LoadChoicesFromJson();
        void LoadCorrectSetFromJson();
        void AddQuestionToJson(Question question);
        void SaveQuestionsToJson();
    }
        
}
