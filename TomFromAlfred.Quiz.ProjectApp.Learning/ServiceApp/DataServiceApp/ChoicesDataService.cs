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
                new (EntitySupport.OptionLetter.A, 6, "poprawna"), //pamietać o tym, że w innej kolejności wyświetlić
                                                                   //dla użytkownika!!!
                new (EntitySupport.OptionLetter.B, 6, "zła"),
                new (EntitySupport.OptionLetter.C, 6, "zła 2"),

                new (EntitySupport.OptionLetter.A, 7, "też nie"),
                new (EntitySupport.OptionLetter.B, 7, "nie"),
                new (EntitySupport.OptionLetter.C, 7, "dobra"),

                new (EntitySupport.OptionLetter.A, 8, "też nie"),
                new (EntitySupport.OptionLetter.B, 8, "dobra"),
                new (EntitySupport.OptionLetter.C, 8, "nie")
            });
        }

        public void SaveToJson(string filePath)
        {
            try
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Console.WriteLine($"Katalog '{directory}' nie istnieje.");
                    return; // przerwij dalsze operacje, jeśli katalog nie istnieje
                }

                JsonSerializerOptions options = new() { WriteIndented = true };
                var json = JsonSerializer.Serialize(_choices, options);
                File.WriteAllText(filePath, json);
            }
            catch (UnauthorizedAccessException uae)
            {
                Console.WriteLine($"Brak dostępu do pliku: {uae.Message}");
            }
            catch (DirectoryNotFoundException dnf)
            {
                Console.WriteLine($"Nie znaleziono katalogu: {dnf.Message}");
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Błąd we/wy podczas zapisu: {ioEx.Message}");
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
                    if (loadedChoices != null && loadedChoices.Count > 0)
                    {
                        _choices.AddRange(loadedChoices);
                    }
                }
                catch (JsonException jsonEx)
                {
                    Console.WriteLine($"Błąd podczas deserializacji: {jsonEx.Message}");
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
