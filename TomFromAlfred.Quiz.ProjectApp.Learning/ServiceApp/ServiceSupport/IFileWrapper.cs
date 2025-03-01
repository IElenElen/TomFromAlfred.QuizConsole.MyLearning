using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.ServiceSupport
{

    // Interfejs dla testu
    public interface IFileWrapper 
    {
        bool Exists(string filePath);
    }
}

