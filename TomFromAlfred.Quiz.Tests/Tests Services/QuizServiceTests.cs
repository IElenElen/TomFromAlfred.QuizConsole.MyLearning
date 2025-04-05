using System.Collections.Generic;
using System.Text.Json;
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
            string validFilePath = "validFilePath.json";

            var mockQuestions = new List<Question>
            {
                new Question(1, "Z ilu części składa się powieść Alfreda Szklarskiego?")
            };

            _mockJsonCommonClass.Setup(s => s.ReadFromFile<List<Question>>(It.IsAny<string>()))
                .Returns(mockQuestions);

            // Act
            Action action = () => _quizService.InitializeJsonService(null, validFilePath);

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("jsonService");
        }

        // 3 
        [Fact] // Zaliczony
        public void InitializeJsonService_ShouldThrowFileNotFoundException_WhenFileDoesNotExist() // Inicjalizacja: wyrzuca, jeśli plik nie istnieje
        {
            // Arrange
            string nonExistentFilePath = "nonexistent.json";
            _mockFileWrapper.Setup(f => f.Exists(nonExistentFilePath)).Returns(false);

            // Act
            var exception = Record.Exception(() =>
                _quizService.InitializeJsonService(_mockJsonCommonClass.Object, nonExistentFilePath)
            );

            // Assert
            exception.Should().BeOfType<FileNotFoundException>()
                .Which.Message.Should().Contain("Plik nonexistent.json nie istnieje.");
        }
        #endregion InitializeJsonS QuizS. Tests

        #region GetAllQuestions QuizS. Tests
        // 4
        [Fact] // Zaliczony
        public void GetAllQuestions_ShouldReturnQuestionsFromJsonAndService() // Zwraca: pytania z serwisu i pliku json
        {
            // Arrange
            var jsonQuestions = new List<Question>
            {
                new Question(1, "Z JSON 1"),
                new Question(2, "Z JSON 2")
            };
                    
            var serviceQuestions = new List<Question>
            {
                new Question(3, "Z serwisu 3")
            };

            _mockQuestionService.Setup(s => s.GetAllActive()).Returns(serviceQuestions);

            _quizService.SetTestData(jsonQuestions, new(), new());

            // Act
            var result = _quizService.GetAllQuestions().ToList();

            // Assert
            result.Should().HaveCount(3)
                .And.Contain(q => q.QuestionId == 1)
                .And.Contain(q => q.QuestionId == 2)
                .And.Contain(q => q.QuestionId == 3);
        }

        // 5
        [Fact] // Zaliczony - wreszcie
        public void GetAllQuestions_ShouldReturnQuestionsFromServiceWhenJsonIsEmpty() // Zwraca: pytania z serwisu, jeśli json pusty
        {
            // Arrange
            var serviceQuestions = new List<Question>
            {
                new Question(1, "Z serwisu")
            };

            _mockQuestionService.Setup(s => s.GetAllActive()).Returns(serviceQuestions);
            _quizService.SetTestData(new(), new(), new());

            // Act
            var result = _quizService.GetAllQuestions();

            // Assert
            result.Should().HaveCount(1)
                .And.ContainSingle(q => q.QuestionId == 1);
        }

        // 6
        [Fact] // Zaliczony
        public void GetAllQuestions_ShouldReturnQuestionsFromJsonWhenServiceIsEmpty() // Zwraca: pytania z json, jeśli serwis pusty
        {
            // Arrange
                var jsonQuestions = new List<Question>
                {
                    new Question(1, "Z JSON")
                };

            _mockQuestionService.Setup(s => s.GetAllActive()).Returns(new List<Question>());

            _quizService.SetTestData(jsonQuestions, new(), new());

            // Act
            var result = _quizService.GetAllQuestions();

            // Assert
            result.Should().HaveCount(1)
                .And.ContainSingle(q => q.QuestionId == 1);
        }
        #endregion GetAllQuestions QuizS. Tests

        #region GetChoicesForQuestion QuizS. Tests
        // 7
        [Fact] // Zaliczony
        public void GetChoicesForQuestion_ShouldReturnChoicesFromJson_WhenChoicesExistInJson() // Zwraca: wybory z jsona, jeśli wybory te istnieją
        {
            // Arrange
            var questionId = 1;
            var choices = new List<Choice>
            {
                new Choice(1, 'A', "odp A"),
                new Choice(1, 'B', "odp B")
            };

            _quizService.SetTestData(new(), new Dictionary<int, List<Choice>> { { questionId, choices } }, new());

            // Act
            var result = _quizService.GetChoicesForQuestion(questionId);

            // Assert
            result.Should().HaveCount(2)
                .And.Contain(c => c.ChoiceLetter == 'A' && c.ChoiceContent == "odp A")
                .And.Contain(c => c.ChoiceLetter == 'B' && c.ChoiceContent == "odp B");
        }

        // 8
        [Fact] // Zaliczony
        public void GetChoicesForQuestion_ShouldReturnChoicesFromService_WhenChoicesNotExistInJson() // Daje wybory z serwisu, jeśli w json ich brak
        {
            // Arrange
            var questionId = 2;
            var serviceChoices = new List<Choice>
            {
                new Choice(2, 'A', "Wybór A"),
                new Choice(2, 'B', "Wybór B")
            };

            _mockChoiceService.Setup(s => s.GetChoicesForQuestion(questionId)).Returns(serviceChoices);

            _quizService.SetTestData(new(), new(), new()); // Brak JSON-a

            // Act
            var result = _quizService.GetChoicesForQuestion(questionId);

            // Assert
            result.Should().HaveCount(2)
                .And.Contain(c => c.ChoiceContent == "Wybór A")
                .And.Contain(c => c.ChoiceContent == "Wybor B");
        }

        // 9
        [Fact] // Zaliczony
        public void GetChoicesForQuestion_ShouldReturnEmpty_WhenNoChoicesFound() // Zwróci pustkę, jeśli wyborów nie znalazł
        {
            // Arrange
            var questionId = 3;
            _mockChoiceService.Setup(s => s.GetChoicesForQuestion(questionId)).Returns(new List<Choice>());

            _quizService.SetTestData(new(), new(), new());

            // Act
            var result = _quizService.GetChoicesForQuestion(questionId);

            // Assert
            result.Should().BeEmpty("Bo nie znaleziono żadnych wyborów w JSON ani w serwisie.");
        }
        #endregion GetChoicesForQuestion QuizS. Tests

        #region GetShuffledChFQ QuizS. Tests
        // 10 
        [Fact] // Zaliczony
        public void GetShuffledChoicesForQuestion_ShouldShuffleChoicesAndMapLetters() // Losuje wybory i mapuje litery
        {
            // Arrange
            var questionId = 1;

            var originalChoices = new List<Choice>
            {
                new Choice(1, 'A', "A"),
                new Choice(1, 'B', "B"),
                new Choice(1, 'C', "C")
            };

            _quizService.SetTestData(new(), new Dictionary<int, List<Choice>> { { questionId, originalChoices } }, new());

            // Act
            var shuffled = _quizService.GetShuffledChoicesForQuestion(questionId, out var letterMap).ToList();

            // Assert
            shuffled.Should().HaveCount(3);

            letterMap.Should().HaveCount(3);

            letterMap.Values.Should().BeSubsetOf(new[] { 'A', 'B', 'C' });

            // Nie sprawdzam dokładnej kolejności, ale upewniam się, że coś się zmieniło
            shuffled.Select(c => c.ChoiceContent)
                .Should().NotBeInAscendingOrder("Bo odpowiedzi powinny zostać przetasowane.");
        }

        // 11
        [Fact] // Zaliczony
        public void GetShuffledChoicesForQuestion_ShouldReturnSameNumberOfChoices() // Przy losowaniu Quizu, zawsze zwraca tę samą liczbę wyborów w zestawie
        {
            // Arrange
            var questionId = 2;

            var originalChoices = new List<Choice>
            {
                new Choice(questionId, 'A', "Wybór A"),
                new Choice(questionId, 'B', "Wybór B"),
                new Choice(questionId, 'C', "Wybór C")
            };

            _quizService.SetTestData(
                questions: new(),
                choices: new Dictionary<int, List<Choice>> { { questionId, originalChoices } },
                correctAnswers: new()
            );

            // Act
            var shuffled = _quizService.GetShuffledChoicesForQuestion(questionId, out var mapping).ToList();

            // Assert
            shuffled.Should().HaveSameCount(originalChoices)
                .And.OnlyContain(c => c.ChoiceContent.StartsWith("Wybór"));
        }

        // 12
        [Fact] // Zaliczony
        public void GetShuffledChoicesForQuestion_ShouldCreateCorrectLetterMapping() // Tworzy poprawną mapę liter
        {
            // Arrange
            var questionId = 3;

            var originalChoices = new List<Choice>
            {
                new Choice(questionId, 'A', "Opcja A"),
                new Choice(questionId, 'B', "Opcja B"),
                new Choice(questionId, 'C', "Opcja C")
            };

            _quizService.SetTestData(
                questions: new(),
                choices: new Dictionary<int, List<Choice>> { { questionId, originalChoices } },
                correctAnswers: new()
            );

            // Act
            var shuffled = _quizService.GetShuffledChoicesForQuestion(questionId, out var mapping).ToList();

            // Assert
            mapping.Should().HaveCount(3)
                .And.ContainKey('A')
                .And.ContainKey('B')
                .And.ContainKey('C');

            mapping.Values.Should().OnlyContain(l => new[] { 'A', 'B', 'C' }.Contains(l));
        }
        #endregion GetShuffledChFQ QuizS. Tests

        #region CheckAnswer QuizS. Tests
        // 13
        [Fact] // Zaliczony
        public void CheckAnswer_ShouldReturnTrue_WhenAnswerIsCorrectFromJson() // Zwraca prawdę, jeśli odpowiedź poprawna
        {
            // Arrange
            int questionId = 1;
            char userLetter = 'B'; // B → A
            var mapping = new Dictionary<char, char> { { 'A', 'C' }, { 'B', 'A' }, { 'C', 'B' } };

            var questions = new List<Question> { new Question(questionId, "Pytanie") };

            var choices = new List<Choice>
            {
                new Choice(questionId, 'A', "Answer A"),
                new Choice(questionId, 'B', "Answer B"),
                new Choice(questionId, 'C', "Answer C")
            };
            var correctAnswers = new Dictionary<int, string> { { questionId, "A" } };

            _quizService.SetTestData(questions, new() { { questionId, choices } }, correctAnswers);

            // Act
            var result = _quizService.CheckAnswer(questionId, userLetter, mapping);

            // Assert
            result.Should().BeTrue("Bo zamapowana odpowiedź odpowiada poprawnej odpowiedzi z JSON.");
        }

        // 14
        [Fact] // Zaliczony
        public void CheckAnswer_ShouldReturnTrue_WhenAnswerIsCorrectFromEntity() // Zwraca prawdę, jesli odpowiedź ok z entity 
        {
            // Arrange
            int questionId = 2;
            char userLetter = 'C'; // C → B
            var mapping = new Dictionary<char, char> { { 'A', 'C' }, { 'B', 'A' }, { 'C', 'B' } };

            var questions = new List<Question> { new Question(questionId, "Pytanie") };
            var choices = new List<Choice>
            {
                new Choice(questionId, 'A', "Answer A"),
                new Choice(questionId, 'B', "Answer B"),
                new Choice(questionId, 'C', "Answer C")
            };

            _mockCorrectAnswerService.Setup(x => x.GetCorrectAnswerForQuestion(questionId))
                .Returns(new CorrectAnswer(questionId, "Answer B", true));

            _quizService.SetTestData(questions, new() { { questionId, choices } }, new());

            // Act
            var result = _quizService.CheckAnswer(questionId, userLetter, mapping);

            // Assert
            result.Should().BeTrue("bo odpowiedź z entity pasuje do wyboru użytkownika");
        }

        // 15 
        [Fact] // Zaliczony
        public void CheckAnswer_ShouldReturnFalse_WhenAnswerIsIncorrect() // Sprawdza: błąd, jeśli odpowiedź błędna
        {
            // Arrange
            int questionId = 3;
            char userLetter = 'B'; // B → A
            var mapping = new Dictionary<char, char> { { 'A', 'C' }, { 'B', 'A' }, { 'C', 'B' } };

            var questions = new List<Question> { new Question(questionId, "Pytanie") };
            var choices = new List<Choice>
            {
                new Choice(questionId, 'A', "Answer C"),
                new Choice(questionId, 'B', "Answer B"),
                new Choice(questionId, 'C', "Answer A")
            };

            _mockCorrectAnswerService.Setup(x => x.GetCorrectAnswerForQuestion(questionId))
                .Returns(new CorrectAnswer(questionId, "A", true)); // Oczekiwana: "A", wybór: "C"

            _quizService.SetTestData(questions, new() { { questionId, choices } }, new());

            // Act
            var result = _quizService.CheckAnswer(questionId, userLetter, mapping);

            // Assert
            result.Should().BeFalse("bo odpowiedź użytkownika nie pasuje do żadnej poprawnej odpowiedzi");
        }

        // 16
        [Fact] // Zaliczony
        public void CheckAnswer_ShouldReturnFalse_WhenLetterMappingDoesNotContainUserChoice() // Sprawdza: podaje błąd, jeśli błędna litera użytkownika
        {
            // Arrange
            int questionId = 1;
            char userLetter = 'D'; // Brak w mapowaniu
            var mapping = new Dictionary<char, char> { { 'A', 'B' }, { 'B', 'C' } };

            var questions = new List<Question> { new Question(questionId, "Pytanie") };
            var choices = new List<Choice>
            {
                new Choice(questionId, 'A', "Answer A"),
                new Choice(questionId, 'B', "Answer B"),
                new Choice(questionId, 'C', "Answer C")
            };

            _quizService.SetTestData(questions, new() { { questionId, choices } }, new());

            // Act
            var result = _quizService.CheckAnswer(questionId, userLetter, mapping);

            // Assert
            result.Should().BeFalse("bo użytkownik podał literę, której nie ma w mapowaniu");
        }

        // 17
        [Fact] // Zaliczony
        public void CheckAnswer_ShouldReturnFalse_WhenLetterMappingIsIncorrect() // Sprawdza: podaje błąd czyli???, jeśli mapowanie niepoprawne
        {
            // Arrange
            int questionId = 1;
            char userLetter = 'B'; // B → D (błąd)
            var mapping = new Dictionary<char, char> { { 'A', 'C' }, { 'B', 'D' } };

            var questions = new List<Question> { new Question(questionId, "Pytanie") };
            var choices = new List<Choice>
            {
                new Choice(questionId, 'A', "Answer A"),
                new Choice(questionId, 'B', "Answer B"),
                new Choice(questionId, 'C', "Answer C")
            };

            _quizService.SetTestData(questions, new() { { questionId, choices } }, new());

            // Act
            var result = _quizService.CheckAnswer(questionId, userLetter, mapping);

            // Assert
            result.Should().BeFalse("bo zamapowana litera nie istnieje w wyborach");
        }

        // 18
        [Fact] // Zaliczony
        public void CheckAnswer_ShouldReturnFalse_WhenLetterMappingIsMissing() // Sprawdza: podaje błąd, jeśli nie ma litery zmapowanej
        {
            // Arrange
            int questionId = 1;
            char userLetter = 'C'; // Brak w mapowaniu
            var mapping = new Dictionary<char, char> { { 'A', 'X' }, { 'B', 'Y' } };

            var questions = new List<Question> { new Question(questionId, "Pytanie") };
            var choices = new List<Choice>
            {
                new Choice(questionId, 'A', "Answer X"),
                new Choice(questionId, 'B', "Answer Y")
            };

            _mockCorrectAnswerService.Setup(ca => ca.GetCorrectAnswerForQuestion(questionId))
                .Returns(new CorrectAnswer(questionId, "X", true));

            _quizService.SetTestData(questions, new() { { questionId, choices } }, new());

            // Act
            var result = _quizService.CheckAnswer(questionId, userLetter, mapping);

            // Assert
            result.Should().BeFalse("bo brak mapowania dla litery użytkownika");
        }
        #endregion CheckAnswer QuizS. Tests

        #region GetLetterForAnswer QuizS. Tests
        // 19
        [Fact] // Zaliczony
        public void GetLetterForAnswer_ShouldReturnCorrectLetterForAnswer() // Podaje: literę dla poprawnej odpowiedzi
        {
            // Arrange
            int questionId = 1;
            char letterToCheck = 'B';

            var questions = new List<Question> { new Question(questionId, "Jakie jest pytanie?") };

            var choices = new List<Choice>
            {
                new Choice(questionId, 'A', "Answer A"),
                new Choice(questionId, 'B', "Answer B"),
                new Choice(questionId, 'C', "Answer C")
            };

            _quizService.SetTestData(questions, new() { { questionId, choices } }, new());

            // Act
            var result = _quizService.GetAnswerContentFromLetter(letterToCheck, questionId);

            // Assert
            result.Should().Be("Answer B");
        }

        // 20
        [Fact] // Zaliczony
        public void GetLetterForAnswer_ShouldReturnDefaultLetter_WhenNoMatchFound() // Podaje: domyślną literę, jeśli powiązania nie znajdzie
        {
            // Arrange
            int questionId = 1;

            char letterToCheck = 'D'; // Nie istnieje

            var questions = new List<Question> { new Question(questionId, "Placeholder") };

            var choices = new List<Choice>
            {
                new Choice(questionId, 'A', "Answer A"),
                new Choice(questionId, 'B', "Answer B"),
                new Choice(questionId, 'C', "Answer C")
            };

            _quizService.SetTestData(questions, new() { { questionId, choices } }, new());

            // Act
            var result = _quizService.GetAnswerContentFromLetter(letterToCheck, questionId);

            // Assert
            result.Should().BeNull("bo nie znaleziono dopasowania dla podanej litery");
        }
        #endregion GetLetterForAnswer QuizS. Tests

        #region LoadQFJson QuizS. Tests
        // 21
        [Fact] // Zaliczony
        public void LoadQuestionsFromJson_ShouldReturn_WhenFileDoesNotExist() // Ładuje: zwraca, jeśli brak pliku z pytaniami
        {
            // Arrange
            // Arrange
            var filePath = "nonexistentFile.json";
            _mockFileWrapper.Setup(f => f.Exists(filePath)).Returns(false);

            // Act
            var act = () => _quizService.LoadQuestionsFromJson(filePath);

            // Assert
            act.Should().NotThrow("bo brak pliku powinien być obsłużony bez wyjątku");
        }

        // 22
        [Fact] // Zaliczony
        public void LoadQuestionsFromJson_ShouldThrowJsonException_WhenJsonIsEmpty() // Ładuje: wyrzuca wyjątek, jeśli json pusty
        {
            // Arrange
            var filePath = "questions.json";
            _mockFileWrapper.Setup(f => f.Exists(filePath)).Returns(true);
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(filePath))
                .Returns(new List<Question>()); // Pusty JSON

            // Act
            var act = () => _quizService.LoadQuestionsFromJson(filePath);

            // Assert
            act.Should().Throw<JsonException>()
                .WithMessage("*does not contain any questions*");
        }

        // 23
        [Fact] // Zaliczony
        public void LoadQuestionsFromJson_ShouldLoadQuestions_WhenJsonIsValid() // Ładuje: pytania, jeśli json jest prawidłowy
        {
            // Arrange
            var filePath = "questions.json";
            var questions = new List<Question> { new Question(1, "What is 2 + 2?") };

            _mockFileWrapper.Setup(f => f.Exists(filePath)).Returns(true);
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(filePath)).Returns(questions);

            // Act
            _quizService.LoadQuestionsFromJson(filePath);

            // Assert
            _quizService.GetAllQuestions()
                .Should().ContainSingle(q => q.QuestionId == 1 && q.QuestionContent == "What is 2 + 2?");
        }
        #endregion LoadQFJson QuizS. Tests

        #region LoadChFJson QuizS. Tests
        // 24
        [Fact] // Zaliczony
        public void LoadChoicesFromJson_ShouldReturn_WhenChoicesFileDoesNotExist() // Ładuje: wyrzuca, jeśli plik wyborów nie istnieje
        {
            // Arrange
            _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(false);
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Choice>>(It.IsAny<string>()))
                .Returns(new List<Choice>());

            // Act
            var act = () => _quizService.LoadChoicesFromJson();

            // Assert
            act.Should().NotThrow("bo brak pliku z odpowiedziami powinien być bezpiecznie obsłużony");
        }

        // 25
        [Fact] // Zaliczony
        public void LoadChoicesFromJson_ShouldLoadChoices_WhenFileIsValid() // Ładuje: wybory, jeśli plik jest prawidłowy
        {
            // Arrange
            var questionId = 1;
            var choices = new List<Choice>
            {
                new Choice(questionId, 'A', "Answer A"),
                new Choice(questionId, 'B', "Answer B")
            };

            _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Choice>>(It.IsAny<string>())).Returns(choices);

            // Act
            _quizService.LoadChoicesFromJson();

            // Assert
            _quizService.GetChoicesForQuestion(questionId)
                .Should().HaveCount(2)
                .And.Contain(c => c.ChoiceLetter == 'A' && c.ChoiceContent == "Answer A")
                .And.Contain(c => c.ChoiceLetter == 'B' && c.ChoiceContent == "Answer B");
        }
        #endregion  LoadChFJson QuizS. Tests

        #region LoadCorrectSFJson QuizS. Tests
        // 26
        [Fact] // Zaliczony
        public void LoadCorrectSetFromJson_ShouldReturn_WhenCorrectSetFileDoesNotExist() // Ładuje: podaje wyjątek, jeśli plik nie istnieje
        {
            /// Arrange
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<JsonHelper>>(It.IsAny<string>()))
                .Throws(new FileNotFoundException());

            // Act
            var act = () => _quizService.LoadCorrectSetFromJson();

            // Assert
            act.Should().NotThrow("bo brak pliku powinien być obsłużony bez wyjątku");
        }

        // 27
        [Fact] // Zaliczony
        public void LoadCorrectSetFromJson_ShouldLoadCorrectAnswers_WhenFileIsValid() // Ładuje: poprawne odpowiedzi, jeśli plik jest ok 
        {
            // Arrange
            var correctSet = new List<JsonHelper>
            {
                new JsonHelper
                {
                    QuestionNumber = 1,
                    LetterCorrectAnswer = "A",
                    ContentCorrectAnswer = "Answer A"
                }
            };

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<JsonHelper>>(It.IsAny<string>()))
                .Returns(correctSet);

            // Act
            _quizService.LoadCorrectSetFromJson();

            // Assert
            // Testujemy nie publiczny stan _correctAnswers (domena prywatna) przez publiczne zachowanie
            var mapping = new Dictionary<char, char> { { 'A', 'A' } };
            var choices = new List<Choice>
            {
                new Choice(1, 'A', "Answer A")
            };

            _quizService.SetTestData(new(), new() { { 1, choices } }, new());

            var result = _quizService.CheckAnswer(1, 'A', mapping);
            result.Should().BeTrue("bo poprawna odpowiedź z JSON została wczytana poprawnie");
        }
        #endregion LoadCorrectSFJson QuizS. Tests

        // 28
        [Fact] // Zaliczony
        public void AddQuestionToJson_ShouldAddQuestionAndSaveToFile() // Dodaje: pytanie i zapisuje
        {
            // Arrange
            var question = new Question(1, "Z ilu części składa się powieść Alfreda Szklarskiego?");
            _mockJsonCommonClass.Invocations.Clear();

            // Act
            _quizService.AddQuestionToJson(question);

            // Assert
            _mockJsonCommonClass.Verify(
                x => x.WriteToFile(It.IsAny<string>(), It.Is<List<Question>>(q => q.Contains(question))),
                Times.Once);
        }

        // 29
        [Fact] // Zaliczony
        public void SaveQuestionsToJson_ShouldHandleFileWriteException()
        {
            // Arrange
            var question = new Question(1, "Z ilu części składa się powieść Alfreda Szklarskiego?");
            _quizService.SetTestData(new List<Question> { question }, new(), new());

            _mockJsonCommonClass.Setup(x => x.WriteToFile(It.IsAny<string>(), It.IsAny<List<Question>>()))
                .Throws(new Exception("Błąd podczas zapisywania pytań do pliku JSON"));

            #if DEBUG
            // Act & Assert
            var act = () => _quizService.SaveQuestionsToJson();

            act.Should().Throw<Exception>()
                .WithMessage("Błąd podczas zapisywania pytań do pliku JSON");
            #else
            // Act & Assert
            var act = () => _quizService.SaveQuestionsToJson();

            act.Should().NotThrow("bo poza DEBUG wyjątek nie powinien być propagowany");
            #endif
        }
    }
}

