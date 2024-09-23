using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.DataServiceApp
{
    public class CorrectDataService
    {
        public List<ContentCorrectSet> ContentCorrectSets { get; set; }

        public CorrectDataService()
        {
            ContentCorrectSets = new List<ContentCorrectSet>(); 
            InitializeData(); 
        }

        public void InitializeData()
        {
            ContentCorrectSets.Add(new ContentCorrectSet(" ", EntitySupport.OptionLetter.A, " "));
        }

        public void LoadDataFromJson(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    var json = File.ReadAllText(filePath);
                    ContentCorrectSets = JsonSerializer.Deserialize<List<ContentCorrectSet>>(json) ?? new List<ContentCorrectSet>();
                }
                catch (JsonException jsonEx)
                {
                    Console.WriteLine($"Błąd deserializacji JSON: {jsonEx.Message}");
                    InitializeData();
                }
                catch (IOException ioEx)
                {
                    Console.WriteLine($"Błąd odczytu pliku: {ioEx.Message}");
                    InitializeData();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił nieoczekiwany błąd: {ex.Message}");
                    InitializeData();
                }
            }
            else
            {
                InitializeData();
            }
        }

        public void SaveDataToJson(string filePath)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(ContentCorrectSets, options);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd zapisywania do pliku: {ex.Message}");
            }
        }
    }
}
