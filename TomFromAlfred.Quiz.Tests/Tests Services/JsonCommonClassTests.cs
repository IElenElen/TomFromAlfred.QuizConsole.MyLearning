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
    // Oblane: 0 / 12

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
        public void CreateDefaultFile_ShouldNotCreateFileAndShouldLogMessage_WhenFileAlreadyExists() // Nie tworzy: jeśli plik już istnieje i daje info
        {
            // Arrange
            string existingFilePath = "existingFile.json";
            var defaultNewData = new List<Question> { new Question(1, "pytanie przykładowe") };

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(existingFilePath))
                                .Returns(defaultNewData);

            _mockJsonCommonClass.Setup(x => x.WriteToFile(existingFilePath, defaultNewData));

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw); 

                // Act
                _mockJsonCommonClass.Object.CreateDefaultFile(existingFilePath, defaultNewData);

                // Assert
                var logOutput = sw.ToString();

                Assert.Contains($"Plik {existingFilePath} już istnieje. Tworzenie domyślnego pliku pominięte.", logOutput);
                _mockJsonCommonClass.Verify(x => x.WriteToFile(existingFilePath, defaultNewData), Times.Never);
            }
        }

        // 3
        [Fact] // Zaliczony
        public void CreateDefaultFile_ShouldThrow_WhenDefaultDataIsNull() // Nie tworzy: jeśli dana to null
        {
            // Arrange
            string path = "default.json";
            var jsonCommon = new JsonCommonClass();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => jsonCommon.CreateDefaultFile<object>(path, null));
        }

        // 4
        [Fact] // Zaliczony
        public void WriteToFile_ShouldCreateFile_WithCorrectJson() // Zapisuje: jeśli dane poprawne
        {
            // Arrange
            string testFilePath = "mockedFile.json";
            var testData = new List<Question> { new Question(1, "Przykładowe pytanie") };
            var mockJsonCommon = new Mock<JsonCommonClass> { CallBase = true };

            mockJsonCommon.Setup(x => x.WriteToFile(testFilePath, It.IsAny<List<Question>>()));

            // Act
            mockJsonCommon.Object.WriteToFile(testFilePath, testData);

            // Assert
            mockJsonCommon.Verify(x => x.WriteToFile(testFilePath, testData), Times.Once);
        }

        // 5
        [Fact] // Zaliczony
        public void WriteToFile_ShouldCreateEmptyJson_WhenDataIsEmptyList() // Zapisuje: tworzy pusty json, jeśli lista danych jest pusta
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

        // 6
        [Fact] // Zaliczony
        public void WriteToFile_ShouldThrowArgumentNullException_WhenDataIsNull() // Zapisuje: wyrzuca wyjątek, jeśli dane to null
        {
            // Arrange
            string testFilePath = "nullData.json";
            var jsonCommon = new JsonCommonClass();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => jsonCommon.WriteToFile<object>(testFilePath, null));
        }
        
        // 7
        [Fact] // Zaliczony
        public void WriteToFile_ShouldCreateFile_WithSingleObjectJson() // Zapisuje: tworzy plik z pojedynczym obiektem
        {
            // Arrange
            string testFilePath = "singleObject.json";
            var question = new Question(0, "Z ilu części składa się powieść Alfreda Szklarskiego?");
            var jsonCommon = new JsonCommonClass();

            // Act
            jsonCommon.WriteToFile(testFilePath, question);

            // Assert
            Assert.True(File.Exists(testFilePath));

            string jsonContent = File.ReadAllText(testFilePath);
            Assert.Contains("\"Z ilu części składa się powieść Alfreda Szklarskiego?\"", jsonContent);

            // Cleanup
            File.Delete(testFilePath);
        }

        // 8
        [Fact] // Zaliczony
        public void WriteToFile_ShouldThrowException_WhenFilePathIsInvalid() // Zapisuje: wyrzuca wyjątek, jeśli ścieżka pliku wadliwa
        {
            // Arrange
            string invalidPath = "?:\\invalid\\path.json"; // Nieprawidłowa ścieżka
            var testData = new List<Question> { new Question(1, "Z ilu części składa się powieść Alfreda Nowaka?") };
            var jsonCommon = new JsonCommonClass();

            // Act & Assert
            Assert.Throws<DirectoryNotFoundException>(() => jsonCommon.WriteToFile(invalidPath, testData));
        }

        // 9
        [Fact] // Zaliczony
        public void ReadFromFile_ShouldReturnCorrectData() // Odczytuje: zwraca poprawne dane
        {
            // Arrange
            var filePath = "testRead.json";
            var expected = new List<Question> { new Question(1, "Jakie zwierzę jest królem dżungli?") };
            File.WriteAllText(filePath, JsonConvert.SerializeObject(expected));

            var jsonCommon = new JsonCommonClass();

            // Act
            var result = jsonCommon.ReadFromFile<List<Question>>(filePath);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result[0].QuestionId);
            Assert.Equal("Jakie zwierzę jest królem dżungli?", result[0].QuestionContent);

            // Cleanup
            File.Delete(filePath);
        }

        // 10
        [Fact] // Zaliczony
        public void ReadFromFile_ShouldThrowFileNotFoundException_WhenFileDoesNotExist() // Odczytuje: wyrzuca wyjątek, jeśli plik nie istnieje
        {
            // Arrange
            string nonExistentFile = "doesNotExist.json";
            var jsonCommon = new JsonCommonClass();

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => jsonCommon.ReadFromFile<List<Question>>(nonExistentFile));
        }

        // 11
        [Fact] // Zaliczony
        public void ReadFromFile_ShouldThrowInvalidDataException_WhenFileIsEmpty() // Odczytuje: wyrzuca wyjątek, jeśli plik jest pusty
        {
            // Arrange
            string emptyFilePath = "emptyFile.json";
            File.WriteAllText(emptyFilePath, ""); // Tworzę pusty plik
            var jsonCommon = new JsonCommonClass();

            // Act & Assert
            Assert.Throws<JsonReaderException>(() => jsonCommon.ReadFromFile<List<Question>>(emptyFilePath));

            // Cleanup
            File.Delete(emptyFilePath);
        }

        // 12
        [Fact] // Zaliczony
        public void ReadFromFile_ShouldThrowJsonReaderException_WhenFileContainsInvalidJson() //Odczytuje: wyrzuca wyjatek, jeśli json ma błędny format
        {
            // Arrange
            string invalidJsonFilePath = "invalidJson.json";
            File.WriteAllText(invalidJsonFilePath, "{ niepoprawny JSON }"); // Niepoprawna struktura JSON

            var jsonCommon = new JsonCommonClass();

            // Act & Assert
            Assert.Throws<JsonReaderException>(() => jsonCommon.ReadFromFile<List<Question>>(invalidJsonFilePath));

            // Cleanup
            File.Delete(invalidJsonFilePath);
        }
    }
}
