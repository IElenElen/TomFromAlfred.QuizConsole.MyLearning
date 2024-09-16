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
            return Console.ReadKey(intercept: true);
        }
    }
}
