using System.Security.Cryptography;

namespace TomFromAlfred.Quiz.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void TestQuizExit()
        {
            using (StringWriter sw = new StringWriter()) // symulacja wej�cia "k" przez konsol�

            {
                using (StringReader sr = new StringReader("k\n"))
                {
                    Console.SetIn(sr);
                    Console.SetOut(sw);

                    //TomFromAlfred.QuizConsole.MyLearning.Program.Main(new string[0]); // wywo�anie metody do przetestowania


                    Assert.Contains("Quiz zosta� zatrzymany.", sw.ToString()); // czy konsola wy�wietli�a odpowiedni komunikat?

                }
            }
        }
    }
}