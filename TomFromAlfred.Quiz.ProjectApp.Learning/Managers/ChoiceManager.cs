using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.Managers
{
    public class ChoiceManager : BaseApp<Choice> //kod z poprzedniego ChoiceHandling
    {
        public Choice Choice0A => new Choice(0, 'a', "8");
        public Choice Choice0B => new Choice(0, 'b', "9");
        public Choice Choice0C => new Choice(0, 'c', "10");
        public Choice Choice1A => new Choice(1, 'a', "Tomek w grobowcach faraonów.");
        public Choice Choice1B => new Choice(1, 'b', "Tomek u źródeł Amazonki.");
        public Choice Choice1C => new Choice(1, 'c', "Tajemnicza wyprawa Tomka.");
        public Choice Choice2A => new Choice(2, 'a', "W Gdańsku.");
        public Choice Choice2B => new Choice(2, 'b', "W Krakowie.");
        public Choice Choice2C => new Choice(2, 'c', "W Warszawie.");
        public Choice Choice3A => new Choice(3, 'a', "Na koledze z klasy.");
        public Choice Choice3B => new Choice(3, 'b', "Na wrogu ze szkoły.");
        public Choice Choice3C => new Choice(3, 'c', "Na pseudo-przyjacielu ze stadniny koni.");
        public Choice Choice4A => new Choice(4, 'a', "historia");
        public Choice Choice4B => new Choice(4, 'b', "biologia");
        public Choice Choice4C => new Choice(4, 'c', "geografia");
        public Choice Choice5A => new Choice(5, 'a', "Janina");
        public Choice Choice5B => new Choice(5, 'b', "Antonina");
        public Choice Choice5C => new Choice(5, 'c', "Irena");
        public void RemoveChoice(Choice choiceToRemove) //w planach
        {
            Remove(choiceToRemove);
        }
    }
}
