using Moq;
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
    public class JsonCommonClassTests
    {
        private readonly Mock<JsonCommonClass> _mockJsonCommonClass;

        public JsonCommonClassTests()
        {
            _mockJsonCommonClass = new Mock<JsonCommonClass>();
        }

        [Fact]
        public void CreateDefaultFile_ShouldCreateFile_WhenFileDoesNotExist() // Oblany
        {
            // Arrange
            string filePath = "defaultFile.json";
            var defaultData = new List<Question> { new Question(1, "pytanie przykładowe") };

            // Mockuję metodę odczytu pliku, która wyrzuca wyjątek FileNotFoundException, jeśli plik nie istnieje
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(filePath)).Throws(new FileNotFoundException());

            // Mockuję metodę zapisu do pliku
            _mockJsonCommonClass.Setup(x => x.WriteToFile(filePath, defaultData));

            // Act
            _mockJsonCommonClass.Object.CreateDefaultFile(filePath, defaultData); // Wywołanie metody na mocku

            // Assert
            _mockJsonCommonClass.Verify(x => x.WriteToFile(filePath, defaultData), Times.Once); // Sprawdzam, czy WriteToFile zostało wywołane
        }

        // Testowanie, gdy plik już istnieje
        [Fact]
        public void CreateDefaultFile_ShouldNotCreateFile_WhenFileExists() // Oblany
        {
            // Arrange
            string filePath = "existingFile.json";
            var defaultData = new List<Question> {new Question (1, "pytanie przykładowe")};

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(filePath)).Returns(defaultData);

            // Act
            _mockJsonCommonClass.Object.CreateDefaultFile(filePath, defaultData); // Wywołanie metody na mocku

            // Assert
            _mockJsonCommonClass.Verify(x => x.WriteToFile(filePath, defaultData), Times.Never);
        }
    }
}
