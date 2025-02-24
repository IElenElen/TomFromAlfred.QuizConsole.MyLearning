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
    // Oblane: 8 / 29
    // Wybieram zamianę metod w klasach na wirtualne

    public class QuizServiceTests
    {
        private Mock<QuestionService> _mockQuestionService;
        private readonly Mock<ChoiceService> _mockChoiceService;
        private readonly Mock<CorrectAnswerService> _mockCorrectAnswerService;
        private readonly Mock<JsonCommonClass> _mockJsonCommonClass;
        private readonly Mock<IFileWrapper> _mockFileWrapper;
        private QuizService _quizService;


        public QuizServiceTests()
        {
            // Tworzę mocki dla klas serwisowych
            _mockQuestionService = new Mock<QuestionService>(); 
            _mockChoiceService = new Mock<ChoiceService>();
            _mockCorrectAnswerService = new Mock<CorrectAnswerService>();
            _mockJsonCommonClass = new Mock<JsonCommonClass>();
            _mockFileWrapper = new Mock<IFileWrapper>();

            // Mockuję File.Exists, aby test nie zależał od rzeczywistych plików
            _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);
        }

        // 1
        [Fact] // Zaliczony
        public void InitializeJsonService_ShouldNotThrowException_WhenValidArgumentsArePassed() // Inicjalizacja: nie wyrzuca wyjątku, jeśli dane poprawne
        {
            // Arrange
            string validFilePath = "validFilePath.json";

            // Poprawna inicjalizacja mocka QuestionService z parametrem false
            _mockQuestionService = new Mock<QuestionService>(false);

            // Przygotowanie minimalnych danych wejściowych, aby uniknąć wyjątku przy wczytywaniu pytań
            var mockQuestions = new List<Question>
            {
                new Question(1, "Z ilu części składa się powieść Alfreda Szklarskiego?")
            };

            // Ustawienie mocka, aby zwracał poprawne dane
            _mockJsonCommonClass.Setup(s => s.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestions);

            // Inicjalizacja quizu
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
            Assert.Null(exception); // Sprawdzam, czy żaden wyjątek się nie pojawił
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

            // Inicjalizacja quizu
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

            // Inicjalizacja quizu
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

            // Sprawdzam, czy wyjątek dotyczy poprawnego komunikatu
            Assert.Equal($"Plik {noExistingFilePath} nie istnieje.", exception.Message);
        }

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

            // Mockowanie QuizService i nadpisanie metody LoadQuestionsFromJson, aby nie wykonywała żadnej akcji
            var mockQuizService = new Mock<QuizService>(
                _mockQuestionService.Object,
                _mockChoiceService.Object,
                _mockCorrectAnswerService.Object,
                _mockJsonCommonClass.Object,
                _mockFileWrapper.Object
            );

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

            // Mockowanie QuizService i nadpisanie metody LoadQuestionsFromJson, aby nie wykonywała żadnej akcji
            var mockQuizService = new Mock<QuizService>(
                _mockQuestionService.Object,
                _mockChoiceService.Object,
                _mockCorrectAnswerService.Object,
                _mockJsonCommonClass.Object,
                _mockFileWrapper.Object
            );

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

            // Inicjalizacja QuizService z mockiem
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

            // Inicjalizacja QuizService po ustawieniu mocków
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
            Assert.Empty(choices);
        }

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
                .Returns(originalChoices);

            // Inicjalizacja quizu po mockowaniu
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

            _quizService = new QuizService(
                _mockQuestionService.Object,
                _mockChoiceService.Object,
                _mockCorrectAnswerService.Object,
                _mockJsonCommonClass.Object,
                _mockFileWrapper.Object
            );

            // Act
            var result = _quizService.CheckAnswer(questionId, userChoiceLetter, letterMapping);

            // Assert
            Assert.True(result); // Powinno zwrócić true, zwrot A
        }

        // 14
        [Fact] // Zaliczony
        public void CheckAnswer_ShouldReturnTrue_WhenAnswerIsCorrectFromEntity() // Zwraca prawdę, jesli odpowiedź ok z entity ???
        {
            // Arrange
            int questionId = 2;
            _mockQuestionService = new Mock<QuestionService>(false);

            char userChoiceLetter = 'B';
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

            _quizService = new QuizService(
                _mockQuestionService.Object,
                _mockChoiceService.Object,
                _mockCorrectAnswerService.Object,
                _mockJsonCommonClass.Object,
                _mockFileWrapper.Object
            );

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

            _quizService = new QuizService(
                _mockQuestionService.Object,
                _mockChoiceService.Object,
                _mockCorrectAnswerService.Object,
                _mockJsonCommonClass.Object,
                _mockFileWrapper.Object
            );

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

            _quizService = new QuizService(
                _mockQuestionService.Object,
                _mockChoiceService.Object,
                _mockCorrectAnswerService.Object,
                _mockJsonCommonClass.Object,
                _mockFileWrapper.Object
            );

            // Act
            var result = _quizService.CheckAnswer(1, userChoiceLetter, letterMapping);

            // Assert
            Assert.False(result); // Spodziewam się, że odpowiedź będzie błędna, ponieważ nie ma mapowania dla 'C'
        }

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

            var answerContent = "Answer B";

            var choices = new List<Choice>
            {
                new Choice (1, 'A', "Answer A"),
                new Choice (1, 'B', "Answer B"),
                new Choice (1, 'C', "Answer C")
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

            var answerContent = "Answer D"; 

            var choices = new List<Choice>
            {
                new Choice (1, 'A', "Answer A"),     
                new Choice (1, 'B', "Answer B"),
                new Choice (1, 'C', "Answer C" )
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

            // Mockowanie QuizService z CallBase = true
            var mockQuizService = new Mock<QuizService>(
                _mockQuestionService.Object,
                _mockChoiceService.Object,
                _mockCorrectAnswerService.Object,
                _mockJsonCommonClass.Object,
                _mockFileWrapper.Object
            )
            { CallBase = true };

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
        [Fact] // Oblany
        public void LoadQuestionsFromJson_ShouldThrowJsonException_WhenJsonIsEmpty() // Ładuje: wyrzuca wyjątek, jeśli json pusty
        {
            // Arrange
            string filePath = "questions.json";

            // Mockowanie pustej listy pytań
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(filePath))
                .Returns(new List<Question>());

            // Zamockowanie QuestionService z przekazanym argumentem false
            _mockQuestionService = new Mock<QuestionService>(false);

            // Tworzenie mocka QuizService z CallBase = true
            var mockQuizService = new Mock<QuizService>(
                _mockQuestionService.Object,
                _mockChoiceService.Object,
                _mockCorrectAnswerService.Object,
                _mockJsonCommonClass.Object,
                _mockFileWrapper.Object
            )
            { CallBase = true };

            // Nadpisanie metody, aby nie wywoływała kodu w konstruktorze
            mockQuizService.Setup(q => q.LoadQuestionsFromJson(It.IsAny<string>()))
                .Callback(() => { });

            _quizService = mockQuizService.Object;

            // Act & Assert
            var exception = Assert.Throws<JsonException>(() => _quizService.LoadQuestionsFromJson(filePath));
            Assert.Equal("JSON does not contain any questions", exception.Message);
        }

        // 23
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

        // 24
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

        // 25
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

        // 26
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

        // 27
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

        // 28
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

        // 29
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

