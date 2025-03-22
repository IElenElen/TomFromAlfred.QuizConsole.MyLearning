using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;
using Xunit.Abstractions;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    // Ilość oblanych: 0 / ?

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
        public void Delete_ShouldDoNothing_WhenChoiceDoesNotExist() // Usuwa: nic nie usuwa, jeśli wybór nie istnieje
        {
            // Arrange
            var initialCount = _choiceService.GetAllActive().Count(); // Zapisuję liczbę aktywnych elementów przed testem

            var noExistingChoiceToDelete = new Choice(89, 'A', "Opcja nieistniejąca.");

            // Act
            _choiceService.Delete(noExistingChoiceToDelete);
            var allChoices = _choiceService.GetAllActive();

            // Assert
            Assert.Equal(initialCount, allChoices.Count()); // Liczba elementów powinna się nie zmienić
        }

        //
        [Fact] //
        public void Delete_ShouldNotThrow_WhenChoiceIsNull() // Usuwa: nic nie robi, jeśli choice to null
        {
            // Act
            var exception = Record.Exception(() => _choiceService.Delete(null));

            // Assert
            Assert.Null(exception); // Nie powinien rzucać wyjątku
        }

        // 5
        [Fact] // 
        public void GetAllActive_ShouldReturnOnlyChoicesWithIsActiveTrue() // Pobiera: wszystkie aktywne wybory
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

        // 
        [Fact] //
        public void GetAllActive_ShouldNotReturnInactiveChoices() // Pobiera: nie pobiera wyborów, które są nieaktywne
        {
            // Arrange
            _choiceService.Clear();

            _choiceService.Add(new Choice(50, 'A', "Aktywna") { IsActive = true });
            _choiceService.Add(new Choice(50, 'B', "Nieaktywna") { IsActive = false });

            // Act
            var activeChoices = _choiceService.GetAllActive().ToList();

            // Assert
            Assert.Single(activeChoices);
            Assert.All(activeChoices, c => Assert.True(c.IsActive));
            Assert.DoesNotContain(activeChoices, c => c.IsActive == false);
        }

        // 6
        [Fact] // 
        public void GetChoicesForQuestion_ShouldReturnThreeChoicesInSet_WhenQuestionIdExists() // Podaje: 3 wybory w zestawie dla pytania o istniejacym Id
        {
            // Arrange
            _choiceService.Clear();

            _choiceService.Add(new Choice(11, 'A', "Testowe A"));
            _choiceService.Add(new Choice(11, 'B', "Testowe B"));
            _choiceService.Add(new Choice(11, 'C', "Testowe C"));

            // Act
            var result = _choiceService.GetChoicesForQuestion(11);

            // Assert
            Assert.Equal(3, result.Count());
        }

        //
        [Fact] //
        public void GetChoicesForQuestion_ShouldReturnOnlyChoices_Set_WithGivenQuestionId() // Podaje: zestaw dla pytania o danym Id
        {
            var result = _choiceService.GetChoicesForQuestion(11);

            Assert.All(result, c => Assert.Equal(11, c.ChoiceId));
        }

        // 
        [Theory] // 
        [InlineData('A', "Testowe A")]
        [InlineData('B', "Testowe B")]
        [InlineData('C', "Testowe C")]
        public void GetChoicesForQuestion_ShouldContainExpectedChoices(char letter, string content) // Podaje: przypisany zestaw = literę i treść
        {
            // Arrange
            _choiceService.Clear();
            _choiceService.Add(new Choice(11, 'A', "Testowe A"));
            _choiceService.Add(new Choice(11, 'B', "Testowe B"));
            _choiceService.Add(new Choice(11, 'C', "Testowe C"));

            var result = _choiceService.GetChoicesForQuestion(11);

            Assert.Contains(result, c => c.ChoiceLetter == letter && c.ChoiceContent == content);
        }

        // 7
        [Fact] // Zaliczony
        public void GetChoicesForQuestion_ShouldReturnNothing_WhenQuestionIdNotFound() // Podaje: nie podaje wyborów, bo pytanie o danym Id nie istnieje
        {
            // Arrange - brak danych

            // Act
            var choicesForQuestion = _choiceService.GetChoicesForQuestion(100);

            // Assert
            Assert.Empty(choicesForQuestion); // Nie ma wyboru dla pytania 100
            Assert.NotNull(choicesForQuestion);
        }

        // 8
        [Fact] // 
        public void Update_ShouldUpdateChoiceContent_WhenChoiceExists() // Aktualizuje: zmienia treść wyboru, jeśli wybór istnieje
        {
            // Arrange
            _choiceService.Clear();

            var originalChoice = new Choice(11, 'A', "Jesień.");
            _choiceService.Add(originalChoice);

            var updatedChoice = new Choice(11, 'A', "Zmieniona Jesień.");

            // Act
            _choiceService.Update(updatedChoice);

            // Assert
            var result = _choiceService.GetChoicesForQuestion(11)
                .FirstOrDefault(c => c.ChoiceLetter == 'A');

            Assert.NotNull(result);
            Assert.Equal("Zmieniona Jesień.", result!.ChoiceContent);
        }

        // 9
        [Fact] // 
        public void Update_ShouldReplaceLetterAndKeepContent_WhenChoiceExists() // Aktualizuje: aktualizuje literę wyboru, jeśli wybór istnieje
        {
            // Arrange
            _choiceService.Clear();
            var originalChoice = new Choice(12, 'A', "Kraków") { IsActive = true };
            _choiceService.Add(originalChoice);

            // Act
            _choiceService.UpdateChoiceLetter(12, 'B');

            // Assert
            var choices = _choiceService.GetChoicesForQuestion(12).ToList();

            // Sprawdzam, że nowa litera istnieje
            var updatedChoice = choices.FirstOrDefault(c => c.ChoiceLetter == 'B');
            Assert.NotNull(updatedChoice);
            Assert.Equal("Kraków", updatedChoice!.ChoiceContent);

            // Sprawdzam, że stara litera została usunięta
            Assert.DoesNotContain(choices, c => c.ChoiceLetter == 'A');
        }

        // 10
        [Fact] // 
        public void Update_ShouldNotModifyChoices_WhenChoiceDoesNotExist() // Aktualizacja: brak zmiany, jeśli dany wybór nie istnieje. Sprawdzam po Id wyboru.
        {
            // Arrange
            _choiceService.Clear(); 

            var initialCount = _choiceService.GetAllActive().Count();

            var nonExistingChoice = new Choice(99, 'A', "Nieistniejąca opcja.");

            // Act
            _choiceService.Update(nonExistingChoice);

            // Assert
            var allChoices = _choiceService.GetAllActive();

            // Lista nie powinna się zmienić
            Assert.Equal(initialCount, allChoices.Count());

            // Sprawdzam, czy ten konkretny wybor nie istnieje
            Assert.DoesNotContain(allChoices, c => c.ChoiceId == 99 && c.ChoiceLetter == 'A');
        }

        // 11
        [Fact] // 
        public void Update_ShouldNotChangeChoice_WhenContentIsSame() // Aktualizuje: nic nie robi, jeśli treść wyboru pozostaje ta sama
        {
            // Arrange
            _choiceService.Clear();

            var existingChoice = new Choice(15, 'C', "Wąż z Afryki");
            _choiceService.Add(existingChoice);

            var sameContentChoice = new Choice(15, 'C', "Wąż z Afryki");

            // Act
            _choiceService.Update(sameContentChoice);

            // Assert
            var updated = _choiceService.GetChoicesForQuestion(15).First(c => c.ChoiceLetter == 'C');
            Assert.Equal("Wąż z Afryki", updated.ChoiceContent);
            Assert.Single(_choiceService.GetChoicesForQuestion(15)); // Upewniam się, że nie utworzono duplikatu
        }

        //
        [Fact] //
        public void UpdateChoiceLetter_ShouldDoNothing_WhenNewLetterIsTheSame() // Aktualizuje: nic nie robi - litera ta sama
        {
            // Arrange
            _choiceService.Clear();

            var originalChoice = new Choice(20, 'B', "Afryka") { IsActive = true };
            _choiceService.Add(originalChoice);

            var countChoicesBefore = _choiceService.GetChoicesForQuestion(20).Count();

            // Act – próbuję zaktualizować literę na tę samą
            _choiceService.UpdateChoiceLetter(20, 'B');

            // Assert
            var choicesAfterUpdate = _choiceService.GetChoicesForQuestion(20).ToList();

            // Nie powinno być zmian w ilości
            Assert.Equal(countChoicesBefore, choicesAfterUpdate.Count);

            // Powinien nadal istnieć tylko jeden wybór z literą 'B'
            Assert.Single(choicesAfterUpdate, c => c.ChoiceLetter == 'B' && c.ChoiceContent == "Afryka");
        }

        //
        [Fact] //
        public void Update_ShouldNotThrow_WhenChoiceIsNull() // Aktualizuje: nic się nie dzieje - jeśli choice = null
        {
            // Act
            var exception = Record.Exception(() => _choiceService.Update(null));

            // Assert
            Assert.Null(exception); 
        }

        // 12
        [Theory] // 
        [InlineData('Z')]
        [InlineData('1')]
        [InlineData('$')] 
        public void ChoiceConstructor_Update_ShouldThrowArgumentException_WhenChoiceHasInvalidSign(char invalidLetter) // Aktualizuje: wyrzuca wyjątek, jeśli użytkownik poda niepoprawny znak
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Choice(99, invalidLetter, "Nieistniejąca opcja", true));
            Assert.Equal("Niepoprawny znak. Litera odpowiedzi musi być w zakresie A-C.", ex.Message);
        }

        // 13
        [Theory] // 
        [InlineData(99, 'A', "Opcja A", true)]
        [InlineData(100, 'B', "Opcja B", true)]
        [InlineData(101, 'C', "Opcja C", true)]
        public void ChoiceConstructor_Update_ShouldNotThrow_WhenChoiceHasValidLetter(int choiceId, char choiceLetter, string choiceContent, bool isActive) // Aktualizuje: przyjmuje wybór, jeśli system otrzymuje prawidłową literę z zakresu A-C
        {
            // Act
            var exception = Record.Exception(() => new Choice(choiceId, choiceLetter, choiceContent, isActive));

            // Assert
            Assert.Null(exception); // Konstruktor nie powinien rzucać wyjątku dla prawidłowych danych
        }
    }
}
