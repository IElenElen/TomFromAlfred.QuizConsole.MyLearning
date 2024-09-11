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
            _choices = new List<Choice>();
            InitializeDefaultData();
        }

        private void InitializeDefaultData()
        {
            _choices.AddRange(new List<Choice>
            {
                new Choice(0, EntitySupport.OptionLetter.A, "8"),
                new Choice(0, EntitySupport.OptionLetter.B, "9"),
                new Choice(0, EntitySupport.OptionLetter.C, "10"),

                new Choice(1, EntitySupport.OptionLetter.A, "Tomek w grobowcach faraonów."),
                new Choice(1, EntitySupport.OptionLetter.B, "Tomek u źródeł Amazonki."),
                new Choice(1, EntitySupport.OptionLetter.C, "Tajemnicza wyprawa Tomka."),

                new Choice(2, EntitySupport.OptionLetter.A, "W Gdańsku."),
                new Choice(2, EntitySupport.OptionLetter.B, "W Krakowie."),
                new Choice(2, EntitySupport.OptionLetter.C, "W Warszawie."),

                new Choice(3, EntitySupport.OptionLetter.A, "Na koledze z klasy."),
                new Choice(3, EntitySupport.OptionLetter.B, "Na wrogu ze szkoły."),
                new Choice(3, EntitySupport.OptionLetter.C, "Na pseudo-przyjacielu ze stadniny koni."),

                new Choice(4, EntitySupport.OptionLetter.A, "historia"),
                new Choice(4, EntitySupport.OptionLetter.B, "biologia"),
                new Choice(4, EntitySupport.OptionLetter.C, "geografia"),

                new Choice(5, EntitySupport.OptionLetter.A, "Janina"),
                new Choice(5, EntitySupport.OptionLetter.B, "Antonina"),
                new Choice(5, EntitySupport.OptionLetter.C, "Irena"),

                new Choice(6, EntitySupport.OptionLetter.A, "poprawna"),
                new Choice(6, EntitySupport.OptionLetter.B, "zła"),
                new Choice(6, EntitySupport.OptionLetter.C, "zła 2"),

                new Choice(7, EntitySupport.OptionLetter.A, "też nie"),
                new Choice(7, EntitySupport.OptionLetter.B, "nie"),
                new Choice(7, EntitySupport.OptionLetter.C, "dobra"),

                new Choice(8, EntitySupport.OptionLetter.A, "też nie"),
                new Choice(8, EntitySupport.OptionLetter.B, "dobra"),
                new Choice(8, EntitySupport.OptionLetter.C, "nie")
            });
        }

        public void SaveToJson(string filePath)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
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
