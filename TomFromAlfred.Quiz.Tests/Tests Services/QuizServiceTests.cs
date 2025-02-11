using Moq;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract;
using System.Reflection;


namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    // Sporo tu roboty

    // Oblane: 36 / 36
    // Wybieram zamianę metod w klasie na wirtualne

    // Przetestować kolejne metody
    public class QuizServiceTests
    {
        private readonly Mock<QuestionService> _mockQuestionService;
        private readonly Mock<ChoiceService> _mockChoiceService;
        private readonly Mock<CorrectAnswerService> _mockCorrectAnswerService;
        private readonly Mock<JsonCommonClass> _mockJsonCommonClass;
        private readonly QuizService _quizService;


        public QuizServiceTests()
        {
            // Tworzę mocki dla klas serwisowych
            _mockQuestionService = new Mock<QuestionService>();
            _mockChoiceService = new Mock<ChoiceService>();
            _mockCorrectAnswerService = new Mock<CorrectAnswerService>();
            _mockJsonCommonClass = new Mock<JsonCommonClass>();

            // Inicjalizacja QuizService z zamockowanymi obiektami
            _quizService = new QuizService(
                _mockQuestionService.Object,
                _mockChoiceService.Object,
                _mockCorrectAnswerService.Object,
                _mockJsonCommonClass.Object
            );
        }

        // 1
        [Fact] // Oblany
        public void InitializeJsonService_ShouldNotThrowException_WhenValidArgumentsArePassed() // Nie wyrzuca wyjątku jeśli ścieżka poprawna
        {
            // Arrange
            var mockJsonService = new Mock<JsonCommonClass>();
            var quizService = new QuizService(
                new Mock<QuestionService>().Object,
                new Mock<ChoiceService>().Object,
                new Mock<CorrectAnswerService>().Object,
                mockJsonService.Object
            );

            string validFilePath = "validFilePath.json"; // Symuluję poprawną ścieżkę

            // Act & Assert
            var exception = Record.Exception(() => quizService.InitializeJsonService(mockJsonService.Object, validFilePath));

            Assert.Null(exception); // Sprawdzam, czy nie wystąpił żaden wyjątek
        }

        // 2
        [Fact] // Oblany
        public void InitializeJsonService_ShouldThrowArgumentNullException_WhenJsonServiceIsNull() // Nie wyrzuca wyjątku, jeśli json jest null
        {
            // Arrange
            var quizService = new QuizService(
                new Mock<QuestionService>().Object,
                new Mock<ChoiceService>().Object,
                new Mock<CorrectAnswerService>().Object,
                new Mock<JsonCommonClass>().Object
            );

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => quizService.InitializeJsonService(null, "validFilePath.json"));
        }

        // 3 
        [Fact] // Oblany
        public void InitializeJsonService_ShouldThrowFileNotFoundException_WhenFileDoesNotExist() // Nie wyrzuca wyjątku, jesli plik nie istnieje
        {
            // Arrange
            var mockJsonService = new Mock<JsonCommonClass>();
            var quizService = new QuizService(
                new Mock<QuestionService>().Object,
                new Mock<ChoiceService>().Object,
                new Mock<CorrectAnswerService>().Object,
                mockJsonService.Object
            );

            // Symuluję ścieżkę do nieistniejącego pliku
            string noExistingFilePath = "nonExistentFile.json";

            // Act & Assert
            var exception = Assert.Throws<FileNotFoundException>(() => quizService.InitializeJsonService(mockJsonService.Object, noExistingFilePath));
            Assert.Equal($"Plik {noExistingFilePath} nie istnieje.", exception.Message);
        }

        // 4
        [Fact] // Oblany
        public void GetAllQuestions_ShouldReturnQuestionsFromJsonAndService() // Zwraca pytania z serwisu i pliku json
        {
            // Arrange
            var mockQuestionsFromJson = new List<Question>
        {
            new Question (1, "Pytanie 1" ),
            new Question (2, "Pytanie 2")
        };
            var mockQuestionsFromService = new List<Question>
        {
            new Question (3, "Pytanie 3")
        };

            _mockJsonCommonClass.Setup(s => s.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestionsFromJson);
            _mockQuestionService.Setup(s => s.GetAllActive())
                .Returns(mockQuestionsFromService);

            // Act
            var result = _quizService.GetAllQuestions().ToList();

            // Assert
            Assert.Equal(3, result.Count);  // 2 z JSON + 1 z serwisu
            Assert.Contains(result, q => q.QuestionId == 1);
            Assert.Contains(result, q => q.QuestionId == 2);
            Assert.Contains(result, q => q.QuestionId == 3);
        }

        // 5
        [Fact] // Oblany
        public void GetAllQuestions_ShouldReturnQuestionsFromServiceWhenJsonIsEmpty() // Zwraca pytania z serwisu, jesli json pusty
        {
            // Arrange
            var mockQuestionsFromJson = new List<Question>(); // Puste pytania JSON
            var mockQuestionsFromService = new List<Question>
            {
            new Question (1, "Pytanie 1")
            };

            _mockJsonCommonClass.Setup(s => s.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestionsFromJson);
            _mockQuestionService.Setup(s => s.GetAllActive())
                .Returns(mockQuestionsFromService);

            // Act
            var result = _quizService.GetAllQuestions().ToList();

            // Assert
            Assert.Single(result);  // Powinno być tylko pytanie z serwisu
            Assert.Contains(result, q => q.QuestionId == 1);
        }

        // 6
        [Fact] // Oblany
        public void GetAllQuestions_ShouldReturnQuestionsFromJsonWhenServiceIsEmpty() // Zwraca pytania z json, jeśli serwis pusty
        {
            // Arrange
            var mockQuestionsFromJson = new List<Question>
            {
                new Question (1, "Pytanie 1")
            };
            var mockQuestionsFromService = new List<Question>(); // Puste pytania z serwisu

            _mockJsonCommonClass.Setup(s => s.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestionsFromJson);
            _mockQuestionService.Setup(s => s.GetAllActive())
                .Returns(mockQuestionsFromService);

            // Act
            var result = _quizService.GetAllQuestions().ToList();

            // Assert
            Assert.Single(result);  // Powinno być tylko pytanie z JSON
            Assert.Contains(result, q => q.QuestionId == 1);
        }

        // 7
        [Fact] // Oblany
        public void GetChoicesForQuestion_ShouldReturnChoicesFromJson_WhenChoicesExistInJson() // Zwraca wybory z jsona, jeśli wybory te istnieją
        {
            // Arrange
            int questionId = 1;
            var expectedChoices = new List<Choice>
            {
                new Choice (1, 'A', "Answer A" ),
                new Choice (2, 'B', "Answer B" )
            };

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Choice>>(It.IsAny<string>()))
                            .Returns(expectedChoices);

            // Ustawienie: w JSON są już odpowiedzi dla tego pytania
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Choice>>(It.Is<string>(s => s.Contains("choices"))))
                            .Returns(expectedChoices);

            // Act
            var choices = _quizService.GetChoicesForQuestion(questionId).ToList();

            // Assert
            Assert.Equal(2, choices.Count);
            Assert.Equal('A', choices[0].ChoiceLetter);
            Assert.Equal("Answer A", choices[0].ChoiceContent);
            Assert.Equal('B', choices[1].ChoiceLetter);
            Assert.Equal("Answer B", choices[1].ChoiceContent);
        }

        // 8
        [Fact] // Oblany
        public void GetChoicesForQuestion_ShouldReturnChoicesFromService_WhenChoicesNotExistInJson() // Daje wybory z serwisu, jeśli w json ich brak
        {
            // Arrange
            int questionId = 2;
            var serviceChoices = new List<Choice>
            {
                new Choice (3, 'C', "Wybór C"),
                new Choice (4, 'D', "Wybór D")
            };

            // Mockuję metodę GetChoicesForQuestion w ChoiceService, by zwróciła odpowiedzi z serwisu
            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(serviceChoices);

            // Act
            var choices = _quizService.GetChoicesForQuestion(questionId).ToList();

            // Assert
            Assert.Equal(2, choices.Count);
            Assert.Equal('C', choices[0].ChoiceLetter);
            Assert.Equal("Wybór C", choices[0].ChoiceContent);
            Assert.Equal('D', choices[1].ChoiceLetter);
            Assert.Equal("Wybór D", choices[1].ChoiceContent);
        }

        // 9
        [Fact] // Oblany
        public void GetChoicesForQuestion_ShouldReturnEmpty_WhenNoChoicesFound() // Zwróci pustkę, jeśli wyborów nie znalazł
        {
            // Arrange
            int questionId = 3;

            // Mockuję brak odpowiedzi w JSON
            _mockJsonCommonClass.Setup(service => service.ReadFromFile<List<Choice>>(It.IsAny<string>()))
                            .Returns(new List<Choice>());

            // Mockuję brak odpowiedzi w ChoiceService
            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(Enumerable.Empty<Choice>());

            // Act
            var choices = _quizService.GetChoicesForQuestion(questionId).ToList();

            // Assert
            Assert.Empty(choices);
        }

        // 10 
        [Fact] // Oblany
        public void GetShuffledChoicesForQuestion_ShouldShuffleChoicesAndMapLetters() // Losuje wybory i mapuje litery
        {
            // Arrange
            int questionId = 1;

            var originalChoices = new List<Choice>
            {
                new Choice (1, 'A', "Answer A"),
                new Choice (2, 'B', "Answer B"),
                new Choice (3, 'C', "Answer C")
            };

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(originalChoices);

            // Act
            Dictionary<char, char> letterMapping;
            var shuffledChoices = _quizService.GetShuffledChoicesForQuestion(questionId, out letterMapping).ToList();

            // Assert
            Assert.Equal(originalChoices.Count, shuffledChoices.Count); 
            Assert.Contains(letterMapping.Keys, key => key == 'A' || key == 'B' || key == 'C'); 

            // Upewniam się, że mapowanie jest poprawne
            foreach (var choice in shuffledChoices)
            {
                Assert.Contains(letterMapping, mapping => mapping.Value == choice.ChoiceLetter);
            }

            Assert.NotEqual(originalChoices[0].ChoiceLetter, shuffledChoices[0].ChoiceLetter);
            Assert.NotEqual(originalChoices[1].ChoiceLetter, shuffledChoices[1].ChoiceLetter);
            Assert.NotEqual(originalChoices[2].ChoiceLetter, shuffledChoices[2].ChoiceLetter);
        }

        // 11
        [Fact] // Oblany
        public void GetShuffledChoicesForQuestion_ShouldReturnSameNumberOfChoices() // Przy losowaniu Quizu, zawsze zwraca tę samą liczbę wyborów w zestawie
        {
            // Arrange
            int questionId = 2;
            var originalChoices = new List<Choice>
            {
                new Choice (1, 'A', "Wybór A"),
                new Choice (1, 'B', "Wybór B")
            };

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(originalChoices);

            // Act
            Dictionary<char, char> letterMapping;
            var shuffledChoices = _quizService.GetShuffledChoicesForQuestion(questionId, out letterMapping).ToList();

            // Assert
            Assert.Equal(originalChoices.Count, shuffledChoices.Count); 
        }

        // 12
        [Fact] // Oblany
        public void GetShuffledChoicesForQuestion_ShouldCreateCorrectLetterMapping() // Tworzy poprawną mapę liter
        {
            // Arrange
            int questionId = 3;
            var originalChoices = new List<Choice>
            {
                new Choice (3, 'A', "Opcja A"),
                new Choice (3, 'B', "Opcja B"),
                new Choice (3, 'C', "Opcja C")
            };

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(originalChoices);

            // Act
            Dictionary<char, char> letterMapping;
            var shuffledChoices = _quizService.GetShuffledChoicesForQuestion(questionId, out letterMapping).ToList();

            // Assert
            Assert.Equal(3, letterMapping.Count);
            Assert.Contains(letterMapping, mapping => mapping.Key == 'A');
            Assert.Contains(letterMapping, mapping => mapping.Key == 'B');
            Assert.Contains(letterMapping, mapping => mapping.Key == 'C');
        }

        // 13
        [Fact] // Oblany
        public void CheckAnswer_ShouldReturnTrue_WhenAnswerIsCorrectFromJson() // Zwraca prawdę, jeśli odpowiedź poprawna
        {
            // Arrange
            int questionId = 1;
            char userChoiceLetter = 'B';
            var letterMapping = new Dictionary<char, char> { { 'A', 'C' }, { 'B', 'A' }, { 'C', 'B' } };

            _mockCorrectAnswerService.Setup(service => service.GetCorrectAnswerForQuestion(questionId))
                                      .Returns(new CorrectAnswer (1, "A", true));
            var choices = new List<Choice>
            {
                new Choice (1, 'A', "Answer A"),
                new Choice (2, 'B', "Answer B"),
                new Choice (3, 'C', "Answer C")
            };

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Act
            var result = _quizService.CheckAnswer(questionId, userChoiceLetter, letterMapping);

            // Assert
            Assert.True(result); // Powinno zwróić A
        }

        // 14
        [Fact] // Oblany
        public void CheckAnswer_ShouldReturnTrue_WhenAnswerIsCorrectFromEntity() // Zwraca prawdę, jesli odpowiedź ok z entity ???
        {
            // Arrange
            int questionId = 2;
            char userChoiceLetter = 'B';
            var letterMapping = new Dictionary<char, char> { { 'A', 'C' }, { 'B', 'A' }, { 'C', 'B' } };

            
            var correctAnswerFromService = new CorrectAnswer (2, "Answer A", true);
            _mockCorrectAnswerService.Setup(service => service.GetCorrectAnswerForQuestion(questionId))
                                      .Returns(correctAnswerFromService);

            
            var choices = new List<Choice>
            {
                new Choice (1, 'A', "Answer A"),
                new Choice (2, 'B', "Answer B"),
                new Choice (3, 'C', "Answer C")
            };

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Act
            var result = _quizService.CheckAnswer(questionId, userChoiceLetter, letterMapping);

            // Assert
            Assert.True(result); 
        }

        // 15 
        [Fact] // 
        public void CheckAnswer_ShouldReturnFalse_WhenAnswerIsIncorrect() // 
        {
            // Arrange
            int questionId = 3;
            char userChoiceLetter = 'C';
            var letterMapping = new Dictionary<char, char> { { 'A', 'C' }, { 'B', 'A' }, { 'C', 'B' } };

            _mockCorrectAnswerService.Setup(service => service.GetCorrectAnswerForQuestion(questionId))
                                                  .Returns(new CorrectAnswer(3, "A", true));
            var choices = new List<Choice>
            {
                new Choice (1, 'A', "Answer A"),
                new Choice (2, 'B', "Answer B"),
                new Choice (3, 'C', "Answer C")
            };

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Act
            var result = _quizService.CheckAnswer(questionId, userChoiceLetter, letterMapping);

            // Assert
            Assert.False(result); 
        }

        // 16
        [Fact]
        public void CheckAnswer_ShouldReturnFalse_WhenLetterMappingIsEmpty() // 
        {
            // Arrange
            int questionId = 1;
            char userChoiceLetter = 'A';
            var emptyLetterMapping = new Dictionary<char, char>(); // Puste mapowanie

            var choices = new List<Choice>
            {
                new Choice (1, 'A', "Answer A"),
                new Choice (2, 'B', "Answer B")
            };

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Act
            var result = _quizService.CheckAnswer(questionId, userChoiceLetter, emptyLetterMapping);

            // Assert
            Assert.False(result); // Ponieważ mapowanie jest puste, odpowiedź jest błędna
        }

        // 17
        [Fact] //
        public void CheckAnswer_ShouldReturnFalse_WhenLetterMappingDoesNotContainUserChoice() // 
        {
            // Arrange
            int questionId = 1;
            char userChoiceLetter = 'D'; // Litera, która nie znajduje się w mapowaniu
            var letterMapping = new Dictionary<char, char> { { 'A', 'B' }, { 'B', 'C' } }; // Brak mapowania dla 'D'

            
            var choices = new List<Choice>
            {
                new Choice (1, 'A', "Answer A"),
                new Choice (2, 'B', "Answer B")
             };

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Act
            var result = _quizService.CheckAnswer(questionId, userChoiceLetter, letterMapping);

            // Assert
            Assert.False(result); // Brak mapowania dla 'D', więc odpowiedź jest błędna
        }

        // 18
        [Fact] // 
        public void CheckAnswer_ShouldReturnFalse_WhenLetterMappingIsIncorrect() // Oblany
        {
            // Arrange
            int questionId = 1;
            char userChoiceLetter = 'B';
            var letterMapping = new Dictionary<char, char> { { 'A', 'C' }, { 'B', 'D' } }; // Błędne mapowanie

            
            var choices = new List<Choice>
            {
                new Choice (1, 'A', "Answer A" ),
                new Choice (2, 'B', "Answer B" )
            };

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Act
            var result = _quizService.CheckAnswer(questionId, userChoiceLetter, letterMapping);

            // Assert
            Assert.False(result); // Zły wybór w mapowaniu, odpowiedź powinna być błędna
        }

        // 19
        [Fact]
        public void CheckAnswer_ShouldReturnFalse_WhenChoicesAreEmpty() // Oblany
        {
            // Arrange
            int questionId = 1;
            char userChoiceLetter = 'A';
            var letterMapping = new Dictionary<char, char> { { 'A', 'B' }, { 'B', 'C' } };

            var choices = new List<Choice>(); // Pusta lista odpowiedzi

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Act
            var result = _quizService.CheckAnswer(questionId, userChoiceLetter, letterMapping);

            // Assert
            Assert.False(result); // Brak odpowiedzi, odpowiedź jest błędna
        }

        // 20
        [Fact]
        public void CheckAnswer_ShouldReturnFalse_WhenLetterMappingIsMissing() // Oblany
        {
            // Arrange
            var letterMapping = new Dictionary<char, char>
            {
                {'A', 'X'}, // Mapowanie poprawnej odpowiedzi
                {'B', 'Y'}
            };

            // Użytkownik wybiera literę, której brak w letterMapping
            char userChoiceLetter = 'C';

            _mockCorrectAnswerService.Setup(ca => ca.GetCorrectAnswerForQuestion(It.IsAny<int>()))
                .Returns(new CorrectAnswer(1, "X", true));  // Zwracamy obiekt CorrectAnswer z odpowiednim Id i treścią
            // Act
            var result = _quizService.CheckAnswer(1, userChoiceLetter, letterMapping);

            // Assert
            Assert.False(result); // Spodziewam się, że odpowiedź będzie błędna, ponieważ nie ma mapowania dla 'C'
        }

        // 21
        [Fact]
        public void GetLetterForAnswer_ShouldReturnCorrectLetterForAnswer() // Oblany
        {
            // Arrange
            int questionId = 1;
            var answerContent = "Answer B";

            var choices = new List<Choice>
            {
                new Choice (1, 'A', "Answer A"),
                new Choice (2, 'B', "Answer B"),
                new Choice (3, 'C', "Answer C")
            };

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Act
            var result = _quizService.GetLetterForAnswer(answerContent, questionId);

            // Assert
            Assert.Equal('B', result); 
        }

        // 22
        [Fact]
        public void GetLetterForAnswer_ShouldReturnDefaultLetter_WhenNoMatchFound() // Oblany
        {
            // Arrange
            int questionId = 1;
            var answerContent = "Answer D"; 

            var choices = new List<Choice>
            {
                new Choice (1, 'A', "Answer A"),     
                new Choice (2, 'B', "Answer B"),
                new Choice (3, 'C', "Answer C" )
            };

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Act
            var result = _quizService.GetLetterForAnswer(answerContent, questionId);

            // Assert
            Assert.Equal('?', result); 
        }

        // 23
        [Fact]
        public void LoadQuestionsFromJson_ShouldReturn_WhenFileDoesNotExist() // Oblany
        {
            // Arrange
            string filePath = "nonexistentFile.json";
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(filePath)).Throws(new FileNotFoundException());

            // Act & Assert
            var exception = Record.Exception(() => _quizService.LoadQuestionsFromJson(filePath));
            Assert.Null(exception);  // Sprawdzam, czy nie pojawił się wyjątek
        }

        // 24
        [Fact]
        public void LoadQuestionsFromJson_ShouldThrowJsonException_WhenJsonIsEmpty() // Oblany
        {
            // Arrange
            string filePath = "questions.json";
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(filePath)).Returns(new List<Question>());

            // Act & Assert
            var exception = Assert.Throws<JsonException>(() => _quizService.LoadQuestionsFromJson(filePath));
            Assert.Equal("JSON does not contain any questions", exception.Message);
        }

        // 25
        [Fact]
        public void LoadQuestionsFromJson_ShouldLoadQuestions_WhenJsonIsValid() // Oblany
        {
            // Arrange
            string filePath = "questions.json";
            var questions = new List<Question>
            {
                new Question (1, "What is 2 + 2?" )
            };

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(filePath)).Returns(questions);

            // Act
            _quizService.LoadQuestionsFromJson(filePath);

            // Assert
            Assert.NotEmpty(_quizService.GetAllQuestions()); // Sprawdzam, czy pytania zostały załadowane
        }

        // 26
        [Fact]
        public void LoadChoicesFromJson_ShouldReturn_WhenChoicesFileDoesNotExist() // Oblany
        {
            // Arrange
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Choice>>(It.IsAny<string>())).Throws(new FileNotFoundException());

            // Act & Assert
            var exception = Record.Exception(() => _quizService.LoadChoicesFromJson());
            Assert.Null(exception);  // Sprawdzam, czy nie pojawił się wyjątek
        }

        // 27
        [Fact]
        public void LoadChoicesFromJson_ShouldLoadChoices_WhenFileIsValid() // Oblany
        {
            // Arrange
            var choices = new List<Choice>
            {
                new Choice (1, 'A', "Answer A" ), // Dla pytania 1
                new Choice (1, 'B', "Answer B")  // Dla pytania 1
            };

            // Setup mock dla odczytu pliku
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Choice>>(It.IsAny<string>())).Returns(choices);

            // Act
            _quizService.LoadChoicesFromJson();

            // Assert
            var loadedChoices = _quizService.GetChoicesForQuestion(1).ToList(); // Pobieram wybory dla pytania o Id = 1

            // Sprawdzam, czy zostały załadowane 2 wybory dla pytania 1
            Assert.Equal(2, loadedChoices.Count); // Sprawdzam, czy zostały załadowane 2 wybory

            // Sprawdzam, czy odpowiedzi są zgodne z danymi wejściowymi
            Assert.Contains(loadedChoices, c => c.ChoiceId == 1 && c.ChoiceLetter == 'A' && c.ChoiceContent == "Answer A");
            Assert.Contains(loadedChoices, c => c.ChoiceId == 1 && c.ChoiceLetter == 'B' && c.ChoiceContent == "Answer B");
        }

        // 28
        [Fact]
        public void LoadChoicesFromJson_ShouldLoadAndShuffleChoices_WhenFileIsValid() // Oblany
        {
            // Arrange
            var choices = new List<Choice>
            {
                new Choice(1, ' ', "Choice 1"),
                new Choice(2, ' ', "Choice 2"),
                new Choice(3, ' ', "Choice 3")
            };

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Choice>>(It.IsAny<string>())).Returns(choices);

            // Act
            _quizService.LoadChoicesFromJson();

            // Assert
            var shuffledChoices = _quizService.GetChoicesForQuestion(1).ToList();

            // Sprawdzam, czy wybory zostały przetasowane
            Assert.NotEqual(choices[0].ChoiceLetter, shuffledChoices[0].ChoiceLetter);
            Assert.NotEqual(choices[1].ChoiceLetter, shuffledChoices[1].ChoiceLetter);
            Assert.NotEqual(choices[2].ChoiceLetter, shuffledChoices[2].ChoiceLetter);

            // Sprawdzam, czy litery zostały przypisane poprawnie
            Assert.Equal('A', shuffledChoices[0].ChoiceLetter);
            Assert.Equal('B', shuffledChoices[1].ChoiceLetter);
            Assert.Equal('C', shuffledChoices[2].ChoiceLetter);
        }

        // 29
        [Fact]
        public void LoadCorrectSetFromJson_ShouldReturn_WhenCorrectSetFileDoesNotExist() // Oblany
        {
            // Arrange
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<JsonHelper>>(It.IsAny<string>())).Throws(new FileNotFoundException());

            // Act & Assert
            var exception = Record.Exception(() => _quizService.LoadCorrectSetFromJson());
            Assert.Null(exception);  // Sprawdzam, czy nie pojawił się wyjątek
        }

        // 30
        [Fact]
        public void LoadCorrectSetFromJson_ShouldLoadCorrectAnswers_WhenFileIsValid() // Oblany
        {
            // Arrange
            var correctSet = new List<JsonHelper>
            {
                new JsonHelper { QuestionNumber = 1, LetterCorrectAnswer = "A", ContentCorrectAnswer = "Answer A" }
            };

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<JsonHelper>>(It.IsAny<string>())).Returns(correctSet);

            // Setup mock dla GetCorrectAnswerForQuestion, by zwrócić odpowiedź dla pytania 1
            _mockCorrectAnswerService.Setup(x => x.GetCorrectAnswerForQuestion(1))
                .Returns(new CorrectAnswer(1, "Answer A", true));

            // Act
            _quizService.LoadCorrectSetFromJson();

            // Assert
            var correctAnswer = _mockCorrectAnswerService.Object.GetCorrectAnswerForQuestion(1); // Wywołuję metodę na obiekcie mocka
            Assert.NotNull(correctAnswer); // Sprawdzam, czy odpowiedź została załadowana
            Assert.Equal(1, correctAnswer.CorrectAnswerId); // Sprawdzam, czy Id odpowiedzi to 1
            Assert.Equal("Answer A", correctAnswer.CorrectAnswerContent); // Sprawdzam, czy treść odpowiedzi to "Answer A"
        }

        // 31
        [Fact]
        public void LoadCorrectSetFromJson_ShouldHandleInvalidJson() // Oblany
        {
            // Arrange
            var incorrectSet = new List<JsonHelper>
            {
                new JsonHelper { QuestionNumber = -1, LetterCorrectAnswer = "Z", ContentCorrectAnswer = "" }
            };

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<JsonHelper>>(It.IsAny<string>())).Returns(incorrectSet);

            // Act
            _quizService.LoadCorrectSetFromJson();

            // Assert

            // Testuję, czy po załadowaniu błędnych danych, metoda GetCorrectAnswerForQuestion nie zwróci odpowiedzi dla pytania o id -1
            var correctAnswer = _mockCorrectAnswerService.Object.GetCorrectAnswerForQuestion(-1);
            Assert.Null(correctAnswer); // Oczekuję, że nie będzie poprawnej odpowiedzi dla tego pytania
        }

        // 32
        [Fact]
        public void LoadCorrectSetFromJson_ShouldHandleEmptyCorrectAnswers() // Oblany
        {
            // Arrange
            var emptyCorrectAnswers = new List<JsonHelper> // Pusty zestaw poprawnych odpowiedzi
            {
                // Zawiera dane, ale bez odpowiedzi poprawnych
                new JsonHelper { QuestionNumber = 1, LetterCorrectAnswer = null, ContentCorrectAnswer = null }
            };

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<JsonHelper>>(It.IsAny<string>())).Returns(emptyCorrectAnswers);

            // Setup mock dla GetCorrectAnswerForQuestion, zwracający null
            _mockCorrectAnswerService.Setup(x => x.GetCorrectAnswerForQuestion(It.IsAny<int>())).Returns((CorrectAnswer)null);

            // Act
            _quizService.LoadCorrectSetFromJson();

            // Assert
            var correctAnswer = _mockCorrectAnswerService.Object.GetCorrectAnswerForQuestion(1); // Sprawdzam wywołanie mocka
            Assert.Null(correctAnswer); // Spodziewam się, że nie będzie poprawnej odpowiedzi
        }

        // Próba zastosowania refleksji
        // 33
        [Fact]
        public void IsChoiceRelatedToQuestion_ShouldReturnTrue_WhenChoiceIsRelatedToQuestion() // Oblany
        {
            // Arrange
            var choice = new Choice(15, 'A', "próbny wybór");
            var question = new Question(1, "próbna treść");

            // Używam refleksji, aby uzyskać dostęp do prywatnej metody
            var methodInfo = typeof(QuizService).GetMethod("IsChoiceRelatedToQuestion", BindingFlags.NonPublic | BindingFlags.Static);
            var result = (bool)methodInfo.Invoke(null, new object[] { choice, question });

            // Assert
            Assert.True(result); // Oczekuję, że wynik będzie True
        }

        // 34
        [Fact]
        public void IsChoiceRelatedToQuestion_ShouldReturnFalse_WhenChoiceIsNotRelatedToQuestion() // Oblany
        {
            // Arrange
            var choice = new Choice (25, 'A', "próbny wybór");
            var question = new Question (1, "próbna treść");

            // Używam refleksji, aby uzyskać dostęp do prywatnej metody
            var methodInfo = typeof(QuizService).GetMethod("IsChoiceRelatedToQuestion", BindingFlags.NonPublic | BindingFlags.Static);
            var result = (bool)methodInfo.Invoke(null, new object[] { choice, question });

            // Assert
            Assert.False(result); // Oczekuję, że wynik będzie False
        }

        // Za pomocą publicznej metody
        // 35
        [Fact]
        public void AddQuestionToJson_ShouldAddQuestionAndSaveToFile() // Oblany
        {
            // Arrange
            var question = new Question(1, "próbna treść");

            _mockJsonCommonClass.Setup(x => x.WriteToFile(It.IsAny<string>(), It.IsAny<List<Question>>()));

            // Act
            _quizService.AddQuestionToJson(question);

            // Assert
            _mockJsonCommonClass.Verify(x => x.WriteToFile(It.IsAny<string>(), It.Is<List<Question>>(q => q.Contains(question))), Times.Once);
            // Sprawdzam, czy metoda zapisu została wywołana raz i czy zawiera pytanie
        }

        // Test przez mock
        // 36
        [Fact]
        public void SaveQuestionsToJson_ShouldHandleFileWriteException() // Oblany
        {
            // Arrange
            var question = new Question(1, "próbna treść");

            _mockJsonCommonClass.Setup(x => x.WriteToFile(It.IsAny<string>(), It.IsAny<List<Question>>()))
                            .Throws(new Exception("File write error"));

            // Act & Assert
            var exception = Record.Exception(() => _quizService.AddQuestionToJson(question));

            Assert.NotNull(exception); // Oczekuję, że wystąpi wyjątek
            Assert.IsType<Exception>(exception); // Sprawdzam, czy wyjątek to wyjątek typu Exception
        }
    }
}

