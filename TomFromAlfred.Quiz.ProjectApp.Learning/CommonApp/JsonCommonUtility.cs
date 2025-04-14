using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.ServiceSupport;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp
{
    public class JsonCommonUtility // Klasa dla json = schemat działania plików
    {
        private readonly IFileWrapper _fileWrapper;

        public JsonCommonUtility(IFileWrapper fileWrapper)
        {
            _fileWrapper = fileWrapper;
        }

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

        public virtual T ReadFromFile<T>(string filePath)
        {
            if (!_fileWrapper.Exists(filePath))
                throw new FileNotFoundException($"Plik {filePath} nie istnieje.");

            Console.WriteLine($"Wczytuję plik: {filePath}");
            string json = _fileWrapper.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(json))
                throw new JsonReaderException($"Plik {filePath} jest pusty lub zawiera nieprawidłowe dane.");

            var data = JsonConvert.DeserializeObject<T>(json);

            return data ?? throw new JsonException($"Nie udało się zdeserializować danych z pliku {filePath}.");
        }

        public virtual void WriteToFile<T>(string filePath, T data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            string json = JsonConvert.SerializeObject(data);
            _fileWrapper.WriteAllText(filePath, json);
        }
    }
}
