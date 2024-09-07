using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.DataServiceApp
{
    public class CorrectDataService
    {
        public void AddCorrectData(string questionContent, EntitySupport.OptionLetter optionLetter, string contentCorrectAnswer)
        {
            ContentCorrectSets?.Add((questionContent, optionLetter, contentCorrectAnswer));
        }
    }
}
