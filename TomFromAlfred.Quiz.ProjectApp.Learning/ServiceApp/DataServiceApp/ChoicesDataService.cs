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
        private readonly List<Choice> _choices;

        public ChoicesDataService()
        {
            _choices = [];
            InitializeDefaultData();
        }

        private void InitializeDefaultData()
        {
            _choices.AddRange(
            [
                new (0, EntitySupport.OptionLetter.A, "8"),
                new (0, EntitySupport.OptionLetter.B, "9"),
                new (0, EntitySupport.OptionLetter.C, "10"),

                new (1, EntitySupport.OptionLetter.A, "Tomek w grobowcach faraonów."),
                new (1, EntitySupport.OptionLetter.B, "Tomek u źródeł Amazonki."),
                new (1, EntitySupport.OptionLetter.C, "Tajemnicza wyprawa Tomka."),

                new (2, EntitySupport.OptionLetter.A, "W Gdańsku."),
                new (2, EntitySupport.OptionLetter.B, "W Krakowie."),
                new (2, EntitySupport.OptionLetter.C, "W Warszawie."),

                new (3, EntitySupport.OptionLetter.A, "Na koledze z klasy."),
                new (3, EntitySupport.OptionLetter.B, "Na wrogu ze szkoły."),
                new (3, EntitySupport.OptionLetter.C, "Na pseudo-przyjacielu ze stadniny koni."),

                new (4, EntitySupport.OptionLetter.A, "historia"),
                new (4, EntitySupport.OptionLetter.B, "biologia"),
                new (4, EntitySupport.OptionLetter.C, "geografia"),

                new (5, EntitySupport.OptionLetter.A, "Janina"),
                new (5, EntitySupport.OptionLetter.B, "Antonina"),
                new (5, EntitySupport.OptionLetter.C, "Irena"),

                new (6, EntitySupport.OptionLetter.A, "poprawna"),
                new (6, EntitySupport.OptionLetter.B, "zła"),
                new (6, EntitySupport.OptionLetter.C, "zła 2"),

                new (7, EntitySupport.OptionLetter.A, "też nie"),
                new (7, EntitySupport.OptionLetter.B, "nie"),
                new (7, EntitySupport.OptionLetter.C, "dobra"),

                new (8, EntitySupport.OptionLetter.A, "też nie"),
                new (8, EntitySupport.OptionLetter.B, "dobra"),
                new (8, EntitySupport.OptionLetter.C, "nie")
            ]);
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
