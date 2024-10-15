using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    /*
     Pytanie to jednostka. Pojedyncze pytanie ma właściwości.
     Właściwości Question to Id - generowane przez system oraz treść. 
     Na jakiej podstawie łaczę pytanie z jego zestawem wyboru???
     */
    public class Question //klasa publiczna, bo pytanie to podstawa w budowie Quizu
    {
        public int Id { get; set; }
        public required string Content { get; set; } //co jeśli dopiero tworzę treść???

        public Question(int id, string content)
        {
            Id = id;
            Content = content;
        }
    }
}
