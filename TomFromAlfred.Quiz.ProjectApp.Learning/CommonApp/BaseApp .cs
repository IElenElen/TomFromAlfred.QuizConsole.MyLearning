using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract;
using TomFromAlfred.Quiz.ProjectDomain.Learning.CommonDomain;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp
{
    public class BaseApp<T> : IOperations<T> where T : BaseEntity //baza dla projektu app //klasa generyczna po interfejsie
    {
        public List<T> Entities { get; set; }
        public BaseApp()
        {
            Entities = new List<T>();
        }
        public List<T> GetAll()
        {
            return Entities;
        }
        public void Add(T entity)
        {
            Entities.Add(entity);
        }

        public void Remove(T entity)
        {
            Entities.Remove(entity);
        }

        public void Update(T entity)
        {
            if (entity != null)
            {
                return;
            }
            else
            {
                Console.WriteLine($"Brak {entity}");
            }
        }
    }
}
