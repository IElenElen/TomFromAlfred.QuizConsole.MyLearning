﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.Abstract.AbstractForManager
{
    public interface IConsoleUserInterface
    {
        string ReadInputLine();
        void WriteOutputLine(string message);
    }
}
