using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.Abstract
{
    public interface IUserInterface
    {
        string ReadLine();
        void WriteLine(string message);
    }
}
