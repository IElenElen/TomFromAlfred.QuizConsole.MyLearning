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
            InitializeDataCorrectData();
        }

        public void InitializeDataCorrectData()
        {
            Console.WriteLine("Inicjalizacja domyślnych danych odpowiedzi...");
            ContentCorrectSets.Add(new ContentCorrectSet(9, EntitySupport.OptionLetter.A, " ")); //na razie zmyślone
            Console.WriteLine("Domyślne dane odpowiedzi zostały zainicjalizowane.");
        }

        public void LoadDataFromJson(string filePath)
        {
            Console.WriteLine($"Rozpoczynanie wczytywania danych z pliku: {filePath}");

            if (File.Exists(filePath))
            {
                try
                {
                    var json = File.ReadAllText(filePath);

                    var loadedData = JsonSerializer.Deserialize<List<ContentCorrectSet>>(json);

                    if (loadedData == null || loadedData.Any(c => c.QuestionId <= 0 || !Enum.IsDefined(typeof(EntitySupport.OptionLetter), c.LetterCorrectAnswer)))
                    {
                        Console.WriteLine("Dane odpowiedzi są niekompletne lub niepoprawne.");
                        InitializeDataCorrectData();
                        return;
                    }

                    ContentCorrectSets = loadedData;
                    Console.WriteLine($"Wczytano {ContentCorrectSets.Count} zestawów poprawnych odpowiedzi.");
                }
                catch (JsonException jsonEx)
                {
                    Console.WriteLine($"Błąd deserializacji JSON: {jsonEx.Message}");
                    InitializeDataCorrectData();
                }
                catch (IOException ioEx)
                {
                    Console.WriteLine($"Błąd odczytu pliku: {ioEx.Message}");
                    InitializeDataCorrectData();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił nieoczekiwany błąd: {ex.Message}");
                    InitializeDataCorrectData();
                }
            }
            else
            {
                InitializeDataCorrectData();
            }
        }

        public void SaveDataToJson(string filePath)
        {
            if (ContentCorrectSets == null || !ContentCorrectSets.Any())
            {
                Console.WriteLine("Brak danych do zapisania.");
                return;
            }

            try
            {
                Console.WriteLine($"Rozpoczynanie zapisu danych do pliku: {filePath}");

                var directory = Path.GetDirectoryName(filePath);

                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory); //system tworzy katalog
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(ContentCorrectSets, options);
                File.WriteAllText(filePath, json);

                Console.WriteLine("Dane zostały pomyślnie zapisane do pliku.");
            }
            catch (UnauthorizedAccessException authEx)
            {
                Console.WriteLine($"Brak uprawnień do zapisu w pliku: {authEx.Message}");
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Błąd odczytu/zapisu pliku: {ioEx.Message}");
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"Błąd podczas serializacji do JSON: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił nieoczekiwany błąd: {ex.Message}");
            }
        }
    }
}
