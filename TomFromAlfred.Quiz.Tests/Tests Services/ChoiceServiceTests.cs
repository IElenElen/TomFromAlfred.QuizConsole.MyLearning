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
    public class ChoiceServiceTests
    {
        // Ilość oblanych: 4

        public required ChoiceService _choiceService;
        private readonly ITestOutputHelper _output;

        public ChoiceServiceTests(ITestOutputHelper output)
        {
            _choiceService = new ChoiceService();
            _output = output; 

            _output.WriteLine($"_choiceService is null: {_choiceService == null}");
        }

        [Fact] // Zaliczony

        public void Add_ShouldAddNewChoice_WhenChoiceDoesNotExist() // Ma dodać nowy zestaw wyboru
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

        [Fact] // Oblany, ma problem z liczeniem zestawów!!!
        public void Add_ShouldNotAdd_WhenChoiceExist() // Ma nie dodawać już istniejący wybór
        {
            //Arrange
            var existingChoice = new Choice(8, 'A', "Opcja A");

            //Act
            _choiceService.Add(existingChoice);
            var allChoices = _choiceService.GetAllActive();

            //Assert
            Assert.Single(allChoices);
        }

        [Fact] // Zaliczony
        public void Delete_ShouldRemoveExistingChoice() // Usuwa istniejący zestaw wyboru, Id
        {
            //Arrange
            var existingChoiceToDelete = new Choice(22, 'A', "Opcja A");

            //Act
            _choiceService.Delete(existingChoiceToDelete);
            var allChoices = _choiceService.GetAllActive();


            //Assert
            Assert.DoesNotContain(existingChoiceToDelete, allChoices);
        }

        [Fact] // Oblany, nadal widzi 9 zestawów, a jest aktywnych tylko 8 !!!
        public void Delete_ShouldNotRemoveChoice_WhenChoiceDoesNotExist() // Nic nie usuwa, jeśli zestaw wyborów nie istnieje
        {
            var noExistingChoiceToDelete = new Choice(89, 'A', "Opcja nieistniejąca");

            _choiceService.Delete(noExistingChoiceToDelete);
            var allChoices = _choiceService.GetAllActive();

            Assert.Equal(8, allChoices.Count()); // Nic w liście nie zmieniam
        }

        [Fact] // Oblany
        public void GetAll_ShouldReturnAllActiveChoices() // Pobiera wszystkie aktywne wybory
        {
            // Act: Pobieram wszystkie aktywne wybory
            var activeChoices = _choiceService.GetAllActive();

            // Debugging: Wypisanie danych w kolekcji przed testem
            Console.WriteLine("Active Choices in test:");
            foreach (var choice in activeChoices)
            {
                Console.WriteLine($"ChoiceId: {choice.ChoiceId}, ChoiceLetter: {choice.ChoiceLetter}, ChoiceContent: {choice.ChoiceContent}, IsActive: {choice.IsActive}");
            }

            // Assert: Oczekuję, że będzie dokładnie 8 aktywnych wyborów
            Assert.Equal(8, activeChoices.Count());  
        }

        [Fact] // Zaliczony
        public void GetChoicesForQuestion_ShouldReturnFilteredChoices_WhenQuestionIdExists() // Dodaje wybory dla pytania o istniejacym Id
        {
            var choicesForQuestion = _choiceService.GetChoicesForQuestion(11);
            Assert.Equal(3, choicesForQuestion.Count()); // Trzy wybory dla pytania nr 11
        }

        [Fact] // Zaliczony
        public void GetChoicesForQuestion_ShouldReturnEmpty_WhenQuestionIdDoesNotExist() // Nie dodaje wyborów, bo pytanie o danym Id nie istnieje
        {
            var choicesForQuestion = _choiceService.GetChoicesForQuestion(100);
            Assert.Empty(choicesForQuestion); // Nie ma wyboru dla pytania 100
        }

        [Fact] // Zaliczony
        public void Update_ShouldUpdateChoiceContent_WhenChoiceExists() // Możliwa zmiana treści
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

        [Fact] // Oblany // Czy on nie będzie integracyjny?
        public void Update_ShouldUpdateChoiceLetter_WhenChoiceExists() // Możliwa zmiana litery wyboru
        {
            // Arrange: Tworzę wybór do zaktualizowania (zmiana litery)
            var existingChoiceWithLetterToUpdate = new Choice(12, 'A', "Kraków");

            // Zmieniam literę i treść
            existingChoiceWithLetterToUpdate.ChoiceLetter = 'B';
            //existingChoiceWithLetterToUpdate.ChoiceContent = "Kraków z literą B";

            // Act: Aktualizuję wybór (zmiana litery)
            _choiceService.Update(existingChoiceWithLetterToUpdate);

            // Debugging: Sprawdzam dane przed asercjami
            var updatedChoiceWithLetter = _choiceService.GetChoicesForQuestion(12); // ???
            Console.WriteLine("Updated Choice: ");
            foreach (var choice in updatedChoiceWithLetter)
            {
                Console.WriteLine($"ChoiceLetter: {choice.ChoiceLetter}, ChoiceContent: {choice.ChoiceContent}");
            }

            // Assert: Sprawdzam, czy litera została zaktualizowana oraz treść
            var updatedChoiceWithNewLetter = updatedChoiceWithLetter.FirstOrDefault(c => c.ChoiceLetter == 'B');
            Assert.NotNull(updatedChoiceWithNewLetter); // Upewnij się, że wybór z literą 'B' istnieje
            Assert.Equal("Kraków z literą B", updatedChoiceWithNewLetter.ChoiceContent);

            // Dodatkowo: Sprawdzam, czy stary wybór z literą 'A' został usunięty
            var oldChoiceWithOldLetter = updatedChoiceWithLetter.FirstOrDefault(c => c.ChoiceLetter == 'A');
            Assert.Null(oldChoiceWithOldLetter);  // Stary wybór z literą 'A' powinien zostać usunięty
        }

        [Fact] // Zaliczony :-) nareszcie
        public void Update_ShouldNotUpdate_WhenChoiceDoesNotExist() // Brak zmiany, jeśli dany wybór nie istnieje. Sprawdzam po Id wyboru.
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

        [Fact] // Zaliczony
        public void Update_ShouldThrowArgumentException_WhenChoiceHasInvalidSign() // Wyrzuca wyjątek, jeśli użytkownik podaa niepoprawny znak
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

        [Fact] // Zaliczony
        public void Update_ShouldNotThrow_WhenChoiceHasValidLetter() // Przyjmuje wybór, jeśli użytkownik daje prawidłową literę z zakresu A-C
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
