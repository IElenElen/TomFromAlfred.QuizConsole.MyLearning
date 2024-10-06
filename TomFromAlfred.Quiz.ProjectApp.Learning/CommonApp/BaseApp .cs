using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp
{
    //Klasa bazowa to klasa wspólnych cech, ktorych nie chcemy powtarzać w serwisach i / lub menadżerach...

    //Sprawdzenie null dla Update - uwzględnienie komunikatu CS8602

    public class BaseApp<T> : IOperations<T> 
    {
        public List<T> Entities { get; set; }

        public BaseApp()
        {
            Entities = new List<T>();
        }

        public List<T> GetAll()
        {
            Console.WriteLine($"Pobieranie wszystkich encji, liczba encji: {Entities.Count}");
            return Entities;
        }

        public void Add(T entity)
        {
            Entities.Add(entity);
            Console.WriteLine($"Dodano encję: {entity}");
        }

        public void Update(T entity)
        {
            if (entity == null) // sprawdzenie, czy entity nie jest null przed dalszym działaniem
            {
                Console.WriteLine("Brak encji do aktualizacji.");
                return;
            }

            var existingEntity = Entities.FirstOrDefault(e => e.Equals(entity)); //szukanie istniejącego obiektu

            if (entity != null)
            {
                Console.WriteLine($"Aktualizacja encji: {entity}");
            }

            else
            {
                Console.WriteLine("Brak encji do aktualizacji.");
            }
        }

        public void Remove(T entity)
        {
            Entities.Remove(entity);
            Console.WriteLine($"Usunięto encję: {entity}");
        }
    }
}
