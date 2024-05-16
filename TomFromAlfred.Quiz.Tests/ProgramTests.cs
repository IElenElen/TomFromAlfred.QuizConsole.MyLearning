using System.Security.Cryptography;

namespace TomFromAlfred.Quiz.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void TestQuizExit()
        {
            using (StringWriter sw = new StringWriter()) // symulacja wejœcia "k" przez konsolê

            {
                using (StringReader sr = new StringReader("k\n"))
                {
                    Console.SetIn(sr);
                    Console.SetOut(sw);

                    //TomFromAlfred.QuizConsole.MyLearning.Program.Main(new string[0]); // wywo³anie metody do przetestowania


                    Assert.Contains("Quiz zosta³ zatrzymany.", sw.ToString()); // czy konsola wyœwietli³a odpowiedni komunikat?

                }
            }
        }
    }
}