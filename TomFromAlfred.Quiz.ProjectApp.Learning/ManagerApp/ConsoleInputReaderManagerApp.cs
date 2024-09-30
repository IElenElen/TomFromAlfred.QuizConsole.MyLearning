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
        public ConsoleKeyInfo ReadKey() //obiekt, metoda ReadKey go zwraca
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true); //metoda statyczna, keyInfo = zmienna, ConsoleKeyInfo = struktura
            Console.Write(keyInfo.KeyChar); // wyświetlenie znaku na konsoli po naciśnięciu klawisza, tzn. wartości
            return keyInfo;
        }
    }
}
