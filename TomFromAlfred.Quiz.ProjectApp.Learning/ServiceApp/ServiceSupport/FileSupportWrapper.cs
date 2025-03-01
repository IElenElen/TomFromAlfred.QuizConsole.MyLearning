using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.ServiceSupport
{
    public class FileSupportWrapper : IFileWrapper // Na potrzeby testu
    {
        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
