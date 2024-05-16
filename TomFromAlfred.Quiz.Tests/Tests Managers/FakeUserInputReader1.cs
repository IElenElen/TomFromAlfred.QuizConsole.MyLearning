using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;

namespace TomFromAlfred.Quiz.Tests
{
    public class FakeUserInputReader1 : IUserInputReader //na potrzeby testu
    {
        private readonly char _keyChar;

        public FakeUserInputReader1(char keyChar)
        {
            _keyChar = keyChar;
        }
        public ConsoleKeyInfo ReadKey()
        {
            return new ConsoleKeyInfo(_keyChar, ConsoleKey.A, false, false, false);
        }
    }
}
