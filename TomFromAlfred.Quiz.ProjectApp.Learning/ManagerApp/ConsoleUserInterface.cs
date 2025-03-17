using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract.AbstractForManager;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class ConsoleUserInterface : IUserInterface
    {
        public virtual string ReadLine() => Console.ReadLine();
        public virtual void WriteLine(string message) => Console.WriteLine(message);
    }
}
