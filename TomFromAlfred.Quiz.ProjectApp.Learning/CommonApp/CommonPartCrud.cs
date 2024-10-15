using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp
{
    public class GenericCommonPartCrud<T> where T : class
    {
        public void Add(T entity, Question question)
        {
            //entity.Add();
            //entity.Add(question);
            Console.WriteLine($"{question}: Co nastepuje po zimie?"); //osobne dla question, choice
        }
    }
}
