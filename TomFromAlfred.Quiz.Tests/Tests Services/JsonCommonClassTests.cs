using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    // Oblane: w trakcie tworzenia testów

    public class JsonCommonClassTests
    {
        private readonly Mock<JsonCommonClass> _mockJsonCommonClass;

        public JsonCommonClassTests()
        {
            _mockJsonCommonClass = new Mock<JsonCommonClass> { CallBase = true};
        }

        // 1
        [Fact] // Zaliczony
        public void CreateDefaultFile_ShouldCreateFile_WhenFileDoesNotExist() // Tworzy plik: jeśli ten nie istnieje
        {
            // Arrange
            string newFilePath = "defaultFile.json";
            var defaultNewData = new List<Question> { new Question(1, "pytanie przykładowe") };

            // Mockowanie odczytu pliku → rzuca FileNotFoundException, czyli plik nie istnieje
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(newFilePath)).Throws(new FileNotFoundException());

            // Mockowanie zapisu
            _mockJsonCommonClass.Setup(x => x.WriteToFile(newFilePath, defaultNewData));

            // Act
            _mockJsonCommonClass.Object.CreateDefaultFile(newFilePath, defaultNewData);

            // Assert: Upewniam się, czy WriteToFile() zostało wywołane
            _mockJsonCommonClass.Verify(x => x.WriteToFile(newFilePath, defaultNewData), Times.Once);
        }

        // 2
        [Fact] // Zaliczony
        public void CreateDefaultFile_ShouldNotCreateFile_WhenFileAlreadyExists() // Nie tworzy: jeśli plik już istnieje
        {
            // Arrange
            string existingFilePath = "existingFile.json";
            var defaultNewData = new List<Question> { new Question(1, "pytanie przykładowe") };

            // Mockowanie odczytu pliku → zwraca dane, czyli plik istnieje
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(existingFilePath)).Returns(defaultNewData);

            // Act
            _mockJsonCommonClass.Object.CreateDefaultFile(existingFilePath, defaultNewData);

            // Assert: Sprawdzam, czy WriteToFile() nie zostało wywołane
            _mockJsonCommonClass.Verify(x => x.WriteToFile(existingFilePath, defaultNewData), Times.Never);
        }

        // 3
        [Fact] // Zaliczony
        public void WriteToFile_ShouldCreateFile_WithCorrectJson() // Zapisuje: jeśli dane poprawne
        {
            // Arrange
            string testFilePath = "testFile.json";
            var testData = new List<Question> { new Question(1, "Przykładowe pytanie") };
            var jsonCommon = new JsonCommonClass();

            // Act
            jsonCommon.WriteToFile(testFilePath, testData);

            // Assert
            Assert.True(File.Exists(testFilePath)); // Sprawdza, czy plik został utworzony

            string jsonContent = File.ReadAllText(testFilePath);
            Assert.Contains("Przykładowe pytanie", jsonContent); // Sprawdza, czy plik zawiera poprawne dane

            // Cleanup
            File.Delete(testFilePath);
        }

        [Fact]
        public void WriteToFile_ShouldCreateEmptyJson_WhenDataIsEmptyList()
        {
            // Arrange
            string testFilePath = "emptyList.json";
            var emptyData = new List<Question>(); // Pusta lista
            var jsonCommon = new JsonCommonClass();

            // Act
            jsonCommon.WriteToFile(testFilePath, emptyData);

            // Assert
            Assert.True(File.Exists(testFilePath));

            string jsonContent = File.ReadAllText(testFilePath);
            Assert.Equal("[]", jsonContent.Trim()); // JSON pustej listy powinien wyglądać jak []

            // Cleanup
            File.Delete(testFilePath);
        }

        [Fact]
        public void WriteToFile_ShouldThrowArgumentNullException_WhenDataIsNull()
        {
            // Arrange
            string testFilePath = "nullData.json";
            var jsonCommon = new JsonCommonClass();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => jsonCommon.WriteToFile<object>(testFilePath, null));
        }

        [Fact]
        public void WriteToFile_ShouldCreateFile_WithSingleObjectJson()
        {
            // Arrange
            string testFilePath = "singleObject.json";
            var question = new Question(1, "Pytanie testowe");
            var jsonCommon = new JsonCommonClass();

            // Act
            jsonCommon.WriteToFile(testFilePath, question);

            // Assert
            Assert.True(File.Exists(testFilePath));

            string jsonContent = File.ReadAllText(testFilePath);
            Assert.Contains("\"Pytanie testowe\"", jsonContent); // Sprawdza, czy plik zawiera poprawne dane

            // Cleanup
            File.Delete(testFilePath);
        }

        [Fact]
        public void WriteToFile_ShouldThrowException_WhenFilePathIsInvalid()
        {
            // Arrange
            string invalidPath = "?:\\invalid\\path.json"; // Nieprawidłowa ścieżka
            var testData = new List<Question> { new Question(1, "Przykładowe pytanie") };
            var jsonCommon = new JsonCommonClass();

            // Act & Assert
            Assert.Throws<DirectoryNotFoundException>(() => jsonCommon.WriteToFile(invalidPath, testData));
        }


        [Fact]
        public void ReadFromFile_ShouldReturnCorrectData()
        {
            // Arrange
            string testFilePath = "testFile.json";
            var expectedData = new List<Question> { new Question(1, "Przykładowe pytanie") };
            File.WriteAllText(testFilePath, JsonConvert.SerializeObject(expectedData));

            var jsonCommon = new JsonCommonClass();

            // Act
            var result = jsonCommon.ReadFromFile<List<Question>>(testFilePath);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result[0].QuestionId);
            Assert.Equal("Przykładowe pytanie", result[0].QuestionContent);

            // Cleanup
            File.Delete(testFilePath);
        }

        [Fact]
        public void ReadFromFile_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
        {
            // Arrange
            string nonExistentFile = "doesNotExist.json";
            var jsonCommon = new JsonCommonClass();

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => jsonCommon.ReadFromFile<List<Question>>(nonExistentFile));
        }

        [Fact]
        public void ReadFromFile_ShouldThrowInvalidDataException_WhenFileIsEmpty()
        {
            // Arrange
            string emptyFilePath = "emptyFile.json";
            File.WriteAllText(emptyFilePath, ""); // Tworzymy pusty plik
            var jsonCommon = new JsonCommonClass();

            // Act & Assert
            Assert.Throws<InvalidDataException>(() => jsonCommon.ReadFromFile<List<Question>>(emptyFilePath));

            // Cleanup
            File.Delete(emptyFilePath);
        }
    }
}
