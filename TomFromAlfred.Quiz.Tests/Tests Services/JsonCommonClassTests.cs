using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.ServiceSupport;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    // Oblane: 0 / 12

    public class JsonCommonClassTests
    {
        #region Create JsonCC Tests
        // 1
        [Fact] // Zaliczony
        public void CreateDefaultFile_ShouldCreateFile_WhenFileDoesNotExist() // Tworzy plik: jeśli ten nie istnieje
        {
            // Arrange
            var mockFile = new Mock<IFileWrapper>();

            string newFilePath = "defaultFile.json";

            var defaultNewData = new List<Question> { new Question(1, "pytanie przykładowe") };

            // Symuluję: plik nie istnieje → rzuca wyjątek
            mockFile.Setup(x => x.Exists(newFilePath)).Returns(false);

            mockFile.Setup(x => x.ReadAllText(newFilePath)).Throws<FileNotFoundException>();

            mockFile.Setup(x => x.WriteAllText(newFilePath, It.IsAny<string>()));

            var jsonCommon = new JsonCommonClass(mockFile.Object);

            // Act
            Action act = () => jsonCommon.CreateDefaultFile(newFilePath, defaultNewData);

            // Assert
            act.Should().NotThrow(); // Nie powinno być wyjątku

            mockFile.Verify(x => x.WriteAllText(newFilePath, It.IsAny<string>()), Times.Once);
        }

        // 2
        [Fact] // Zaliczony
        public void CreateDefaultFile_ShouldNotCreateFileAndShouldLogMessage_WhenFileAlreadyExists() // Nie tworzy: jeśli plik już istnieje i daje info
        {
            // Arrange
            var mockFile = new Mock<IFileWrapper>();

            string existingFilePath = "existingFile.json";

            var defaultNewData = new List<Question> { new Question(1, "pytanie przykładowe") };

            string jsonData = JsonConvert.SerializeObject(defaultNewData);

            mockFile.Setup(x => x.Exists(existingFilePath)).Returns(true);

            mockFile.Setup(x => x.ReadAllText(existingFilePath)).Returns(jsonData);

            var jsonCommon = new JsonCommonClass(mockFile.Object);

            using var sw = new StringWriter();

            var originalOut = Console.Out;

            Console.SetOut(sw);

            // Act
            jsonCommon.CreateDefaultFile(existingFilePath, defaultNewData);

            // Restore
            Console.SetOut(originalOut);
            var log = sw.ToString();

            // Assert
            log.Should().Contain($"Plik {existingFilePath} już istnieje. Tworzenie domyślnego pliku pominięte.");

            mockFile.Verify(x => x.WriteAllText(existingFilePath, It.IsAny<string>()), Times.Never);
        }

        // 3
        [Fact] // Zaliczony
        public void CreateDefaultFile_ShouldThrow_WhenDefaultDataIsNull() // Nie tworzy: jeśli dana to null
        {
            // Arrange
            var mockFile = new Mock<IFileWrapper>();

            var jsonCommon = new JsonCommonClass(mockFile.Object);

            string path = "default.json";

            // Act
            Action act = () => jsonCommon.CreateDefaultFile<object>(path, null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
               .WithParameterName("data");
        }
        #endregion Create JsonCC Tests

        #region Write JsonCC Tests
        // 4
        [Fact] // Zaliczony
        public void WriteToFile_ShouldCreateFile_WithCorrectJson() // Zapisuje: jeśli dane poprawne
        {
            // Arrange
            var mockFile = new Mock<IFileWrapper>();

            string testFilePath = "mockedFile.json";

            var testData = new List<Question> { new Question(1, "Przykładowe pytanie") };

            string? serializedJson = null;

            mockFile
                .Setup(x => x.WriteAllText(testFilePath, It.IsAny<string>()))
                .Callback<string, string>((_, json) => serializedJson = json);

            var jsonCommon = new JsonCommonClass(mockFile.Object);

            // Act
            jsonCommon.WriteToFile(testFilePath, testData);

            // Assert
            serializedJson.Should().NotBeNullOrWhiteSpace();

            serializedJson.Should().Contain("Przykładowe pytanie");

            mockFile.Verify(x => x.WriteAllText(testFilePath, It.IsAny<string>()), Times.Once);
        }

        // 5
        [Fact] // Zaliczony
        public void WriteToFile_ShouldCreateEmptyJson_WhenDataIsEmptyList() // Zapisuje: tworzy pusty json, jeśli lista danych jest pusta
        {
            // Arrange
            var mockFile = new Mock<IFileWrapper>();

            string testFilePath = "emptyList.json";

            var emptyData = new List<Question>();

            string? writtenJson = null;

            mockFile
                .Setup(x => x.WriteAllText(testFilePath, It.IsAny<string>()))
                .Callback<string, string>((_, json) => writtenJson = json);

            var jsonCommon = new JsonCommonClass(mockFile.Object);

            // Act
            jsonCommon.WriteToFile(testFilePath, emptyData);

            // Assert
            writtenJson.Should().Be("[]");

            mockFile.Verify(x => x.WriteAllText(testFilePath, "[]"), Times.Once);
        }

        // 6
        [Fact] // Zaliczony
        public void WriteToFile_ShouldThrowArgumentNullException_WhenDataIsNull() // Zapisuje: wyrzuca wyjątek, jeśli dane to null
        {
            // Arrange
            var mockFile = new Mock<IFileWrapper>();

            var jsonCommon = new JsonCommonClass(mockFile.Object);

            string testFilePath = "nullData.json";

            // Act
            Action act = () => jsonCommon.WriteToFile<object>(testFilePath, null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
               .WithParameterName("data");
        }
        
        // 7
        [Fact] // Zaliczony
        public void WriteToFile_ShouldCreateFile_WithSingleObjectJson() // Zapisuje: tworzy plik z pojedynczym obiektem
        {
            // Arrange
            var mockFile = new Mock<IFileWrapper>();

            string testFilePath = "singleObject.json";

            var question = new Question(0, "Z ilu części składa się powieść Alfreda Szklarskiego?");

            string? writtenJson = null;

            mockFile
                .Setup(x => x.WriteAllText(testFilePath, It.IsAny<string>()))
                .Callback<string, string>((_, json) => writtenJson = json);

            var jsonCommon = new JsonCommonClass(mockFile.Object);

            // Act
            jsonCommon.WriteToFile(testFilePath, question);

            // Assert
            writtenJson.Should().NotBeNullOrWhiteSpace();

            writtenJson.Should().Contain("Z ilu części składa się powieść Alfreda Szklarskiego?");

            mockFile.Verify(x => x.WriteAllText(testFilePath, It.IsAny<string>()), Times.Once);
        }

        // 8
        [Fact] // Zaliczony
        public void WriteToFile_ShouldThrowException_WhenFilePathIsInvalid() // Zapisuje: wyrzuca wyjątek, jeśli ścieżka pliku wadliwa
        {
            // Arrange
            var mockFile = new Mock<IFileWrapper>();

            string invalidPath = "?:\\invalid\\path.json";

            var testData = new List<Question> { new Question(1, "Z ilu części składa się powieść Alfreda N?") };

            mockFile
                .Setup(x => x.WriteAllText(invalidPath, It.IsAny<string>()))
                .Throws<DirectoryNotFoundException>();

            var jsonCommon = new JsonCommonClass(mockFile.Object);

            // Act
            Action act = () => jsonCommon.WriteToFile(invalidPath, testData);

            // Assert
            act.Should().Throw<DirectoryNotFoundException>();
        }
        #endregion  Write JsonCC Tests

        #region Read JsonCC Tests
        // 9
        [Fact] // Zaliczony
        public void ReadFromFile_ShouldReturnCorrectData() // Odczytuje: zwraca poprawne dane
        {
            // Arrange
            var mockFile = new Mock<IFileWrapper>();

            var filePath = "testRead.json";

            var expected = new List<Question> { new Question(1, "Jakie zwierzę jest królem dżungli?") };

            string json = JsonConvert.SerializeObject(expected);

            mockFile.Setup(x => x.Exists(filePath)).Returns(true);

            mockFile.Setup(x => x.ReadAllText(filePath)).Returns(json);

            var jsonCommon = new JsonCommonClass(mockFile.Object);

            // Act
            var result = jsonCommon.ReadFromFile<List<Question>>(filePath);

            // Assert
            result.Should().NotBeNull();

            result.Should().HaveCount(1);

            result[0].QuestionId.Should().Be(1);

            result[0].QuestionContent.Should().Be("Jakie zwierzę jest królem dżungli?");
        }

        // 10
        [Fact] // Zaliczony
        public void ReadFromFile_ShouldThrowFileNotFoundException_WhenFileDoesNotExist() // Odczytuje: wyrzuca wyjątek, jeśli plik nie istnieje
        {
            // Arrange
            var mockFile = new Mock<IFileWrapper>();

            string nonExistentFile = "doesNotExist.json";

            mockFile.Setup(x => x.Exists(nonExistentFile)).Returns(false);

            var jsonCommon = new JsonCommonClass(mockFile.Object);

            // Act
            Action act = () => jsonCommon.ReadFromFile<List<Question>>(nonExistentFile);

            // Assert
            act.Should().Throw<FileNotFoundException>()
               .WithMessage($"Plik {nonExistentFile} nie istnieje.");
        }

        // 11
        [Fact] // Zaliczony
        public void ReadFromFile_ShouldThrowInvalidDataException_WhenFileIsEmpty() // Odczytuje: wyrzuca wyjątek, jeśli plik jest pusty
        {
            // Arrange
            var mockFile = new Mock<IFileWrapper>();

            string emptyFilePath = "emptyFile.json";

            mockFile.Setup(x => x.Exists(emptyFilePath)).Returns(true);

            mockFile.Setup(x => x.ReadAllText(emptyFilePath)).Returns(string.Empty);

            var jsonCommon = new JsonCommonClass(mockFile.Object);

            // Act
            Action act = () => jsonCommon.ReadFromFile<List<Question>>(emptyFilePath);

            // Assert
            act.Should().Throw<JsonReaderException>()
               .WithMessage($"Plik {emptyFilePath} jest pusty lub zawiera nieprawidłowe dane.");
        }

        // 12
        [Fact] // Zaliczony
        public void ReadFromFile_ShouldThrowJsonReaderException_WhenFileContainsInvalidJson() //Odczytuje: wyrzuca wyjatek, jeśli json ma błędny format
        {
            // Arrange
            var mockFile = new Mock<IFileWrapper>();

            string invalidJsonFilePath = "invalidJson.json";

            string invalidJson = "{ niepoprawny JSON }"; // Błędny format

            mockFile.Setup(x => x.Exists(invalidJsonFilePath)).Returns(true);

            mockFile.Setup(x => x.ReadAllText(invalidJsonFilePath)).Returns(invalidJson);

            var jsonCommon = new JsonCommonClass(mockFile.Object);

            // Act
            Action act = () => jsonCommon.ReadFromFile<List<Question>>(invalidJsonFilePath);

            // Assert
            act.Should().Throw<JsonReaderException>();
        }
        #endregion Read JsonCC Tests
    }
}
