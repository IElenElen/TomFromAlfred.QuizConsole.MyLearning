using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class ConsoleInputReaderManagerApp : IUserInputReader
    {
        public ConsoleKeyInfo ReadKey()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
            Console.Write(keyInfo.KeyChar); // wyświetlenie znaku na konsoli po naciśnięciu klawisza
            return keyInfo;
        }
    }
}
