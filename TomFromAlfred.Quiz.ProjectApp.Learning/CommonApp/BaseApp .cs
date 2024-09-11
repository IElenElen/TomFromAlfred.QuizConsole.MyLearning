using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp
{
    //Klasa bazowa to klasa wspólnych cech, ktorych nie chcemy powtarzać w serwisach i / lub menadżerach...
    public class BaseApp<T> : IOperations<T> 
    {
        public List<T> Entities { get; set; }

        public BaseApp()
        {
            Entities = [];
        }

        public List<T> GetAll()
        {
            return Entities;
        }

        public void Add(T entity)
        {
            Entities.Add(entity);
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

        public void Remove(T entity)
        {
            Entities.Remove(entity);
        }
    }
}
