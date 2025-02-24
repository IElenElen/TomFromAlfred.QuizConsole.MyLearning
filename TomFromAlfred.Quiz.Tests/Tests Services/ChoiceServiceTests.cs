using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;
using Xunit.Abstractions;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    // Ilość oblanych: 0 / 12
    // Nr 8 wykrzaczył się

    public class ChoiceServiceTests
    {
        public required ChoiceService _choiceService;
        private readonly ITestOutputHelper _output;

        public ChoiceServiceTests(ITestOutputHelper output)
        {
            _choiceService = new ChoiceService();
            _output = output; 

            _choiceService.Clear(); // Metoda do czyszczenia listy
        }

        // 1 
        [Fact] // Zaliczony

        public void Add_ShouldAddNewChoiceSet_WhenChoiceSetDoesNotExist() // Dodaje: ma dodać nowy zestaw wyboru, jeśli taki zestaw nie istnieje
        {
            //Arrange
            var newChoice0 = new Choice(22, 'A', "Opcja testowa A");
            var newChoice1 = new Choice(22, 'B', "Opcja testowa B");
            var newChoice2 = new Choice(22, 'C', "Ocpja testowa C");

            //Act
            _choiceService.Add(newChoice0);
            _choiceService.Add(newChoice1);
            _choiceService.Add(newChoice2);

            //Assert
            Assert.Contains(newChoice0, _choiceService.GetAllActive());
            Assert.Contains(newChoice1, _choiceService.GetAllActive());
            Assert.Contains(newChoice2, _choiceService.GetAllActive());
        }

        // 2
        [Fact] // Zaliczony
        public void Add_ShouldNotAdd_WhenChoiceExist() // Dodaje: nic nie dodaje, jeśli wybór istnieje
        {
            // Arrange
            _choiceService.Clear(); // Czyszczenie danych przed testem

            var existingChoice = new Choice(25, 'A', "Opcja A");
            _choiceService.Add(existingChoice); // Dodaję pierwszy raz

            var countBefore = _choiceService.GetAllActive().Count();

            // Act - Próbuję dodać tę samą opcję ponownie
            _choiceService.Add(existingChoice);

            var countAfter = _choiceService.GetAllActive().Count();

            // Assert - Liczba elementów nie powinna się zmienić
            Assert.Equal(countBefore, countAfter);
        }

        // 3
        [Fact] // Zaliczony
        public void Delete_ShouldRemoveExistingChoice() // Usuwa: istniejący wybóru, Id
        {
            //Arrange
            var existingChoiceToDelete = new Choice(22, 'A', "Opcja A");

            //Act
            _choiceService.Delete(existingChoiceToDelete);
            var allChoices = _choiceService.GetAllActive();


            //Assert
            Assert.DoesNotContain(existingChoiceToDelete, allChoices);
        }

        // 4
        [Fact] // Zaliczony
        public void Delete_ShouldNotRemoveChoice_WhenChoiceDoesNotExist() // Usuwa: nic nie usuwa, jeśli wybór nie istnieje
        {
            // Arrange
            var initialCount = _choiceService.GetAllActive().Count(); // Zapisuję liczbę aktywnych elementów przed testem

            var noExistingChoiceToDelete = new Choice(89, 'A', "Opcja nieistniejąca");

            // Act
            _choiceService.Delete(noExistingChoiceToDelete);
            var allChoices = _choiceService.GetAllActive();

            // Assert
            Assert.Equal(initialCount, allChoices.Count()); // Liczba elementów powinna się nie zmienić
        }


        // 5
        [Fact] // Zaliczony
        public void GetAll_ShouldReturnAllActiveChoices() // Pobiera: wszystkie aktywne wybory
        {
            // Arrange - Wypełniam listę aktywnymi wyborami
            _choiceService.Clear(); 

            _choiceService.Add(new Choice(40, 'A', "Jesień") { IsActive = true });
            _choiceService.Add(new Choice(40, 'B', "Zima") { IsActive = true });
            _choiceService.Add(new Choice(40, 'C', "Wiosna") { IsActive = true });
            _choiceService.Add(new Choice(41, 'A', "Kraków") { IsActive = true });
            _choiceService.Add(new Choice(41, 'B', "Warszawa") { IsActive = true });
            _choiceService.Add(new Choice(41, 'C', "Poznań") { IsActive = true });
            _choiceService.Add(new Choice(42, 'A', "Jabłko") { IsActive = true });
            _choiceService.Add(new Choice(42, 'B', "Gruszka") { IsActive = true });

            // Act
            var activeChoices = _choiceService.GetAllActive();

            // Assert
            Assert.Equal(8, activeChoices.Count());
        }

        // 6
        [Fact] // Zaliczony
        public void GetChoicesForQuestion_ShouldReturnFilteredChoiceSet_WhenQuestionIdExists() // Podaje: wybory dla pytania o istniejacym Id
        {
            // Arrange - dodanie testowych danych
            _choiceService.Add(new Choice(11, 'A', "A"));
            _choiceService.Add(new Choice(11, 'B', "B"));
            _choiceService.Add(new Choice(11, 'C', "C"));

            var choicesForQuestion = _choiceService.GetChoicesForQuestion(11);
            _output.WriteLine($"Choices count: {choicesForQuestion.Count()}"); // Podgląd
            foreach (var choice in choicesForQuestion)
            {
                Console.WriteLine($"Choice: {choice.ChoiceContent} (Id: {choice.ChoiceId})");
            }

            Assert.Equal(3, choicesForQuestion.Count()); // Trzy wybory dla pytania nr 11
        }

        // 7
        [Fact] // Zaliczony
        public void GetChoicesForQuestion_ShouldReturnEmptySet_WhenQuestionIdDoesNotExist() // Podaje: nie podaje wyborów, bo pytanie o danym Id nie istnieje
        {
            var choicesForQuestion = _choiceService.GetChoicesForQuestion(100);
            Assert.Empty(choicesForQuestion); // Nie ma wyboru dla pytania 100
        }

        // 8
        [Fact] // Zaliczony  - wykrzaczył się
        public void Update_ShouldUpdateChoiceContent_WhenChoiceExists() // Aktualizuje: zmienia treść wyboru, jeśli wybór istnieje
        {
            // Arrange: Tworzę wybór do zaktualizowania 
            var existingChoiceToUpdateContent = new Choice(11, 'A', "Jesień");

            // Act: Aktualizuję wybór (zmiana treści)
            existingChoiceToUpdateContent.ChoiceContent = "Zmieniona Jesień";
            _choiceService.Update(existingChoiceToUpdateContent);

            // Assert: Sprawdzam, czy treść została zaktualizowana
            var updatedChoiceContent = _choiceService.GetChoicesForQuestion(11).First(c => c.ChoiceLetter == 'A');
            Assert.Equal("Zmieniona Jesień", updatedChoiceContent.ChoiceContent);
        }

        // 9
        [Fact] // Zaliczony
        public void Update_ShouldUpdateChoiceLetter_WhenChoiceExists() // Aktualizuje: aktualizuje literę wyboru, jeśli wybór istnieje
        {
            // Arrange - Dodaję wybór do kolekcji przed testem
            var existingChoice = new Choice(12, 'A', "Kraków") { IsActive = true };
            _choiceService.Add(existingChoice);

            // Tworzę obiekt do aktualizacji
            var updatedChoice = new Choice(12, 'B', "Kraków z literą B") { IsActive = true };

            // Act - Aktualizuję wybór
            _choiceService.Update(updatedChoice);

            // Debugging - Sprawdzam, co zwraca GetChoicesForQuestion
            var updatedChoices = _choiceService.GetChoicesForQuestion(12);
            Console.WriteLine("Updated Choices:");
            foreach (var choice in updatedChoices)
            {
                Console.WriteLine($"ChoiceLetter: {choice.ChoiceLetter}, ChoiceContent: {choice.ChoiceContent}");
            }

            // Assert - Sprawdzam, czy zmiana się powiodła
            var updatedChoiceWithNewLetter = updatedChoices.FirstOrDefault(c => c.ChoiceLetter == 'B');
            Assert.NotNull(updatedChoiceWithNewLetter);
            Assert.Equal("Kraków z literą B", updatedChoiceWithNewLetter.ChoiceContent);

            // Sprawdzam, czy stary wybór z literą 'A' został usunięty
            var oldChoice = updatedChoices.FirstOrDefault(c => c.ChoiceLetter == 'A');
            Assert.Null(oldChoice);
        }

        // 10
        [Fact] // Zaliczony :-) nareszcie
        public void Update_ShouldNotUpdate_WhenChoiceDoesNotExist() // Aktualizacja: brak zmiany, jeśli dany wybór nie istnieje. Sprawdzam po Id wyboru.
        {
            // Arrange
            var existingChoiceToUpdate = new Choice(99, 'A', "Nieistniejąca opcja");

            // Sprawdzam, czy wyboru faktycznie nie ma w systemie przed aktualizacją
            var existingChoice = _choiceService.GetAllActive().FirstOrDefault(c => c.ChoiceId == 99);
            Assert.Null(existingChoice);  // Jeśli nie istnieje, to jest OK

            // Act
            _choiceService.Update(existingChoiceToUpdate);

            // Zbieram wszystkie wybory
            IEnumerable<Choice> allChoices = _choiceService.GetAllActive();
            _output.WriteLine($"Wszystkie wybory po próbie aktualizacji: {allChoices.Count()}");

            // Assert: Sprawdzam, czy zbiór wyborów nie został zmieniony
            var choiceAfterUpdate = allChoices.FirstOrDefault(c => c.ChoiceId == 99);
            Assert.Null(choiceAfterUpdate);  // Nadal nie istnieje

            // Dodatkowe sprawdzenie, czy wybory zostały poprawnie utrzymane
            foreach (var choice in allChoices)
            {
                _output.WriteLine($"All Choices - Id: {choice.ChoiceId}, Letter: {choice.ChoiceLetter}, Content: {choice.ChoiceContent}.");
            }
        }

        // 11
        [Fact] // Zaliczony
        public void Update_ShouldThrowArgumentException_WhenChoiceHasInvalidSign() // Aktualizuje: wyrzuca wyjątek, jeśli użytkownik poda niepoprawny znak
        {
            // Arrange: Lista znaków, które są niepoprawne (litera spoza zakresu, liczba, znak specjalny)
            var invalidSigns = new[] { 'Z', '1', '$' };

            foreach (var sign in invalidSigns)
            {
                // Act & Assert: Sprawdzam, czy konstruktor rzuca odpowiedni wyjątek
                var exception = Assert.Throws<ArgumentException>(() => new Choice(99, sign, "Nieistniejąca opcja", true));

                // Assert: Sprawdzam, czy komunikat wyjątku jest zgodny z oczekiwaniami
                Assert.Equal("Niepoprawny znak. Litera odpowiedzi musi być w zakresie A-C.", exception.Message);
            }
        }

        // 12
        [Fact] // Zaliczony
        public void Update_ShouldNotThrow_WhenChoiceHasValidLetter() // Aktualizuje: przyjmuje wybór, jeśli system otrzymuje prawidłową literę z zakresu A-C
        {
            // Arrange
            var validChoices = new List<Choice>
            {
                new Choice(99, 'A', "Opcja A", true),
                new Choice(100, 'B', "Opcja B", true),
                new Choice(101, 'C', "Opcja C", true)
            };

            foreach (var validChoice in validChoices)
            {
                // Act & Assert
                var exception = Record.Exception(() => new Choice(validChoice.ChoiceId, validChoice.ChoiceLetter, validChoice.ChoiceContent, validChoice.IsActive)); 
                Assert.Null(exception); // Upewniam się, że nie rzucił wyjątku
            }
        }
    }
}
