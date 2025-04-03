using System.Collections.Generic;
using FluentAssertions;
using Moq;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.ServiceSupport;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;
using TomFromAlfred.QuizConsole.Tests.SupportForTests;
using Xunit;
using Xunit.Abstractions;


namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    // Oblane: 0 / 29
    // Wybieram zamianę metod w klasach na wirtualne

    public class QuizServiceTests
    {
        private Mock<QuestionService> _mockQuestionService;
        private Mock<ChoiceService> _mockChoiceService;
        private Mock<CorrectAnswerService> _mockCorrectAnswerService;
        private Mock<JsonCommonClass> _mockJsonCommonClass;
        private Mock<IFileWrapper> _mockFileWrapper;
        private MockQuizService _quizService;

        private readonly ITestOutputHelper _output;

        public QuizServiceTests(ITestOutputHelper output)
        {
            _output = output;

            _mockQuestionService = new Mock<QuestionService>(false);
            _mockChoiceService = new Mock<ChoiceService>();
            _mockCorrectAnswerService = new Mock<CorrectAnswerService>();
            _mockJsonCommonClass = new Mock<JsonCommonClass>();
            _mockFileWrapper = new Mock<IFileWrapper>();

            _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);

            _quizService = new MockQuizService(
                _mockQuestionService.Object,
                _mockChoiceService.Object,
                _mockCorrectAnswerService.Object,
                _mockJsonCommonClass.Object,
                _mockFileWrapper.Object
            );
        }

        #region InitializeJsonS QuizS. Tests
        // 1
        [Fact] // Zaliczony
        public void InitializeJsonService_ShouldNotThrowException_WhenValidArgumentsArePassed() // Inicjalizacja: nie wyrzuca wyjątku, jeśli dane poprawne
        {
            // Arrange
            string validFilePath = "validFilePath.json";

            var mockQuestions = new List<Question>
            {
                new Question(1, "Pytanie testowe.")
            };

            _mockJsonCommonClass.Setup(s => s.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestions);

            // Act
            var exception = Record.Exception(() =>
                _quizService.InitializeJsonService(_mockJsonCommonClass.Object, validFilePath)
            );

            // Assert
            exception.Should().BeNull("Metoda nie powinna rzucać wyjątku dla poprawnych danych wejściowych.");
        }

        // 2
        [Fact] // Zaliczony
        public void InitializeJsonService_ShouldThrowArgumentNullException_WhenJsonServiceIsNull() // Inicjalizacja: wyrzuca wyjątek, jeśli JsonService jest null
        {
            // Arrange
            _mockQuestionService = new Mock<QuestionService>(false); // zakładam parametr false

            // Przygotowanie minimalnych danych pytań, aby uniknąć wyjątku z LoadQuestionsFromJson
            var mockQuestions = new List<Question>
            {
                new Question(1, "Z ilu części składa się powieść Alfreda Szklarskiego?")
            };

            // Ustawienie mocka dla ReadFromFile, aby zwracał listę pytań
            _mockJsonCommonClass.Setup(s => s.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestions);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _quizService.InitializeJsonService(null, "validFilePath.json"));

            // Sprawdzam, czy wyjątek dotyczy parametru `jsonService`
            Assert.Equal("jsonService", exception.ParamName);
        }

        // 3 
        [Fact] // Zaliczony
        public void InitializeJsonService_ShouldThrowFileNotFoundException_WhenFileDoesNotExist() // Inicjalizacja: wyrzuca, jeśli plik nie istnieje
        {
            // Arrange
            string noExistingFilePath = "nonExistentFile.json"; // Plik nie istnieje

            // Mockuję istnienie plików: tylko dla ścieżki, która faktycznie ma nie istnieć, zwracam false
            _mockFileWrapper.Setup(f => f.Exists(It.Is<string>(path => path == noExistingFilePath))).Returns(false);
            _mockFileWrapper.Setup(f => f.Exists(It.Is<string>(path => path != noExistingFilePath))).Returns(true);

            // Przygotowuję minimalne dane, aby konstruktor nie wyrzucał wyjątku
            _mockQuestionService = new Mock<QuestionService>(false);
            var mockQuestions = new List<Question>
            {
                new Question(1, "Z ilu części składa się powieść Alfreda Szklarskiego?")
            };

            _mockJsonCommonClass.Setup(s => s.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestions);

           
            // Act & Assert
            var exception = Assert.Throws<FileNotFoundException>(() =>
                _quizService.InitializeJsonService(_mockJsonCommonClass.Object, noExistingFilePath));

            // Sprawdzam, czy wyjątek dotyczy poprawnego komunikatu
            Assert.Equal($"Plik {noExistingFilePath} nie istnieje.", exception.Message);
        }
        #endregion InitializeJsonS QuizS. Tests

        #region GetAllQuestions QuizS. Tests
        // 4
        [Fact] // Zaliczony
        public void GetAllQuestions_ShouldReturnQuestionsFromJsonAndService() // Zwraca: pytania z serwisu i pliku json
        {
            // Arrange
            // Inicjalizacja mocka z argumentem false
            _mockQuestionService = new Mock<QuestionService>(false);

            var mockQuestionsFromJson = new List<Question>
            {
                new Question (1, "Pytanie 1" ),
                new Question (2, "Pytanie 2")
            };
            var mockQuestionsFromService = new List<Question>
            {
            new Question (3, "Pytanie 3")
            };

            // Ustawienie mocków dla odczytu JSON i zwracania pytań z serwisu
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
        [Fact] // Zaliczony - wreszcie
        public void GetAllQuestions_ShouldReturnQuestionsFromServiceWhenJsonIsEmpty() // Zwraca: pytania z serwisu, jeśli json pusty
        {
            // Arrange
            _mockQuestionService = new Mock<QuestionService>(false);

            var mockQuestionsFromJson = new List<Question>(); // Puste pytania JSON
            var mockQuestionsFromService = new List<Question>
            {
                new Question(1, "Pytanie 1")
            };

            // Ustawienie mocka, aby zwracał pustą listę
            _mockJsonCommonClass.Setup(s => s.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestionsFromJson);

            _mockQuestionService.Setup(s => s.GetAllActive())
                .Returns(mockQuestionsFromService);

            _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);

            mockQuizService.CallBase = true; // Wykonuje prawdziwe metody, chyba że zostały zamockowane

            mockQuizService.Setup(q => q.LoadQuestionsFromJson(It.IsAny<string>()))
                .Callback(() => { }); // Nic nie robi

            _quizService = mockQuizService.Object;

            // Act
            var result = _quizService.GetAllQuestions().ToList();

            // Assert
            Assert.Single(result);  // Powinno być tylko pytanie z serwisu
            Assert.Contains(result, q => q.QuestionId == 1);
        }

        // 6
        [Fact] // Zaliczony
        public void GetAllQuestions_ShouldReturnQuestionsFromJsonWhenServiceIsEmpty() // Zwraca: pytania z json, jeśli serwis pusty
        {
            // Arrange

            // Inicjalizacja mocka z argumentem false
            _mockQuestionService = new Mock<QuestionService>(false);

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
        #endregion GetAllQuestions QuizS. Tests

        #region GetChoicesForQuestion QuizS. Tests
        // 7
        [Fact] // Zaliczony
        public void GetChoicesForQuestion_ShouldReturnChoicesFromJson_WhenChoicesExistInJson() // Zwraca: wybory z jsona, jeśli wybory te istnieją
        {
            // Arrange

            // Inicjalizacja mocka z argumentem false
            _mockQuestionService = new Mock<QuestionService>(false);

            int questionId = 1;
            var expectedChoices = new List<Choice>
            {
                new Choice (1, 'A', "Answer A" ),
                new Choice (2, 'B', "Answer B" )
            };

            // Ustawienie mocka, aby zwracał poprawne wybory z JSON
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Choice>>(It.IsAny<string>()))
                .Returns(expectedChoices);

            // Zapobieganie wyjątku związanego z brakiem pytań
            var mockQuestions = new List<Question>
            {
                new Question(1, "Placeholder question")
            };

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestions);

            mockQuizService.CallBase = true; // Wykonuje prawdziwe metody, chyba że zostały zamockowane

            mockQuizService.Setup(q => q.LoadQuestionsFromJson(It.IsAny<string>()))
                .Callback(() => { }); // Nie wykonuje żadnej akcji

            mockQuizService.Setup(q => q.GetChoicesForQuestion(questionId))
            .Returns(expectedChoices); // Zwraca bezpośrednio zamockowane wybory

            _quizService = mockQuizService.Object;

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
        [Fact] // Zaliczony
        public void GetChoicesForQuestion_ShouldReturnChoicesFromService_WhenChoicesNotExistInJson() // Daje wybory z serwisu, jeśli w json ich brak
        {
            // Arrange
            int questionId = 2;
            var serviceChoices = new List<Choice>
            {
                new Choice(1, 'A', "Wybór C"),
                new Choice(1, 'B', "Wybór D")
            };

            // Tworzenie mocka z wymaganym argumentem w konstruktorze
            _mockQuestionService = new Mock<QuestionService>(false);

            // Mockowanie danych z JSON - chociaż jedno pytanie, żeby uniknąć wyjątku
            var mockQuestions = new List<Question>
            {
                new Question(1, "Placeholder question")
            };

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestions);

            // Mockuję metodę GetChoicesForQuestion w ChoiceService
            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(serviceChoices);

            // Act
            var choices = _quizService.GetChoicesForQuestion(questionId).ToList();

            // Assert
            Assert.Equal(2, choices.Count);
            Assert.Equal('A', choices[0].ChoiceLetter);
            Assert.Equal("Wybór C", choices[0].ChoiceContent);
            Assert.Equal('B', choices[1].ChoiceLetter);
            Assert.Equal("Wybór D", choices[1].ChoiceContent);
        }

        // 9
        [Fact] // Zaliczony
        public void GetChoicesForQuestion_ShouldReturnEmpty_WhenNoChoicesFound() // Zwróci pustkę, jeśli wyborów nie znalazł
        {
            // Arrange
            int questionId = 3;

            // Tworzenie mocka z wymaganym argumentem w konstruktorze
            _mockQuestionService = new Mock<QuestionService>(false);

            var mockQuestions = new List<Question>
            {
                 new Question(1, "Placeholder question")
            };

            // ⬇️ Mockowanie przed utworzeniem instancji QuizService
            _mockJsonCommonClass.Setup(service => service.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestions);

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
        #endregion GetChoicesForQuestion QuizS. Tests

        #region GetShuffledChFQ QuizS. Tests
        // 10 
        [Fact] // Zaliczony
        public void GetShuffledChoicesForQuestion_ShouldShuffleChoicesAndMapLetters() // Losuje wybory i mapuje litery
        {
            // Arrange
            int questionId = 1;

            // Tworzenie mocka z wymaganym argumentem w konstruktorze
            _mockQuestionService = new Mock<QuestionService>(false);

            var originalChoices = new List<Choice>
            {
                new Choice (1, 'A', "Answer A"),
                new Choice (1, 'B', "Answer B"),
                new Choice (1, 'C', "Answer C")
            };

            var mockQuestions = new List<Question>
            {
                new Question(1, "Placeholder question")
            };

            // ⬇Ustawienia mocków przed utworzeniem instancji QuizService
            _mockJsonCommonClass.Setup(service => service.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestions);

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
            .Returns(() => originalChoices.Select(c => new Choice(c.ChoiceId, c.ChoiceLetter, c.ChoiceContent)).ToList());

            // Act
            Dictionary<char, char> letterMapping;
            var shuffledChoices = _quizService.GetShuffledChoicesForQuestion(questionId, out letterMapping).ToList();

            // Assert

            // Upewniam się, że mapowanie jest poprawne
            foreach (var choice in shuffledChoices)
            {
                Assert.Contains(letterMapping, mapping => mapping.Value == choice.ChoiceLetter);
            }

            Assert.False(shuffledChoices.SequenceEqual(originalChoices), "Lista odpowiedzi nie została przetasowana!");
            Assert.Equal(new HashSet<char> { 'A', 'B', 'C' }, letterMapping.Keys.ToHashSet());
            Assert.Equal(shuffledChoices.Count, letterMapping.Count);

            var mockReturnedChoices = _mockChoiceService.Object.GetChoicesForQuestion(questionId);
            Assert.NotSame(originalChoices, mockReturnedChoices);
        }

        // 11
        [Fact] // Zaliczony
        public void GetShuffledChoicesForQuestion_ShouldReturnSameNumberOfChoices() // Przy losowaniu Quizu, zawsze zwraca tę samą liczbę wyborów w zestawie
        {
            // Arrange
            int questionId = 2;

            // Tworzenie mocka z wymaganym argumentem w konstruktorze
            _mockQuestionService = new Mock<QuestionService>(false);

            var originalChoices = new List<Choice>
            {
                new Choice (1, 'A', "Wybór A"),
                new Choice (1, 'B', "Wybór B"),
                new Choice (1, 'C', "Wybór C")
            };

            var mockQuestions = new List<Question>
            {
                new Question(1, "Placeholder question")
            };

            // ⬇Ustawienia mocków przed utworzeniem instancji QuizService
            _mockJsonCommonClass.Setup(service => service.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestions);

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(originalChoices);

            // Act
            Dictionary<char, char> letterMapping;
            var shuffledChoices = _quizService.GetShuffledChoicesForQuestion(questionId, out letterMapping).ToList();

            // Assert
            Assert.Equal(originalChoices.Count, shuffledChoices.Count); 
        }

        // 12
        [Fact] // Zaliczony
        public void GetShuffledChoicesForQuestion_ShouldCreateCorrectLetterMapping() // Tworzy poprawną mapę liter
        {
            // Arrange
            int questionId = 3;

            // Tworzenie mocka z wymaganym argumentem w konstruktorze
            _mockQuestionService = new Mock<QuestionService>(false);

            var originalChoices = new List<Choice>
            {
                new Choice (3, 'A', "Opcja A"),
                new Choice (3, 'B', "Opcja B"),
                new Choice (3, 'C', "Opcja C")
            };

            var mockQuestions = new List<Question>
            {
                new Question(3, "Placeholder question")
            };

            // ⬇Ustawienia mocków przed utworzeniem instancji QuizService
            _mockJsonCommonClass.Setup(service => service.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestions);

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
        #endregion GetShuffledChFQ QuizS. Tests

        #region CheckAnswer QuizS. Tests
        // 13
        [Fact] // Zaliczony
        public void CheckAnswer_ShouldReturnTrue_WhenAnswerIsCorrectFromJson() // Zwraca prawdę, jeśli odpowiedź poprawna
        {
            // Arrange
            int questionId = 1;
            _mockQuestionService = new Mock<QuestionService>(false);

            char userChoiceLetter = 'B';
            var letterMapping = new Dictionary<char, char> { { 'A', 'C' }, { 'B', 'A' }, { 'C', 'B' } };

            // Ustawienie poprawnej odpowiedzi z JSON
            var correctAnswersFromJson = new List<JsonHelper>
            {
                new JsonHelper { QuestionNumber = questionId, LetterCorrectAnswer = "A", ContentCorrectAnswer = "Answer A" }
            };

            // Mockowanie JSON z poprawną odpowiedzią
            _mockJsonCommonClass.Setup(service => service.ReadFromFile<List<JsonHelper>>(It.IsAny<string>()))
                .Returns(correctAnswersFromJson);

            // Mock pytań
            var mockQuestions = new List<Question>
            {
                new Question(1, "Placeholder question")
            };

            // Mock pytań z JSON
            _mockJsonCommonClass.Setup(service => service.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestions);

            // Mock wyborów
            var choices = new List<Choice>
            {
                new Choice(1, 'A', "Answer A"),
                new Choice(1, 'B', "Answer B"),
                new Choice(1, 'C', "Answer C")
            };

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Act
            var result = _quizService.CheckAnswer(questionId, userChoiceLetter, letterMapping);

            // Assert
            Assert.True(result); // Powinno zwrócić true, zwrot A
        }

        // 14
        [Fact] // Zaliczony
        public void CheckAnswer_ShouldReturnTrue_WhenAnswerIsCorrectFromEntity() // Zwraca prawdę, jesli odpowiedź ok z entity 
        {
            // Arrange
            int questionId = 2;
            _mockQuestionService = new Mock<QuestionService>(false);

            char userChoiceLetter = 'C'; // To powinno pasować do Answer B
            var letterMapping = new Dictionary<char, char> { { 'A', 'C' }, { 'B', 'A' }, { 'C', 'B' } };

            // Mockowanie pustych odpowiedzi z JSON, aby test przechodził do serwisu
            _mockJsonCommonClass.Setup(service => service.ReadFromFile<List<CorrectAnswer>>(It.IsAny<string>()))
                .Returns(new List<CorrectAnswer>());

            // Mockowanie poprawnej odpowiedzi z serwisu
            var correctAnswerFromService = new CorrectAnswer(2, "Answer B", true);
            _mockCorrectAnswerService.Setup(service => service.GetCorrectAnswerForQuestion(questionId))
                                      .Returns(correctAnswerFromService);

            // Mockowanie wyborów dla pytania
            var choices = new List<Choice>
            {
                new Choice (2, 'A', "Answer A"),
                new Choice (2, 'B', "Answer B"),
                new Choice (2, 'C', "Answer C")
            };

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Mockowanie pytań z JSON
            var mockQuestions = new List<Question>
            {
                new Question(2, "Placeholder question")
            };

            _mockJsonCommonClass.Setup(service => service.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestions);

            // Act
            var result = _quizService.CheckAnswer(questionId, userChoiceLetter, letterMapping);

            // Assert
            Assert.True(result); // Odpowiedź powinna być poprawna z serwisu
        }

        // 15 
        [Fact] // Zaliczony
        public void CheckAnswer_ShouldReturnFalse_WhenAnswerIsIncorrect() // Sprawdza: błąd, jeśli odpowiedź błędna
        {
            // Arrange
            int questionId = 3;
            _mockQuestionService = new Mock<QuestionService>(false);

            char userChoiceLetter = 'B';
            var letterMapping = new Dictionary<char, char> { { 'A', 'C' }, { 'B', 'A' }, { 'C', 'B' } };

            // Mockowanie pustych odpowiedzi z JSON, aby test przechodził do serwisu
            _mockJsonCommonClass.Setup(service => service.ReadFromFile<List<CorrectAnswer>>(It.IsAny<string>()))
                .Returns(new List<CorrectAnswer>());

            // Mockowanie błędnej odpowiedzi z serwisu
            var incorrectAnswerFromService = new CorrectAnswer(3, "A", true); // Odpowiedź powinna być A
            _mockCorrectAnswerService.Setup(service => service.GetCorrectAnswerForQuestion(questionId))
                                      .Returns(incorrectAnswerFromService);

            // Mockowanie wyborów
            var choices = new List<Choice>
            {
                new Choice (3, 'A', "Answer C"),
                new Choice (3, 'B', "Answer B"), // Użytkownik wybiera B, ale poprawna odpowiedź to A
                new Choice (3, 'C', "Answer A")
            };

            // Mockowanie pytań z JSON
            var mockQuestions = new List<Question>
            {
                new Question(3, "Placeholder question")
            };

            _mockJsonCommonClass.Setup(service => service.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestions);

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Act
            var result = _quizService.CheckAnswer(questionId, userChoiceLetter, letterMapping);

            // Assert
            Assert.False(result); // Odpowiedź powinna być niepoprawna
        }

        // 16
        [Fact] // Zaliczony
        public void CheckAnswer_ShouldReturnFalse_WhenLetterMappingDoesNotContainUserChoice() // Sprawdza: podaje błąd, jeśli błędna litera użytkownika
        {
            // Arrange
            int questionId = 1;
            _mockQuestionService = new Mock<QuestionService>(false);

            char userChoiceLetter = 'D'; // Litera, która nie znajduje się w mapowaniu
            var letterMapping = new Dictionary<char, char> { { 'A', 'B' }, { 'B', 'C' } }; // Brak mapowania dla 'D'

            
            var choices = new List<Choice>
            {
                new Choice (1, 'A', "Answer A"),
                new Choice (1, 'B', "Answer B"),
                new Choice (1, 'C', "Odpowiedź C")
             };

            // Mockowanie pytań z JSON
            var mockQuestions = new List<Question>
            {
                new Question(3, "Placeholder question")
            };

            _mockJsonCommonClass.Setup(service => service.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestions);

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Act
            var result = _quizService.CheckAnswer(questionId, userChoiceLetter, letterMapping);

            // Assert
            Assert.False(result); // Brak mapowania dla 'D', więc odpowiedź jest błędna
        }

        // 17
        [Fact] // Zaliczony
        public void CheckAnswer_ShouldReturnFalse_WhenLetterMappingIsIncorrect() // Sprawdza: podaje błąd czyli???, jeśli mapowanie niepoprawne
        {
            // Arrange
            int questionId = 1;
            _mockQuestionService = new Mock<QuestionService>(false);

            char userChoiceLetter = 'B';
            var letterMapping = new Dictionary<char, char> { { 'A', 'C' }, { 'B', 'D' } }; // Błędne mapowanie

            
            var choices = new List<Choice>
            {
                new Choice (1, 'A', "Answer A" ),
                new Choice (1, 'B', "Answer B" ),
                new Choice (1, 'C', "Odpowiedź C")
            };

            // Mockowanie pytań z JSON
            var mockQuestions = new List<Question>
            {
                new Question(1, "Placeholder question")
            };

            _mockJsonCommonClass.Setup(service => service.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestions);


            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Act
            var result = _quizService.CheckAnswer(questionId, userChoiceLetter, letterMapping);

            // Assert
            Assert.False(result); // Zły wybór w mapowaniu, odpowiedź powinna być błędna
        }

        // 18
        [Fact] // Zaliczony
        public void CheckAnswer_ShouldReturnFalse_WhenLetterMappingIsMissing() // Sprawdza: podaje błąd, jeśli nie ma litery zmapowanej
        {
            // Arrange
            _mockQuestionService = new Mock<QuestionService>(false);

            var mockQuestions = new List<Question>
            {
                new Question(1, "Placeholder question")
            };

            _mockJsonCommonClass.Setup(service => service.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestions);

            var letterMapping = new Dictionary<char, char>
            {
                {'A', 'X'}, // Mapowanie poprawnej odpowiedzi
                {'B', 'Y'}
            };

            char userChoiceLetter = 'C'; // Brak mapowania dla tej litery

            _mockCorrectAnswerService.Setup(ca => ca.GetCorrectAnswerForQuestion(1))
                .Returns(new CorrectAnswer(1, "X", true)); // Zwracam poprawną odpowiedź

            var choices = new List<Choice>
            {
                new Choice(1, 'A', "Answer X"),
                new Choice(1, 'B', "Answer Y")
            };

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(1))
                .Returns(choices);

            // Act
            var result = _quizService.CheckAnswer(1, userChoiceLetter, letterMapping);

            // Assert
            Assert.False(result); // Spodziewam się, że odpowiedź będzie błędna, ponieważ nie ma mapowania dla 'C'
        }
        #endregion CheckAnswer QuizS. Tests

        #region GetLetterForAnswer QuizS. Tests
        // 19
        [Fact] // Zaliczony
        public void GetLetterForAnswer_ShouldReturnCorrectLetterForAnswer() // Podaje: literę dla poprawnej odpowiedzi
        {
            // Arrange
            int questionId = 1;
            _mockQuestionService = new Mock<QuestionService>(false);

            var mockQuestions = new List<Question>
            {
                new Question(1, "Placeholder question")
            };

            _mockJsonCommonClass.Setup(service => service.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestions);

            var choices = new List<Choice>
            {
                new Choice (1, 'A', "Answer A"),
                new Choice (1, 'B', "Answer B"),
                new Choice (1, 'C', "Answer C")
            };

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Act
            var result = _quizService.GetAnswerContentFromLetter('B', questionId);

            // Assert
            Assert.Equal("Answer B", result);
        }

        // 20
        [Fact] // Zaliczony
        public void GetLetterForAnswer_ShouldReturnDefaultLetter_WhenNoMatchFound() // Podaje: domyślną literę, jeśli powiązania nie znajdzie
        {
            // Arrange
            int questionId = 1;
            _mockQuestionService = new Mock<QuestionService>(false);

            var mockQuestions = new List<Question>
            {
                new Question(1, "Placeholder question")
            };

            _mockJsonCommonClass.Setup(service => service.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestions);

            var choices = new List<Choice>
            {
                new Choice (1, 'A', "Answer A"),
                new Choice (1, 'B', "Answer B"),
                new Choice (1, 'C', "Answer C")
            };

            _mockChoiceService.Setup(service => service.GetChoicesForQuestion(questionId))
                              .Returns(choices);

            // Act
            var result = _quizService.GetAnswerContentFromLetter('D', questionId); // 'D' nie istnieje w liście odpowiedzi

            // Assert
            Assert.Null(result); // Teraz metoda zwraca `null`
        }
        #endregion GetLetterForAnswer QuizS. Tests

        #region LoadQFJson QuizS. Tests
        // 21
        [Fact] // Zaliczony
        public void LoadQuestionsFromJson_ShouldReturn_WhenFileDoesNotExist() // Ładuje: zwraca, jeśli brak pliku z pytaniami
        {
            // Arrange
            string filePath = "nonexistentFile.json";

            // Zamockowanie, aby symulować brak pliku
            _mockFileWrapper.Setup(f => f.Exists(filePath)).Returns(false);

            // Mockowanie pytań, aby uniknąć błędu przy konstrukcji QuizService
            var mockQuestions = new List<Question>
            {
                new Question(1, "Placeholder question")
            };

            _mockJsonCommonClass.Setup(service => service.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestions);

            // Zamockowanie QuestionService z przekazanym argumentem false
            _mockQuestionService = new Mock<QuestionService>(false);

            // Nadpisanie działania metody LoadQuestionsFromJson
            mockQuizService.Setup(q => q.LoadQuestionsFromJson(It.IsAny<string>()))
                .Callback(() => { }); // Nie robi nic przy ładowaniu pytań

            _quizService = mockQuizService.Object;

            // Act
            var exception = Record.Exception(() => _quizService.LoadQuestionsFromJson(filePath));

            // Assert
            Assert.Null(exception); // Sprawdzam, czy nie pojawił się wyjątek
        }

        // 22
        [Fact] // Zaliczony
        public void LoadQuestionsFromJson_ShouldThrowJsonException_WhenJsonIsEmpty() // Ładuje: wyrzuca wyjątek, jeśli json pusty
        {
            // Arrange
            string filePath = "questions.json";

            var mockFileWrapper = new Mock<IFileWrapper>();
            mockFileWrapper.Setup(x => x.Exists(filePath)).Returns(true);

            var mockJsonCommonClass = new Mock<JsonCommonClass>();
            mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(filePath))
                               .Returns(new List<Question>()); // Pusta lista zamiast null

            var mockChoiceService = new Mock<ChoiceService>();
            var mockCorrectAnswerService = new Mock<CorrectAnswerService>();

            var realQuestionService = new QuestionService(false); // Nie ładuję domyślnych pytań

            mockQuizService.Setup(q => q.LoadQuestionsFromJson(It.IsAny<string>()))
                           .Throws(new JsonException("JSON does not contain any questions"));

            // Act
            var exception = Record.Exception(() => mockQuizService.Object.LoadQuestionsFromJson(filePath));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<TargetInvocationException>(exception); // Sprawdzam opakowany wyjątek
            Assert.NotNull(exception.InnerException); // Sprawdzam, czy jest wyjątek wewnętrzny
            Assert.IsType<JsonException>(exception.InnerException); // Sprawdzam, czy to rzeczywiście JsonException
            Assert.Contains("JSON does not contain any questions", exception.InnerException.Message);
        }

        // 23
        [Fact] // Zaliczony
        public void LoadQuestionsFromJson_ShouldLoadQuestions_WhenJsonIsValid() // Ładuje: pytania, jeśli json jest prawidłowy
        {
            // Arrange
            string filePath = "questions.json";
            var questions = new List<Question> { new Question(1, "What is 2 + 2?") };

            _mockFileWrapper.Setup(x => x.Exists(filePath)).Returns(true); // Upewniam się, że plik istnieje
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(It.IsAny<string>())).Returns(questions); // Poprawne mockowanie pliku

            _mockQuestionService = new Mock<QuestionService>(MockBehavior.Strict, new object[] { false }); // Wymuszam wywołanie konstruktora
            _mockQuestionService.Setup(x => x.GetAllActive()).Returns(questions); // Poprawne mockowanie odpowiedzi

            // Act
            _quizService.LoadQuestionsFromJson(filePath);

            // Assert
            Assert.NotEmpty(_quizService.GetAllQuestions());
        }
        #endregion LoadQFJson QuizS. Tests

        #region LoadChFJson QuizS. Tests
        // 24
        [Fact] // Zaliczony
        public void LoadChoicesFromJson_ShouldReturn_WhenChoicesFileDoesNotExist() // Ładuje: wyrzuca, jeśli plik wyborów nie istnieje
        {
            // Arrange
            _mockFileWrapper.Setup(x => x.Exists(It.IsAny<string>())).Returns(false); // Symuluję brak pliku
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Choice>>(It.IsAny<string>()))
                                .Returns(new List<Choice>()); // Zamiast wyjątku zwracam pustą listę

            List<Question> mockQuestions = new List<Question>
            {
                new Question(1, "Example question?")
            };

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(It.IsAny<string>())).Returns(mockQuestions);

            _mockQuestionService = new Mock<QuestionService>(MockBehavior.Strict, new object[] { false }); // Wymuszam wywołanie konstruktora

            // Act & Assert
            var exception = Record.Exception(() => _quizService.LoadChoicesFromJson());
            Assert.Null(exception); // Nie powinno być wyjątku, obsługuję brak pliku
        }

        // 25
        [Fact] // Zaliczony
        public void LoadChoicesFromJson_ShouldLoadChoices_WhenFileIsValid() // Ładuje: wybory, jeśli plik jest prawidłowy
        {
            // Arrange
            var choices = new List<Choice>
            {
                new Choice(1, 'A', "Answer A"),
                new Choice(1, 'B', "Answer B")
            };

            List<Question> mockQuestions = new List<Question>
            {
                new Question(1, "Example question?")
            };

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Choice>>(It.IsAny<string>())).Returns(choices);
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(It.IsAny<string>())).Returns(mockQuestions);

            _mockQuestionService = new Mock<QuestionService>(MockBehavior.Strict, new object[] { false }); // Wymuszam wywołanie konstruktora

            // Act
            _quizService.LoadChoicesFromJson();

            // Assert
            var loadedChoices = _quizService.GetChoicesForQuestion(1).ToList();
            Assert.Equal(2, loadedChoices.Count);
        }
        #endregion  LoadChFJson QuizS. Tests

        #region LoadCorrectSFJson QuizS. Tests
        // 26
        [Fact] // Zaliczony
        public void LoadCorrectSetFromJson_ShouldReturn_WhenCorrectSetFileDoesNotExist() // Ładuje: podaje wyjątek, jeśli plik nie istnieje
        {
            // Arrange
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<JsonHelper>>(It.IsAny<string>()))
                                .Throws(new FileNotFoundException());

            List<Question> mockQuestions = new List<Question>
            {
                new Question(1, "Placeholder question?")
            };
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(It.IsAny<string>())).Returns(mockQuestions); // Dodanie pytania do JSON

            _mockQuestionService = new Mock<QuestionService>(MockBehavior.Strict, new object[] { false }); // Wymuszam wywołanie konstruktora

            // Act & Assert
            var exception = Record.Exception(() => _quizService.LoadCorrectSetFromJson());
            Assert.Null(exception);
        }

        // 27
        [Fact] // Zaliczony
        public void LoadCorrectSetFromJson_ShouldLoadCorrectAnswers_WhenFileIsValid() // Ładuje: poprawne odpowiedzi, jeśli plik jest ok 
        {
            // Arrange
            var correctSet = new List<JsonHelper>
            {
                new JsonHelper { QuestionNumber = 1, LetterCorrectAnswer = "A", ContentCorrectAnswer = "Answer A" }
            };

            List<Question> mockQuestions = new List<Question>
            {
                new Question(1, "Example question?")
            };

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<JsonHelper>>(It.IsAny<string>())).Returns(correctSet);
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(It.IsAny<string>())).Returns(mockQuestions); // Dodanie pytania do JSON

            var correctAnswer = new CorrectAnswer(1, "Answer A", true);
            _mockCorrectAnswerService.Setup(x => x.GetCorrectAnswerForQuestion(1)).Returns(correctAnswer);

            _mockQuestionService = new Mock<QuestionService>(MockBehavior.Strict, new object[] { false }); // Wymuszam wywołanie konstruktora


            // Act
            _quizService.LoadCorrectSetFromJson();

            // Assert
            var retrievedCorrectAnswer = _mockCorrectAnswerService.Object.GetCorrectAnswerForQuestion(1);
            Assert.NotNull(retrievedCorrectAnswer);
            Assert.Equal("Answer A", retrievedCorrectAnswer.CorrectAnswerContent);
        }
        #endregion LoadCorrectSFJson QuizS. Tests

        // 28
        [Fact] // Zaliczony
        public void AddQuestionToJson_ShouldAddQuestionAndSaveToFile() // Dodaje: pytanie i zapisuje
        {
            // Arrange
            var question = new Question(1, "Z ilu części składa się powieść Alfreda Szklarskiego?");
            var questions = new List<Question> { question }; // Upewniam się, że JSON zawiera pytania

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(It.IsAny<string>())).Returns(questions);
            _mockJsonCommonClass.Setup(x => x.WriteToFile(It.IsAny<string>(), It.IsAny<List<Question>>()));

            _mockQuestionService = new Mock<QuestionService>(MockBehavior.Strict, false); // Wymuszam wywołanie konstruktora

            // Act
            _quizService.AddQuestionToJson(question);

            // Assert
            _mockJsonCommonClass.Verify(x => x.WriteToFile(It.IsAny<string>(), It.Is<List<Question>>(q => q.Contains(question))), Times.Once);
        }

        // 29
        [Fact] // Zaliczony
        public void SaveQuestionsToJson_ShouldHandleFileWriteException()
        {
            // Arrange
            var question = new Question(1, "Z ilu części składa się powieść Alfreda Szklarskiego?");
            var questions = new List<Question> { question };

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(It.IsAny<string>())).Returns(questions);
            _mockJsonCommonClass.Setup(x => x.WriteToFile(It.IsAny<string>(), It.IsAny<List<Question>>()))
                                .Throws(new Exception("Błąd podczas zapisywania pytań do pliku JSON"));

            _mockQuestionService = new Mock<QuestionService>(MockBehavior.Strict, new object[] { false }); // Wymuszam wywołanie konstruktora

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _quizService.AddQuestionToJson(question));
            Assert.Equal("Błąd podczas zapisywania pytań do pliku JSON", exception.Message);
        }
    }
}

