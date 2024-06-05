using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class ChoicesArraysServiceApp
    {
        private ChoiceServiceApp choiceServiceApp;

        public ChoicesArraysServiceApp()
        {
            choiceServiceApp = new ChoiceServiceApp();
        }

        public Choice[] GetChoicesForQuestion(int questionNumber)
        {
            if (questionNumber >= 0 && questionNumber < choiceServiceApp.ChoicesArrays.Length)
            {
                return choiceServiceApp.ChoicesArrays[questionNumber];
            }
            else
            {
                return new Choice[0];
            }
        }
    }
}
