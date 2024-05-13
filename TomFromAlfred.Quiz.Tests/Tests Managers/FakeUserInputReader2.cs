using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;

namespace TomFromAlfred.Quiz.Tests
{
    public class FakeUserInputReader2 : IUserInputReader
    {
        private Queue<ConsoleKeyInfo> _inputQueue;

        public FakeUserInputReader2(IEnumerable<ConsoleKeyInfo> input)
        {
            _inputQueue = new Queue<ConsoleKeyInfo>(input);
        }
        public ConsoleKeyInfo ReadKey()
        {
            if (_inputQueue.Count > 0)
                return _inputQueue.Dequeue();
            else
                throw new InvalidOperationException("No more input provided.");
        }
    }
}
