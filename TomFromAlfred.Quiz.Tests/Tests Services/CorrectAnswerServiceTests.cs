using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    // Oblane: 0 / 11

    public class CorrectAnswerServiceTests
    {
        #region Add CorrectA.ServiceTests
        // 1 
        [Fact] // Zaliczony
        public void Add_ShouldAddCorrectAnswer_WhenValidEntityIsGiven() // Dodaje: poprawną odpowiedź, jeśli entity jest poprawnie podane
        {
            // Arrange
            var correctAnswerService = new CorrectAnswerService();

            var correctAnswerToAdd = new CorrectAnswer(14, "Zima", true);

            // Act
            correctAnswerService.Add(correctAnswerToAdd);

            var result = correctAnswerService.GetAllActive();

            // Assert
            result.Should().ContainSingle(c => c.CorrectAnswerId == 14 && c.CorrectAnswerContent == "Zima" && c.IsActive);
        }

        // 2 
        [Fact] // Zaliczony
        public void Add_ShouldNotAddDuplicateCorrectAnswer_WhenSameIdIsGiven() // Dodaje: nie dodaje duplikatów, kiedy Id jest takie samo
        {
            // Arrange
            var correctAnswerService = new CorrectAnswerService();

            var originalAnswer = new CorrectAnswer(11, "Jesień", true);

            correctAnswerService.Add(originalAnswer);

            var countCorrectAnswerBefore = correctAnswerService.GetAllActive().Count();

            // Act
            correctAnswerService.Add(new CorrectAnswer(11, "Jesień", true));

            var result = correctAnswerService.GetAllActive();

            // Assert
            result.Should().HaveCount(countCorrectAnswerBefore, "Ponieważ duplikat odpowiedzi nie powinien zostać dodany.");

            result.Should().ContainSingle(c => c.CorrectAnswerId == 11 && c.CorrectAnswerContent == "Jesień" && c.IsActive);
        }
        #endregion Add CorrectA.ServiceTests

        #region Delete CorrectA.ServiceTests
        // 3 
        [Fact] // Zaliczony
            public void Delete_ShouldRemoveCorrectAnswerById_WhenValidEntityIsGiven() // Usuwa: poprawną odpowiedź, jeśli poprawnie podane jest entity
            {
                // Arrange
                var correctAnswerService = new CorrectAnswerService();

                var correctAnswerToDelete = new CorrectAnswer(12, "Warszawa", true);

                // Nowa instancja z tym samym CorrectAnswerId
                var sameIdCorrectAnswer = new CorrectAnswer(12, "Zamość", true);

                // Act
                correctAnswerService.Delete(sameIdCorrectAnswer);

                // Assert
                var result = correctAnswerService.GetCorrectAnswerForQuestion(12);

                result.Should().BeNull("Ponieważ odpowiedź o Id 12 została usunięta niezależnie od treści.");
            }

            // 4 
            [Fact] // Zaliczony
            public void Delete_ShouldNotRemoveCorrectAnswer_WhenEntityDoesNotExist() // Usuwa: nic, jeżeli entity nie istnieje
            {
                // Arrange
                var correctAnswerService = new CorrectAnswerService();

                var initialCount = correctAnswerService.GetAllActive().Count();

                var nonExistingCorrectAnswer = new CorrectAnswer(999, "Brak", true);

                // Act
                correctAnswerService.Delete(nonExistingCorrectAnswer);

                var result = correctAnswerService.GetAllActive().Count();

                // Assert - Liczba odpowiedzi nie powinna się zmienić
                correctAnswerService.GetAllActive()
                .Should().HaveCount(initialCount, "Ponieważ nieistniejąca odpowiedź nie powinna nic usuwać.");
        }
            #endregion Delete CorrectA.ServiceTests

        #region FindCorrectA. GetAll CorrectA.ServiceTests
            // 5
            [Fact] // Zaliczony
            public void FindCorrectAnswerContent_ShouldReturnErrorMessage_WhenCorrectAnswerNotFound_OrIsNull_InCAService() // Znajduje: Jeśli dane w serwisie poprawności: są null lub nie zostały znalezione - daje komunikat
            {
                // Arrange
                var correctAnswerService = new CorrectAnswerService();

                // Act
                var result = correctAnswerService.FindCorrectAnswerContent(11, 'A');

                // Assert
                result.Should().Be("Nieznana odpowiedź.");
            }

            // 6
            [Fact] // Zaliczony
            public void GetAll_ShouldReturnAllCorrectActiveAnswers() // Podaje: wszystkie poprawne aktywne odpowiedzi
            {
                // Arrange
                var correctAnswerService = new CorrectAnswerService();

                correctAnswerService.Add(new CorrectAnswer(1, "Wiosna", true));

                correctAnswerService.Add(new CorrectAnswer(2, "Lato", true));

                correctAnswerService.Add(new CorrectAnswer(3, "Jesień", false)); // nieaktywna

                // Act
                var result = correctAnswerService.GetAllActive().ToList();

                // Assert
                result.Should().HaveCount(2, "Bo tylko 2 poprawne odpowiedzi są aktywne.");

                result.Should().OnlyContain(c => c.IsActive, "GetAllActive() powinno zwracać tylko aktywne odpowiedzi.");

                result.Select(c => c.CorrectAnswerContent)
                      .Should().Contain(new[] { "Wiosna", "Lato" });
        }
            #endregion FindCorrectA. GetAll CorrectA.ServiceTests

        #region Update CorrectA.ServiceTests
            // 7 
            [Fact] // Zaliczony
            public void Update_ShouldUpdateCorrectAnswer_WhenValidEntityIsGiven() // Aktualizuje: poprawną odpowiedź jeśli entity jest poprawnie podane
            {
                // Arrange
                var correctAnswerService = new CorrectAnswerService();

                var correctAnswerToUpdate = new CorrectAnswer(11, "Zmieniona zima", true);

                // Act
                correctAnswerService.Update(correctAnswerToUpdate);

                var result = correctAnswerService.GetCorrectAnswerForQuestion(11);

                // Assert
                result.Should().NotBeNull();

                result!.CorrectAnswerContent.Should().Be("Zmieniona zima");
            }

            // 8
            [Fact] // Zaliczony
            public void Update_ShouldNotUpdateCorrectAnswer_WhenEntityDoesNotExist() // Aktualizuje: nic nie robi, jeśli entity nie istnieje
            {
                // Arrange
                var correctAnswerService = new CorrectAnswerService();

                var nonExistingCorrectAnswer = new CorrectAnswer(999, "Brak", true);

                // Act
                correctAnswerService.Update(nonExistingCorrectAnswer);

                var result = correctAnswerService.GetCorrectAnswerForQuestion(999);

                // Assert
                result.Should().BeNull();
            }

            // 9
            [Fact] // Zaliczony
            public void Update_ShouldNotChangeCorrectAnswerById_WhenContentIsSame() // Aktualizuje: pomija aktualizację, jeśli treść ta sama dla tego samego Id
            {
                // Arrange
                var correctAnswerService = new CorrectAnswerService();

                var existingAnswer = new CorrectAnswer(11, "Jesień", true);

                correctAnswerService.Add(existingAnswer);

                // Act
                using (var sw = new StringWriter())
                {
                    Console.SetOut(sw);

                    correctAnswerService.Update(existingAnswer);

                    // Pobieram komunikaty z konsoli
                    var logOutput = sw.ToString();
                    Assert.Contains("Brak zmian: treść dla Id 11 jest już taka sama.", logOutput);
                }

                // Assert
                var result = correctAnswerService.GetCorrectAnswerForQuestion(11);

                result.Should().NotBeNull();

                result!.CorrectAnswerContent.Should().Be("Jesień");
            }
            #endregion Update CorrectA.ServiceTests

        #region GetCorrectAforQuestion CorrectA.ServiceTests
            // 10
            [Fact] // Zaliczony
            public void GetCorrectAnswerForQuestion_ShouldReturnCorrectAnswer_WhenValidQuestionIdIsGiven() // Daje: poprawną odpowiedź, jeśli Id pytania jest poprawne
            {
                //Arrange
                var correctAnswerService = new CorrectAnswerService();

                // Act
                var result = correctAnswerService.GetCorrectAnswerForQuestion(12);

                // Assert
                result.Should().NotBeNull();

                result!.CorrectAnswerContent.Should().Be("Warszawa");
            }

            // 11
            [Fact] // Zaliczony
            public void GetCorrectAnswerForQuestion_ShouldReturnNull_WhenInvalidQuestionIdIsGiven() // Daje: zwrot nulla, jeśli Id pytania jest niepoprawne
            {
                // Arrange
                var correctAnswerService = new CorrectAnswerService();

                // Act
                var result = correctAnswerService.GetCorrectAnswerForQuestion(999);

                // Assert
                result.Should().BeNull();                
            }
            #endregion GetCorrectAforQuestion CorrectA.ServiceTests
    }
}
