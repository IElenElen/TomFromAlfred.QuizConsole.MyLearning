using FluentAssertions;
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
    // Ilość oblanych: 0 / 22

    public class ChoiceServiceTests
    {
        #region Add ChoiceServiceTests
        // 1 
        [Theory] // Zaliczony
        [InlineData('A', "Opcja A")]
        [InlineData('B', "Opcja B")]
        [InlineData('C', "Opcja C")]

        public void Add_ShouldAddNewChoiceSet_WhenChoiceSetDoesNotExist(char letter, string content) // Dodaje: ma dodać nowy zestaw wyboru, jeśli taki zestaw nie istnieje
        {
            // Arrange
            var choiceService = new ChoiceService();

            var newChoice = new Choice(22, letter, content);

            // Act
            choiceService.Add(newChoice);

            // Assert
            var allChoices = choiceService.GetAllActive();
            allChoices.Should()
                .ContainSingle(c => c.ChoiceId == 22 && c.ChoiceLetter == letter)
                .Which.ChoiceContent.Should().Be(content);
        }

        // 2
        [Fact] // Zaliczony
        public void Add_ShouldNotAdd_WhenChoiceExists_AndKeepChoiceCountUnchanged() // Dodaje: nic nie dodaje, jeśli wybór istnieje, nie zmienia liczby wyborów
        {
            // Arrange
            var choiceService = new ChoiceService();

            var existingChoice = new Choice(25, 'A', "Opcja A");

            choiceService.Add(existingChoice);

            var countChoicesBefore = choiceService.GetAllActive().Count();

            // Act
            choiceService.Add(existingChoice);

            var countChoicesAfter = choiceService.GetAllActive().Count();

            // Assert
            countChoicesAfter.Should().Be(countChoicesBefore,
                $"Ponieważ ponowne dodanie istniejącego wyboru nie powinno zmieniać liczby aktywnych wyborów (Id = {existingChoice.ChoiceId}, Litera = {existingChoice.ChoiceLetter}.)");
        }

        // 3
        [Fact] // Zaliczony
        public void Add_ShouldNotDuplicateChoice_WhenSameIdAndLetterUsed() // Dodaje: nie duplikuje wyboru 
        {
            // Arrange
            var choiceService = new ChoiceService();

            var existingChoice = new Choice(25, 'A', "Opcja A");

            choiceService.Add(existingChoice);

            // Act
            choiceService.Add(existingChoice); // Próba duplikatu

            // Assert
            var duplicates = choiceService.GetAllActive() // Pobranie aktywnych
                .Where(c => c.ChoiceId == 25 && c.ChoiceLetter == 'A')
                .ToList();

            duplicates.Should() // Sprawdzenie, czy jest tylko 1 raz
                .ContainSingle(c => c.ChoiceId == 25 && c.ChoiceLetter == 'A')
                .Which.ChoiceContent.Should().Be("Opcja A");
        }
        #endregion Add ChoiceServiceTests

        #region Delete ChoiceServiceTests
        // 4
        [Fact] // Zaliczony
        public void DeleteChoiceById_ShouldRemoveAllChoicesWithGivenId() // Usuwa: istniejący wybór, po Id
        {
            // Arrange
            var choiceService = new ChoiceService();

            choiceService.Add(new Choice(50, 'A', "Opcja A"));

            choiceService.Add(new Choice(50, 'B', "Opcja B"));

            choiceService.Add(new Choice(50, 'C', "Opcja C"));

            // Act
            choiceService.DeleteChoiceById(50);

            // Assert
            var remainingChoices = choiceService.GetAllActive()
                .Where(c => c.ChoiceId == 50);

            remainingChoices.Should().BeEmpty("Wszystkie wybory z ChoiceId = 50 powinny zostać usunięte.");
        }

        // 5
        [Fact] // Zaliczony
        public void Delete_ShouldRemoveChoice_OnlyWhenIdAndLetterMatch() // Usuwa: wybór po id i literze
        {
            // Arrange
            var choiceService = new ChoiceService();

            choiceService.Add(new Choice(60, 'A', "Opcja A"));

            choiceService.Add(new Choice(60, 'B', "Opcja B"));

            // Act
            choiceService.Delete(new Choice(60, 'A', "Nieistotne."));

            // Assert
            var remainingChoices = choiceService.GetChoicesForQuestion(60).ToList();

            remainingChoices.Should()
                .NotContain(c => c.ChoiceLetter == 'A', "Wybór z literą 'A' powinien zostać usunięty.");

            remainingChoices.Should()
                .Contain(c => c.ChoiceLetter == 'B', "Wybór z literą 'B' powinien pozostać.");
        }

        // 6
        [Fact] // Zaliczony
        public void Delete_ShouldNotRemoveChoice_WhenLetterDoesNotMatch() // Usuwa: nie usuwa, jeśli litera błędna
        {
            // Arrange
            var choiceService = new ChoiceService();

            choiceService.Add(new Choice(70, 'A', "Opcja A."));

            // Act
            choiceService.Delete(new Choice(70, 'B', "Zbędna."));

            // Assert
            var remainingChoices = choiceService.GetChoicesForQuestion(70).ToList();

            remainingChoices.Should()
                .ContainSingle(c => c.ChoiceLetter == 'A', "Tylko wybór z literą 'A' powinien pozostać.");

            remainingChoices.Should()
                .NotContain(c => c.ChoiceLetter == 'B', "Nie powinno być wyboru z literą 'B', bo nie był dodany.");
        }

        // 7
        [Fact] // Zaliczony
        public void Delete_ShouldDoNothing_WhenChoiceDoesNotExist() // Usuwa: nic nie usuwa, jeśli wybór nie istnieje
        {
            // Arrange
            var choiceService = new ChoiceService();

            var initialChoicesCount = choiceService.GetAllActive().Count();

            var nonExistingChoice = new Choice(89, 'A', "Opcja nieistniejąca.");

            // Act
            choiceService.Delete(nonExistingChoice);

            // Assert
            var allActiveChoices = choiceService.GetAllActive();

            allActiveChoices.Count().Should().Be(initialChoicesCount,
                "Ponieważ usuwanie nieistniejącego wyboru nie powinno wpływać na liczbę aktywnych wyborów.");
        }

        // 8
        [Fact] // Zaliczony
        public void Delete_ShouldNotThrow_WhenChoiceIsNull() // Usuwa: nic nie robi, jeśli choice to null
        {
            // Arrange
            var choiceService = new ChoiceService();

            // Act
            var exception = Record.Exception(() => choiceService.Delete(null));

            // Assert
            exception.Should().BeNull("Usunięcie `null` nie powinno rzucać wyjątku.");
        }
        #endregion Delete ChoiceServiceTests

        #region GetAllActive ChoiceServiceTests
        // 9
        [Fact] // Zaliczony
        public void GetAllActive_ShouldReturnOnlyChoicesWithIsActiveTrue() // Pobiera: wszystkie aktywne wybory
        {
            // Arrange - Wypełniam listę aktywnymi wyborami
            var choiceService = new ChoiceService();

            choiceService.Add(new Choice(40, 'A', "Jesień") { IsActive = true });
            choiceService.Add(new Choice(40, 'B', "Zima") { IsActive = true });
            choiceService.Add(new Choice(40, 'C', "Wiosna") { IsActive = true });
            choiceService.Add(new Choice(41, 'A', "Kraków") { IsActive = true });
            choiceService.Add(new Choice(41, 'B', "Warszawa") { IsActive = true });
            choiceService.Add(new Choice(41, 'C', "Poznań") { IsActive = true });
            choiceService.Add(new Choice(42, 'A', "Jabłko") { IsActive = true });
            choiceService.Add(new Choice(42, 'B', "Gruszka") { IsActive = true });

            // Act
            var activeChoices = choiceService.GetAllActive();

            // Assert
            activeChoices.Should().HaveCount(8, "8, bo wszystkie dodane wybory były aktywne.");
            activeChoices.Should().OnlyContain(c => c.IsActive, "Metoda powinna zwracać tylko aktywne wybory.");
        }

        // 10
        [Fact] // Zaliczony
        public void GetAllActive_ShouldNotReturnInactiveChoices() // Pobiera: nie pobiera wyborów, które są nieaktywne
        {
            // Arrange
            var choiceService = new ChoiceService();

            choiceService.Add(new Choice(50, 'A', "Aktywny") { IsActive = true });
            choiceService.Add(new Choice(50, 'B', "Nieaktywny") { IsActive = false });

            // Act
            var activeChoices = choiceService.GetAllActive();

            // Assert
            activeChoices.Should().ContainSingle(c => c.ChoiceLetter == 'A' && c.IsActive,
                          "Bo tylko jedna aktywna odpowiedź została dodana.");

            activeChoices.Should().NotContain(c => c.IsActive == false,
                          "Bo GetAllActive() powinno pomijać nieaktywne odpowiedzi.");
        }
        #endregion GetAllActive ChoiceServiceTests

        #region GetChoicesForQuestion ChoiceServiceTests
        // 11
        [Fact] // Zaliczony
        public void GetChoicesForQuestion_ShouldReturnThreeChoicesInSet_WhenQuestionIdExists() // Podaje: 3 wybory w zestawie dla pytania o istniejacym Id
        {
            // Arrange
            var choiceService = new ChoiceService();

            choiceService.Add(new Choice(11, 'A', "Testowe A"));
            choiceService.Add(new Choice(11, 'B', "Testowe B"));
            choiceService.Add(new Choice(11, 'C', "Testowe C"));

            // Act
            var result = choiceService.GetChoicesForQuestion(11);

            // Assert
            result.Should().HaveCount(3, "Dla pytania 11 powinny istnieć 3 przypisane odpowiedzi.");
        }

        // 12
        [Fact] // Zaliczony
        public void GetChoicesForQuestion_ShouldReturnOnlyChoices_Set_WithGivenQuestionId() // Podaje: zestaw dla pytania o danym Id
        {
            var choiceService = new ChoiceService();

            var result = choiceService.GetChoicesForQuestion(11);

            result.Should().OnlyContain(c => c.ChoiceId == 11, "Bo chcę tylko odpowiedzi dla pytania nr 11.");
        }

        // 13
        [Theory] // Zaliczony
        [InlineData('A', "Testowe A")]
        [InlineData('B', "Testowe B")]
        [InlineData('C', "Testowe C")]
        public void GetChoicesForQuestion_ShouldContainExpectedChoices(char letter, string content) // Podaje: przypisany zestaw = literę i treść
        {
            // Arrange
            var choiceService = new ChoiceService();

            choiceService.Add(new Choice(11, 'A', "Testowe A"));
            choiceService.Add(new Choice(11, 'B', "Testowe B"));
            choiceService.Add(new Choice(11, 'C', "Testowe C"));

            var result = choiceService.GetChoicesForQuestion(11);

            result.Should().Contain(c => c.ChoiceLetter == letter && c.ChoiceContent == content,
            $"Bo odpowiedź z literą '{letter}' i treścią '{content}' powinna się znajdować w zestawie.");
        }

        // 14
        [Fact] // Zaliczony
        public void GetChoicesForQuestion_ShouldReturnNothing_WhenQuestionIdNotFound() // Podaje: nie podaje wyborów, bo pytanie o danym Id nie istnieje
        {
            // Arrange - brak danych
            var choiceService = new ChoiceService();

            // Act
            var choicesForQuestion = choiceService.GetChoicesForQuestion(100);

            // Assert
            choicesForQuestion.Should().NotBeNull("Metoda nie powinna zwracać null, nawet jeśli brak wyników.");
            choicesForQuestion.Should().BeEmpty("Bo pytanie o Id 100 nie istnieje.");
        }
        #endregion GetChoicesForQuestion ChoiceServiceTests

        #region Update ChoiceServiceTests
        // 15
        [Fact] // Zaliczony
        public void Update_ShouldUpdateChoiceContent_WhenChoiceExists() // Aktualizuje: zmienia treść wyboru, jeśli wybór istnieje
        {
            // Arrange
            var choiceService = new ChoiceService();

            var originalChoice = new Choice(11, 'A', "Jesień.");

            choiceService.Add(originalChoice);

            var updatedChoice = new Choice(11, 'A', "Zmieniona Jesień.");

            // Act
            choiceService.Update(updatedChoice);

            // Assert
            var result = choiceService.GetChoicesForQuestion(11)
            .FirstOrDefault(c => c.ChoiceLetter == 'A');

            result.Should().NotBeNull();

            result!.ChoiceContent.Should().Be("Zmieniona Jesień.");
        }

        // 16
        [Fact] // Zaliczony
        public void Update_ShouldReplaceLetterAndKeepContent_WhenChoiceExists() // Aktualizuje: aktualizuje literę wyboru, jeśli wybór istnieje
        {
            // Arrange
            var choiceService = new ChoiceService();

            var originalChoice = new Choice(12, 'A', "Kraków") { IsActive = true };

            choiceService.Add(originalChoice);

            // Act
            choiceService.UpdateChoiceLetter(12, 'B');

            // Assert
            var choices = choiceService.GetChoicesForQuestion(12).ToList();

            choices.Should().ContainSingle(c => c.ChoiceLetter == 'B')
                .Which.ChoiceContent.Should().Be("Kraków");

            choices.Should().NotContain(c => c.ChoiceLetter == 'A');
        }

        // 17
        [Fact] // Zaliczony
        public void Update_ShouldNotModifyChoices_WhenChoiceDoesNotExist() // Aktualizacja: brak zmiany, jeśli dany wybór nie istnieje. Sprawdzam po Id wyboru.
        {
            // Arrange
            var choiceService = new ChoiceService();

            var initialChoiceCount = choiceService.GetAllActive().Count();

            var nonExistingChoice = new Choice(99, 'A', "Nieistniejąca opcja.");

            // Act
            choiceService.Update(nonExistingChoice);

            // Assert
            var allChoices = choiceService.GetAllActive();

            allChoices.Count().Should().Be(initialChoiceCount);

            allChoices.Should().NotContain(c => c.ChoiceId == 99 && c.ChoiceLetter == 'A');
        }

        // 18
        [Fact] // Zaliczony
        public void Update_ShouldNotChangeChoice_WhenContentIsSame() // Aktualizuje: nic nie robi, jeśli treść wyboru pozostaje ta sama
        {
            // Arrange
            var choiceService = new ChoiceService();

            var existingChoice = new Choice(15, 'C', "Wąż z Afryki.");

            choiceService.Add(existingChoice);

            var sameContentChoice = new Choice(15, 'C', "Wąż z Afryki.");

            // Act
            choiceService.Update(sameContentChoice);

            // Assert
            var updatedChoice = choiceService.GetChoicesForQuestion(15).First(c => c.ChoiceLetter == 'C');

            updatedChoice.ChoiceContent.Should().Be("Wąż z Afryki.");

            choiceService.GetChoicesForQuestion(15).Should().ContainSingle();
        }

        // 19
        [Fact] // Zaliczony
        public void UpdateChoiceLetter_ShouldDoNothing_WhenNewLetterIsTheSame() // Aktualizuje: nic nie robi - litera ta sama
        {
            // Arrange
            var choiceService = new ChoiceService();

            var originalChoice = new Choice(20, 'B', "Afryka") { IsActive = true };

            choiceService.Add(originalChoice);

            var countChoicesBefore = choiceService.GetChoicesForQuestion(20).Count();

            // Act – próbuję zaktualizować literę na tę samą
            choiceService.UpdateChoiceLetter(20, 'B');

            // Assert
            var choicesAfterUpdate = choiceService.GetChoicesForQuestion(20);

            choicesAfterUpdate.Should().HaveCount(countChoicesBefore);

            choicesAfterUpdate.Should().ContainSingle(c => c.ChoiceLetter == 'B' && c.ChoiceContent == "Afryka");
        }

        // 20
        [Fact] // Zaliczony
        public void Update_ShouldNotThrow_WhenChoiceIsNull() // Aktualizuje: nic się nie dzieje - jeśli choice = null
        {
            //Arrange
            var choiceService = new ChoiceService();

            // Act
            var exception = Record.Exception(() => choiceService.Update(null));

            // Assert
            exception.Should().BeNull();
        }

        // 21
        [Theory] // Zaliczony
        [InlineData('Z')]
        [InlineData('1')]
        [InlineData('$')] 
        public void ChoiceConstructor_Update_ShouldThrowArgumentException_WhenChoiceHasInvalidSign(char invalidLetter) // Aktualizuje: wyrzuca wyjątek, jeśli użytkownik poda niepoprawny znak
        {
            // Arrange
            var choiceService = new ChoiceService();

            // Act
            var exception = Assert.Throws<ArgumentException>(() => new Choice(99, invalidLetter, "Nieistniejąca opcja", true));

            // Assert
            exception.Message.Should().Be("Niepoprawny znak. Litera odpowiedzi musi być w zakresie A-C.");

        }

        // 22
        [Theory] // Zaliczony
        [InlineData(99, 'A', "Opcja A", true)]
        [InlineData(100, 'B', "Opcja B", true)]
        [InlineData(101, 'C', "Opcja C", true)]
        public void ChoiceConstructor_Update_ShouldNotThrow_WhenChoiceHasValidLetter(int choiceId, char choiceLetter, string choiceContent, bool isActive) // Aktualizuje: przyjmuje wybór, jeśli system otrzymuje prawidłową literę z zakresu A-C
        {
            // Arrange
            var choiceService = new ChoiceService();

            // Act
            var exception = Record.Exception(() => new Choice(choiceId, choiceLetter, choiceContent, isActive));

            // Assert
            exception.Should().BeNull(); // Konstruktor nie powinien rzucać wyjątku dla prawidłowych danych
        }
        #endregion Update ChoiceServiceTests
    }
}
