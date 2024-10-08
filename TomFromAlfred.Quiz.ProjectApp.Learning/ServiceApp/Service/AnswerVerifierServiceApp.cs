﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service
{
    public class AnswerVerifierServiceApp
    {
        public List<ContentCorrectSet>? ContentCorrectSets { get; set; }

        public AnswerVerifierServiceApp(List<ContentCorrectSet> contentCorrectSets)
        {
            ContentCorrectSets = contentCorrectSets ?? new List<ContentCorrectSet>();
        }

        public void AddCorrectData(int questionId, EntitySupport.OptionLetter optionLetter, string? contentCorrectAnswer)
        {
            if (ContentCorrectSets == null || !ContentCorrectSets.Any())
            {
                ContentCorrectSets = new List<ContentCorrectSet>();
            }

            var newCorrectSet = new ContentCorrectSet(questionId, optionLetter, contentCorrectAnswer);

            ContentCorrectSets.Add(newCorrectSet);
        }

        public List<ContentCorrectSet> GetAllCorrectSets()
        {
            return ContentCorrectSets ?? new List<ContentCorrectSet>();
        }

        public bool GetPointsForAnswer(int questionId, char userChoice)
        {
            if (ContentCorrectSets == null || ContentCorrectSets.Count == 0)
            {
                return false;
            }

            var correctSet = ContentCorrectSets.FirstOrDefault(x => x.QuestionId == questionId);

            if (correctSet != null)
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
