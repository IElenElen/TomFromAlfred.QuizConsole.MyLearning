using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;

namespace TomFromAlfred.Quiz.Tests
{
    public class FakeUserInputReader : IUserInputReader
    {
        private readonly ConsoleKeyInfo _keyInfo;

        public FakeUserInputReader(ConsoleKeyInfo keyInfo)
        {
            _keyInfo = keyInfo;
        }
        public ConsoleKeyInfo ReadKey()
        {
            return _keyInfo;
        }
    }
}
