using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.EntityService;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.QuizConsole.Tests.Z___SupportForTests
{
    public static class DataClearingCommonClass
    {
        public static void ClearAll(                 // Nullable
        QuestionService? questionService,
        ChoiceService? choiceService,
        CorrectAnswerService? correctAnswerService,
        ScoreService? score)
        {
            if (questionService is not null)
            {
                ClearPrivateList<Question>(questionService, "_questions");
            }

            if (choiceService is not null)
            { 
                ClearPrivateList<Choice>(choiceService, "_choices");
            }

            if (correctAnswerService is not null)
            {
                ClearPrivateList<CorrectAnswer>(correctAnswerService, "_correctAnswers");
            }

            if(score is not null)
            {
                ClearScore(score);
            }
        }

        public static void ClearQuestions(QuestionService questionService)
        {
            ClearPrivateList<Question>(questionService, "_questions");
        }

        public static void ClearChoices(ChoiceService choiceService)
        {
            ClearPrivateList<Choice>(choiceService, "_choices");
        }

        public static void ClearCorrectAnswers(CorrectAnswerService correctAnswerService)
        {
            ClearPrivateDictionary<int, CorrectAnswer>(correctAnswerService, "_correctAnswers");
        }

        public static void ClearScore(ScoreService score)
        {
            var field = score.GetType().GetField("_score", BindingFlags.NonPublic | BindingFlags.Instance);
            ArgumentNullException.ThrowIfNull(field);

            field.SetValue(score, 0);
        }

        private static void ClearPrivateList<T>(object target, string fieldName)
        {
            var field = target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field?.GetValue(target) is IList<T> list)
            {
                list.Clear();
            }
            else
            {
                throw new InvalidOperationException($"Nie znaleziono pola '{fieldName}' lub nie jest IList<{typeof(T).Name}> w klasie {target.GetType().Name}.");
            }
        }

        private static void ClearPrivateDictionary<TKey, TValue>(object target, string fieldName)
        {
            var field = target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field?.GetValue(target) is IDictionary<TKey, TValue> dict)
            {
                dict.Clear();
            }
            else
            {
                throw new InvalidOperationException($"Nie znaleziono pola '{fieldName}' lub nie jest IDictionary<{typeof(TKey).Name}, {typeof(TValue).Name}> w klasie {target.GetType().Name}.");
            }
        }
    }
}
