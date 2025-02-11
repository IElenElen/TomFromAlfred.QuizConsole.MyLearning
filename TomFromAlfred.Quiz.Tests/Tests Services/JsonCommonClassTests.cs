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
    // Oblane: 2 / 2

    public class JsonCommonClassTests
    {
        private readonly Mock<JsonCommonClass> _mockJsonCommonClass;

        public JsonCommonClassTests()
        {
            _mockJsonCommonClass = new Mock<JsonCommonClass>();
        }

        // 1
        [Fact] // Oblany
        public void CreateDefaultFile_ShouldCreateFile_WhenFileDoesNotExist() // Tworzy plik, jeśli ten nie istnieje
        {
            // Arrange
            string newFilePath = "defaultFile.json";
            var defaultNewData = new List<Question> { new Question(1, "pytanie przykładowe") };

            // Mockuję metodę odczytu pliku, która wyrzuca wyjątek FileNotFoundException, jeśli plik nie istnieje
            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(newFilePath)).Throws(new FileNotFoundException());

            // Mockuję metodę zapisu do pliku
            _mockJsonCommonClass.Setup(x => x.WriteToFile(newFilePath, defaultNewData));

            // Act
            _mockJsonCommonClass.Object.CreateDefaultFile(newFilePath, defaultNewData); // Wywołanie metody na mocku

            // Assert
            _mockJsonCommonClass.Verify(x => x.WriteToFile(newFilePath, defaultNewData), Times.Once); // Sprawdzam, czy WriteToFile zostało wywołane
        }

        // 2
        [Fact] // Oblany
        public void CreateDefaultFile_ShouldNotCreateFile_WhenFileAlreadyExists() // Nie tworzy, jeśli plik już istnieje
        {
            // Arrange
            string existingFilePath = "existingFile.json";
            var defaultNewData = new List<Question> {new Question (1, "pytanie przykładowe")};

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Question>>(existingFilePath)).Returns(defaultNewData);

            // Act
            _mockJsonCommonClass.Object.CreateDefaultFile(existingFilePath, defaultNewData); // Wywołanie metody na mocku

            // Assert
            _mockJsonCommonClass.Verify(x => x.WriteToFile(existingFilePath, defaultNewData), Times.Never);
        }
    }
}
