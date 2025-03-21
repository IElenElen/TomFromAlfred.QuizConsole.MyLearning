using Moq;
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
    // Ilość oblanych: 10 / 13
    // Nr 1 2 3 4 5 6 8 9 10 11

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
        [Theory] // 
        [InlineData('A', "Opcja A")]
        [InlineData('B', "Opcja B")]
        [InlineData('C', "Opcja C")]

        public void Add_ShouldAddNewChoiceSet_WhenChoiceSetDoesNotExist(char letter, string content) // Dodaje: ma dodać nowy zestaw wyboru, jeśli taki zestaw nie istnieje
        {
            // Arrange
            var choice = new Choice(22, letter, content);

            // Act
            _choiceService.Add(choice);

            // Assert
            Assert.Contains(_choiceService.GetAllActive(), c => c.ChoiceId == 22 && c.ChoiceLetter == letter);
        }

        // 2
        [Fact] // 
        public void Add_ShouldNotAdd_WhenChoiceExists_AndKeepChoiceCountUnchanged() // Dodaje: nic nie dodaje, jeśli wybór istnieje, nie zmienia liczby wyborów
        {
            // Arrange
            _choiceService.Clear();

            var existingChoice = new Choice(25, 'A', "Opcja A");

            _choiceService.Add(existingChoice);
            var countBefore = _choiceService.GetAllActive().Count();

            // Act
            _choiceService.Add(existingChoice);
            var countAfter = _choiceService.GetAllActive().Count();

            // Assert
            Assert.Equal(countBefore, countAfter); 
        }

        //
        [Fact] // 
        public void Add_ShouldNotDuplicateChoice_WhenSameIdAndLetterUsed() // Dodaje: nie duplikuje wyboru 
        {
            // Arrange
            _choiceService.Clear();

            var existingChoice = new Choice(25, 'A', "Opcja A");
            _choiceService.Add(existingChoice);

            // Act
            _choiceService.Add(existingChoice); // Próba duplikatu

            // Assert
            var duplicates = _choiceService.GetAllActive()
                .Where(c => c.ChoiceId == 25 && c.ChoiceLetter == 'A')
                .ToList();

            Assert.Single(duplicates); // Tylko jedna taka opcja
        }

        // 3
        [Fact] // 
        public void DeleteChoiceById_ShouldRemoveAllChoicesWithGivenId() // Usuwa: istniejący wybór, po Id
        {
            // Arrange
            _choiceService.Clear();

            _choiceService.Add(new Choice(50, 'A', "Opcja A"));
            _choiceService.Add(new Choice(50, 'B', "Opcja B"));
            _choiceService.Add(new Choice(50, 'C', "Opcja C"));

            // Act
            _choiceService.DeleteChoiceById(50);

            // Assert
            var remaining = _choiceService.GetAllActive().Where(c => c.ChoiceId == 50);
            Assert.Empty(remaining);
        }

        //
        [Fact] //
        public void Delete_ShouldRemoveChoice_OnlyWhenIdAndLetterMatch() // Usuwa: wybór po id i literze
        {
            // Arrange
            _choiceService.Clear();
            _choiceService.Add(new Choice(60, 'A', "Opcja A"));
            _choiceService.Add(new Choice(60, 'B', "Opcja B"));

            // Act
            _choiceService.Delete(new Choice(60, 'A', "Nieistotne."));

            // Assert
            var remaining = _choiceService.GetChoicesForQuestion(60).ToList();

            Assert.DoesNotContain(remaining, c => c.ChoiceLetter == 'A');
            Assert.Contains(remaining, c => c.ChoiceLetter == 'B');
        }
        //
        [Fact] //
        public void Delete_ShouldNotRemoveChoice_WhenLetterDoesNotMatch() // Usuwa: nie usuwa, jeśli litera błędna
        {
            // Arrange
            _choiceService.Clear();

            _choiceService.Add(new Choice(70, 'A', "Opcja A."));

            // Act
            _choiceService.Delete(new Choice(70, 'B', "Nieistotna."));

            // Assert
            var remaining = _choiceService.GetChoicesForQuestion(70);

            Assert.Single(remaining);
            Assert.Contains(remaining, c => c.ChoiceLetter == 'A');
        }

        // 4
        [Fact] // 
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

        [Fact]
        public void Delete_ShouldNotThrow_WhenChoiceIsNull() 
        { ...
                }


        // 5
        [Fact] // 
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
        [Fact] // 
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
        [Fact] // 
        public void Update_ShouldUpdateChoiceContent_WhenChoiceExists() // Aktualizuje: zmienia treść wyboru, jeśli wybór istnieje
        {
            // Arrange
            var existingChoiceToUpdateContent = new Choice(11, 'A', "Jesień");

            // Dodaję wybór do kolekcji, jeśli test tego wymaga
            _choiceService.Add(existingChoiceToUpdateContent);

            // Act
            existingChoiceToUpdateContent.ChoiceContent = "Zmieniona Jesień";
            _choiceService.Update(existingChoiceToUpdateContent);

            // Pobieram wszystkie wybory po aktualizacji
            var allChoicesAfterUpdate = _choiceService.GetChoicesForQuestion(11);

            Console.WriteLine("Po aktualizacji:");
            foreach (var choice in allChoicesAfterUpdate)
            {
                Console.WriteLine($"ChoiceLetter: {choice.ChoiceLetter}, Content: {choice.ChoiceContent}");
            }

            // Assert: Sprawdzam, czy treść została zaktualizowana
            var updatedChoiceContent = allChoicesAfterUpdate.FirstOrDefault(c => c.ChoiceLetter == 'A');

            Assert.NotNull(updatedChoiceContent); // Zapewnia, że wybór istnieje
            Assert.Equal("Zmieniona Jesień", updatedChoiceContent!.ChoiceContent); // Sprawdza, czy treść została zaktualizowana
        }

        // 9
        [Fact] // 
        public void Update_ShouldUpdateChoiceLetter_WhenChoiceExists() // Aktualizuje: aktualizuje literę wyboru, jeśli wybór istnieje
        {
            // Arrange - Dodaję wybór do kolekcji przed testem
            var existingChoice = new Choice(12, 'A', "Kraków") { IsActive = true };
            _choiceService.Add(existingChoice);

            // Act - Aktualizuję literę wyboru
            _choiceService.UpdateChoiceLetter(12, 'B');

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
            Assert.Equal("Kraków", updatedChoiceWithNewLetter.ChoiceContent);

            // Sprawdzam, czy stary wybór z literą 'A' został usunięty
            var oldChoice = updatedChoices.FirstOrDefault(c => c.ChoiceLetter == 'A');
            Assert.Null(oldChoice);
        }

        // 10
        [Fact] // 
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
        [Fact] // 
        public void Update_ShouldNotChangeChoice_WhenContentIsSame() // Aktualizuje: nic nie robi, jeśli treść wyboru pozostaje ta sama
        {
            // Arrange
            var choiceService = new ChoiceService();
            var existingChoice = new Choice(15, 'C', "Wąż z Afryki");
            choiceService.Add(existingChoice);

            var sameContentChoice = new Choice(15, 'C', "Wąż z Afryki");

            // Act
            choiceService.Update(sameContentChoice);

            // Assert
            Assert.Equal("Wąż z Afryki", choiceService.GetChoicesForQuestion(15).First().ChoiceContent);
        }

        [Fact]
        public void Update_ShouldNotThrow_WhenChoiceIsNull() { ... }

        // 12
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

        // 13
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

        // Dodatkowo:

        [Theory]
        [InlineData('Z')]
        [InlineData('1')]
        [InlineData('$')]
        public void Constructor_ShouldThrow_WhenChoiceLetterIsInvalid(char invalidLetter)
        {
            var ex = Assert.Throws<ArgumentException>(() => new Choice(99, invalidLetter, "Opcja", true));
            Assert.Equal("Niepoprawny znak. Litera odpowiedzi musi być w zakresie A-C.", ex.Message);
        }
    }
}
