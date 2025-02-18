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
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.ServiceSupport;


namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    // Oblane: 30 / 31
    // Wybieram zamianę metod w klasach na wirtualne

    public class QuizServiceTests
    {
        private readonly Mock<QuestionService> _mockQuestionService;
        private readonly Mock<ChoiceService> _mockChoiceService;
        private readonly Mock<CorrectAnswerService> _mockCorrectAnswerService;
        private readonly Mock<JsonCommonClass> _mockJsonCommonClass;
        private readonly Mock<IFileWrapper> _mockFileWrapper;
        private QuizService _quizService;


        public QuizServiceTests()
        {
            // Tworzę mocki dla klas serwisowych
            _mockQuestionService = new Mock<QuestionService>(false); // Nie ładuje domyślnych pytań
            _mockChoiceService = new Mock<ChoiceService>();
            _mockCorrectAnswerService = new Mock<CorrectAnswerService>();
            _mockJsonCommonClass = new Mock<JsonCommonClass>();
            _mockFileWrapper = new Mock<IFileWrapper>();

            // Mockuję File.Exists, aby test nie zależał od rzeczywistych plików
            _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);
        }

        // 1
        [Fact] // Oblany
        public void InitializeJsonService_ShouldNotThrowException_WhenValidArgumentsArePassed() // Inicjalizacja: nie wyrzuca wyjątku, jeśli dane poprawne
        {
            // Arrange
            string validFilePath = "validFilePath.json";
            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

            // Act
            var exception = Record.Exception(() => _quizService.InitializeJsonService(_mockJsonCommonClass.Object, validFilePath));

            // Assert
            Assert.Null(exception);
        }

        // 2
        [Fact] // Oblany
        public void InitializeJsonService_ShouldThrowArgumentNullException_WhenJsonServiceIsNull() // Inicjalizacja: wyrzuca wyjątek, jeśli JsonService jest null
        {

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _quizService.InitializeJsonService(null, "validFilePath.json"));

            // Sprawdzam, czy wyjątek dotyczy parametru `jsonService`
            Assert.Equal("jsonService", exception.ParamName);
        }

        // 3 
        [Fact] // Oblany
        public void InitializeJsonService_ShouldThrowFileNotFoundException_WhenFileDoesNotExist() // Inicjalizacja: wyrzuca, jeśli plik nie istnieje
        {
            // Arrange
            string noExistingFilePath = "nonExistentFile.json"; // Plik nie istnieje

            // Ustawiam mocka, aby `Exists()` zwracało `false`
            _mockFileWrapper.Setup(f => f.Exists(noExistingFilePath)).Returns(false);

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

            // Act & Assert
            var exception = Assert.Throws<FileNotFoundException>(() =>
                _quizService.InitializeJsonService(_mockJsonCommonClass.Object, noExistingFilePath));

            Assert.Equal($"Plik {noExistingFilePath} nie istnieje.", exception.Message);
        }

        // 4
        [Fact] // Zaliczony
        public void GetAllQuestions_ShouldReturnQuestionsFromJsonAndService() // Zwraca: pytania z serwisu i pliku json
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

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

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
        public void GetAllQuestions_ShouldReturnQuestionsFromServiceWhenJsonIsEmpty() // Zwraca: pytania z serwisu, jeśli json pusty
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

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

            // Act
            var result = _quizService.GetAllQuestions().ToList();

            // Assert
            Assert.Single(result);  // Powinno być tylko pytanie z serwisu
            Assert.Contains(result, q => q.QuestionId == 1);
        }

        // 6
        [Fact] // Oblany
        public void GetAllQuestions_ShouldReturnQuestionsFromJsonWhenServiceIsEmpty() // Zwraca: pytania z json, jeśli serwis pusty
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

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

            // Act
            var result = _quizService.GetAllQuestions().ToList();

            // Assert
            Assert.Single(result);  // Powinno być tylko pytanie z JSON
            Assert.Contains(result, q => q.QuestionId == 1);
        }

        // 7
        [Fact] // Oblany
        public void GetChoicesForQuestion_ShouldReturnChoicesFromJson_WhenChoicesExistInJson() // Zwraca: wybory z jsona, jeśli wybory te istnieją
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

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

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

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

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

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

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

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

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

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

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

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

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
            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

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
            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Act
            var result = _quizService.CheckAnswer(questionId, userChoiceLetter, letterMapping);

            // Assert
            Assert.True(result); 
        }

        // 15 
        [Fact] // Oblany
        public void CheckAnswer_ShouldReturnFalse_WhenAnswerIsIncorrect() // Sprawdza: błąd, jeśli odpowiedź błędna
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

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );
            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Act
            var result = _quizService.CheckAnswer(questionId, userChoiceLetter, letterMapping);

            // Assert
            Assert.False(result); 
        }

        // 16 
        [Fact] // Oblany
        public void CheckAnswer_ShouldReturnFalse_WhenLetterMappingIsEmpty() // Sprawdza: błąd, jeśli mapowanie puste
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

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );
            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Act
            var result = _quizService.CheckAnswer(questionId, userChoiceLetter, emptyLetterMapping);

            // Assert
            Assert.False(result); // Ponieważ mapowanie jest puste, odpowiedź jest błędna
        }

        // 17
        [Fact] // Oblany
        public void CheckAnswer_ShouldReturnFalse_WhenLetterMappingDoesNotContainUserChoice() // Sprawdza: podaje błąd, jeśli błędna litera użytkownika
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

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Act
            var result = _quizService.CheckAnswer(questionId, userChoiceLetter, letterMapping);

            // Assert
            Assert.False(result); // Brak mapowania dla 'D', więc odpowiedź jest błędna
        }

        // 18
        [Fact] // Oblany
        public void CheckAnswer_ShouldReturnFalse_WhenLetterMappingIsIncorrect() // Sprawdza: podaje błąd, jesli mapowanie niepoprawne
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

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Act
            var result = _quizService.CheckAnswer(questionId, userChoiceLetter, letterMapping);

            // Assert
            Assert.False(result); // Zły wybór w mapowaniu, odpowiedź powinna być błędna
        }

        // 19
        [Fact] // Oblany
        public void CheckAnswer_ShouldReturnFalse_WhenChoicesAreEmpty() // Sprawdza: podaje błąd, jeśli brak wyborów
        {
            // Arrange
            int questionId = 1;
            char userChoiceLetter = 'A';
            var letterMapping = new Dictionary<char, char> { { 'A', 'B' }, { 'B', 'C' } };

            var choices = new List<Choice>(); // Pusta lista odpowiedzi

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Act
            var result = _quizService.CheckAnswer(questionId, userChoiceLetter, letterMapping);

            // Assert
            Assert.False(result); // Brak odpowiedzi, odpowiedź jest błędna
        }

        // 20
        [Fact] // Oblany
        public void CheckAnswer_ShouldReturnFalse_WhenLetterMappingIsMissing() // Sprawdza: podaje błąd, jeśli nie ma litery zmapowanej
        {
            // Arrange
            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

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
        [Fact] // Oblany
        public void GetLetterForAnswer_ShouldReturnCorrectLetterForAnswer() // Podaje: literę dla poprawnej odpowiedzi
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

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

            // Act
            var result = _quizService.GetLetterForAnswer(answerContent, questionId);

            // Assert
            Assert.Equal('B', result); 
        }

        // 22
        [Fact] // Oblany
        public void GetLetterForAnswer_ShouldReturnDefaultLetter_WhenNoMatchFound() // Podaje: domyślną literę, jeśli powiązania nie znajdzie
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

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

            // Act
            var result = _quizService.GetLetterForAnswer(answerContent, questionId);

            // Assert
            Assert.Equal('?', result); 
        }

        // 23
        [Fact] // Oblany
        public void LoadQuestionsFromJson_ShouldReturn_WhenFileDoesNotExist() // Ładuje: zwraca, jeśli brak pliku z pytaniami
        {
            // Arrange
            string filePath = "nonexistentFile.json";
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(filePath)).Throws(new FileNotFoundException());

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

            // Act & Assert
            var exception = Record.Exception(() => _quizService.LoadQuestionsFromJson(filePath));
            Assert.Null(exception);  // Sprawdzam, czy nie pojawił się wyjątek
        }

        // 24
        [Fact] // Oblany
        public void LoadQuestionsFromJson_ShouldThrowJsonException_WhenJsonIsEmpty() // Ładuje: wyrzuca wyjątek, jeśli json pusty
        {
            // Arrange
            string filePath = "questions.json";
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(filePath)).Returns(new List<Question>());

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

            // Act & Assert
            var exception = Assert.Throws<JsonException>(() => _quizService.LoadQuestionsFromJson(filePath));
            Assert.Equal("JSON does not contain any questions", exception.Message);
        }

        // 25
        [Fact] // Oblany
        public void LoadQuestionsFromJson_ShouldLoadQuestions_WhenJsonIsValid() // Ładuje: pytania, jeśli json jest prawidłowy
        {
            // Arrange
            string filePath = "questions.json";
            var questions = new List<Question>
            {
                new Question (1, "What is 2 + 2?" )
            };

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(filePath)).Returns(questions);

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

            // Act
            _quizService.LoadQuestionsFromJson(filePath);

            // Assert
            Assert.NotEmpty(_quizService.GetAllQuestions()); // Sprawdzam, czy pytania zostały załadowane
        }

        // 26
        [Fact] // Oblany
        public void LoadChoicesFromJson_ShouldReturn_WhenChoicesFileDoesNotExist() // Ładuje: wyrzuca, jeśli plik wyborów nie istnieje
        {
            // Arrange
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Choice>>(It.IsAny<string>())).Throws(new FileNotFoundException());

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

            // Act & Assert
            var exception = Record.Exception(() => _quizService.LoadChoicesFromJson());
            Assert.Null(exception);  // Sprawdzam, czy nie pojawił się wyjątek
        }

        // 27
        [Fact] // Oblany
        public void LoadChoicesFromJson_ShouldLoadChoices_WhenFileIsValid() // Ładuje: wybory, jeśli plik jest prawidłowy
        {
            // Arrange
            var choices = new List<Choice>
            {
                new Choice (1, 'A', "Answer A" ), // Dla pytania 1
                new Choice (1, 'B', "Answer B")  // Dla pytania 1
            };

            // Setup mock dla odczytu pliku
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Choice>>(It.IsAny<string>())).Returns(choices);

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

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
        [Fact] // Oblany
        public void LoadCorrectSetFromJson_ShouldReturn_WhenCorrectSetFileDoesNotExist() // Ładuje: podaje wyjątek, jeśli plik nie istnieje
        {
            // Arrange
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<JsonHelper>>(It.IsAny<string>())).Throws(new FileNotFoundException());

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

            // Act & Assert
            var exception = Record.Exception(() => _quizService.LoadCorrectSetFromJson());
            Assert.Null(exception);  // Sprawdzam, czy nie pojawił się wyjątek
        }

        // 29
        [Fact] // Oblany
        public void LoadCorrectSetFromJson_ShouldLoadCorrectAnswers_WhenFileIsValid() // Ładuje: poprawne odpowiedzi, jeśli plik jest ok 
        {
            // Arrange
            var correctSet = new List<JsonHelper>
            {
                new JsonHelper { QuestionNumber = 1, LetterCorrectAnswer = "A", ContentCorrectAnswer = "Answer A" }
            };

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<JsonHelper>>(It.IsAny<string>())).Returns(correctSet);

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

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

        // 30
        [Fact] // Oblany
        public void AddQuestionToJson_ShouldAddQuestionAndSaveToFile() // Dodaje: pytanie i zapisuje
        {
            // Arrange
            var question = new Question(1, "Z ilu części składa się powieść Alfreda Szklarskiego?");

            _mockJsonCommonClass.Setup(x => x.WriteToFile(It.IsAny<string>(), It.IsAny<List<Question>>()));

            _quizService = new QuizService(
                        _mockQuestionService.Object,
                        _mockChoiceService.Object,
                        _mockCorrectAnswerService.Object,
                        _mockJsonCommonClass.Object,
                        _mockFileWrapper.Object
            );

            // Act
            _quizService.AddQuestionToJson(question);

            // Assert
            _mockJsonCommonClass.Verify(x => x.WriteToFile(It.IsAny<string>(), It.Is<List<Question>>(q => q.Contains(question))), Times.Once);
            // Sprawdzam, czy metoda zapisu została wywołana raz i czy zawiera pytanie
        }

        // 31
        [Fact] // Oblany masakra
        public void SaveQuestionsToJson_ShouldHandleFileWriteException() // Zachowuje: pytania do pliku json. Tu - wyrzuca wyjątek.
        {
            // Arrange
            var question = new Question(1, "Z ilu części składa się powieść Alfreda Szklarskiego?");

            // Nadpisuję mocka odczytu JSON, żeby uniknąć błędu `JSON does not contain any questions`
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(It.IsAny<string>()))
                                .Returns(new List<Question>()); // Zapewniam, że JSON nie będzie pusty

            _quizService = new QuizService(
                _mockQuestionService.Object,
                _mockChoiceService.Object,
                _mockCorrectAnswerService.Object,
                _mockJsonCommonClass.Object,
                _mockFileWrapper.Object
                );

            // Symuluję błąd zapisu JSON
            _mockJsonCommonClass.Setup(x => x.WriteToFile(It.IsAny<string>(), It.IsAny<List<Question>>()))
                                .Throws(new Exception("Błąd podczas zapisywania pytań do pliku JSON"));

            // Act & Assert
            #if DEBUG
            Exception exception = null;
            try
            {
                _quizService.AddQuestionToJson(question);
            }
            catch (Exception ex)
            {
                exception = ex; // Przechwytuję wyjątek do zmiennej, żeby można go było zweryfikować w teście
            }

            // Jeśli wyjątek NIE został rzucony, test powinien nie przejść
            Assert.NotNull(exception);

            // Sprawdzam dokładną treść wyjątku
            Assert.Equal("Błąd podczas zapisywania pytań do pliku JSON", exception.Message);
            #else
            var exception = Record.Exception(() => _quizService.AddQuestionToJson(question));
            Assert.Null(exception);
            #endif

            // Weryfikuję, czy zapis JSON został wywołany raz
            _mockJsonCommonClass.Verify(x => x.WriteToFile(It.IsAny<string>(), It.Is<List<Question>>(q => q.Contains(question))), Times.Once);
        }
    }
}

