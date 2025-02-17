using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp
{
    public class JsonCommonClass // Klasa dla json = schemat działania plików
    {
        public virtual void CreateDefaultFile<T>(string filePath, T defaultData)
        {
            try
            {
                ReadFromFile<T>(filePath);
                Console.WriteLine($"Plik {filePath} już istnieje. Tworzenie domyślnego pliku pominięte.");
            }
            catch (FileNotFoundException)
            {
                WriteToFile(filePath, defaultData);
                Console.WriteLine($"Stworzono plik {filePath} z domyślnymi danymi.");
            }
        }

        // Odczyt danych z pliku JSON
        public virtual T ReadFromFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Plik {filePath} nie istnieje.");
            }

            Console.WriteLine($"Wczytuję plik: {filePath}");
            string json = File.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new InvalidDataException($"Plik {filePath} jest pusty lub zawiera nieprawidłowe dane.");
            }

            var data = JsonConvert.DeserializeObject<T>(json);

            // Uproszczone sprawdzenie null
            return data ?? throw new JsonException($"Nie udało się zdeserializować danych z pliku {filePath}. Upewnij się, że plik zawiera poprawny JSON.");
        }

        // Zapis danych do pliku JSON
        public virtual void WriteToFile<T>(string filePath, T data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
