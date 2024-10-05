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
                new (6, 6, EntitySupport.OptionLetter.A, "poprawna"), //id wyboru, nr wyboru, litera opcji, treść wyboru
                new (6, 6, EntitySupport.OptionLetter.B, "zła"),
                new (6, 6, EntitySupport.OptionLetter.C, "zła 2"),

                new (7, 7, EntitySupport.OptionLetter.A, "też nie"),
                new (7, 7, EntitySupport.OptionLetter.B, "nie"),
                new (7, 7, EntitySupport.OptionLetter.C, "dobra"),

                new (8, 8, EntitySupport.OptionLetter.A, "też nie"),
                new (8, 8, EntitySupport.OptionLetter.B, "dobra"),
                new (8, 8, EntitySupport.OptionLetter.C, "nie")
            });

            Console.WriteLine("Zainicjowano domyślne dane wyborów.");
        }

        public void SaveToJson(string filePath)
        {
            try
            {
                Console.WriteLine($"Rozpoczynanie zapisu danych do pliku: {filePath}");

                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory); // tworzenie brakującego katalogu
                    Console.WriteLine($"Katalog '{directory}' został utworzony.");
                }

                JsonSerializerOptions options = new() { WriteIndented = true };
                var json = JsonSerializer.Serialize(_choices, options);
                File.WriteAllText(filePath, json);

                Console.WriteLine("Zapisano dane wyborów do pliku.");
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
            Console.WriteLine($"Rozpoczynanie wczytywania danych z pliku: {filePath}");

            if (File.Exists(filePath))
            {
                try
                {
                    var json = File.ReadAllText(filePath);
                    _choices.Clear();
                    var loadedChoices = JsonSerializer.Deserialize<List<Choice>>(json);

                    if (loadedChoices == null || loadedChoices.Any(c => string.IsNullOrEmpty(c.ChoiceContent))) //walidacja
                    {
                        Console.WriteLine("Błąd w danych wyboru. Nie wszystkie pola są poprawne.");
                        return;
                    }

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
