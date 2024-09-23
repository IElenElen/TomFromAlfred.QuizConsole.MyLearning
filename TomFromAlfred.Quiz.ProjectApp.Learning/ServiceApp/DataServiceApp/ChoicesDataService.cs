using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.DataServiceApp
{
    public class ChoicesDataService
    {
        private readonly List<Choice> _choices = new List<Choice>();
        public ChoicesDataService()
        {
            InitializeDefaultData();
        }

        private void InitializeDefaultData()
        {
            _choices.AddRange(
            new List<Choice>
            {
                new (6, EntitySupport.OptionLetter.A, "poprawna"),
                new (6, EntitySupport.OptionLetter.B, "zła"),
                new (6, EntitySupport.OptionLetter.C, "zła 2"),

                new (7, EntitySupport.OptionLetter.A, "też nie"),
                new (7, EntitySupport.OptionLetter.B, "nie"),
                new (7, EntitySupport.OptionLetter.C, "dobra"),

                new (8, EntitySupport.OptionLetter.A, "też nie"),
                new (8, EntitySupport.OptionLetter.B, "dobra"),
                new (8, EntitySupport.OptionLetter.C, "nie")
            });
        }

        public void SaveToJson(string filePath)
        {
            try
            {
                JsonSerializerOptions options = new() { WriteIndented = true };
                var json = JsonSerializer.Serialize(_choices, options);
                File.WriteAllText(filePath, json);
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }

        public void LoadFromJson(string filePath)
            {
            if (File.Exists(filePath))
            {
                try
                {
                    var json = File.ReadAllText(filePath);
                    _choices.Clear();
                    var loadedChoices = JsonSerializer.Deserialize<List<Choice>>(json);
                    if (loadedChoices != null)
                    {
                        _choices.AddRange(loadedChoices);
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd: {ex.Message}");
                }
            }

            else
            {
                Console.WriteLine("Nie znaleziono pliku.");
            }
        }
    }
}
