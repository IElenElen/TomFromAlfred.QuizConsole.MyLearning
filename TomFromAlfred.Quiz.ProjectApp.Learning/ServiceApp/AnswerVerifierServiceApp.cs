using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class AnswerVerifierServiceApp 
    {
        public List<(string QuestionContent, EntitySupport.OptionLetter LetterCorrectAnswer, string ContentCorrectAnswer)> ?ContentCorrectSets { get; set; }

        public AnswerVerifierServiceApp()
        {
            ContentCorrectSets = new List<(string, EntitySupport.OptionLetter, string)>();
        }
        
        public bool GetPointsForAnswer(string questionContent, char userChoice)
        {
            if (ContentCorrectSets == null || ContentCorrectSets.Count == 0)
            {
                return false;
            }

            var correctSet = ContentCorrectSets.Find(x => x.QuestionContent == questionContent);

            if (correctSet != default)
            {
                if (Enum.TryParse<EntitySupport.OptionLetter>(userChoice.ToString(), out var parsedLetter))
                {
                    return correctSet.LetterCorrectAnswer == parsedLetter;
                }
            }

            return false;
        }
    }
}
