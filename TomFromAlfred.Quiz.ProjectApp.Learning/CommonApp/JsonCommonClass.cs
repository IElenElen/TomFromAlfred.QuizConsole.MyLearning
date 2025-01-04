using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp
{
    public class JsonCommonClass
    {
        public void CreateDefaultFile<T>(string filePath, T defaultData)
        {
            if (!File.Exists(filePath))
            {
                WriteToFile(filePath, defaultData);
                Console.WriteLine($"Stworzono plik {filePath} z domyślnymi danymi.");
            }
            else
            {
                Console.WriteLine($"Plik {filePath} już istnieje. Tworzenie domyślnego pliku pominięte.");
            }
        }
        // Odczyt danych z pliku JSON
        public T ReadFromFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Plik {filePath} nie istnieje.");
            }

            Console.WriteLine($"Wczytuję plik: {filePath}");
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json);
        }

        // Zapis danych do pliku JSON
        public void WriteToFile<T>(string filePath, T data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
