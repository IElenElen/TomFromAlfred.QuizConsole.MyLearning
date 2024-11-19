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
        // Odczyt danych z pliku JSON
        public T ReadFromFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Plik {filePath} nie istnieje.");
            }

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
