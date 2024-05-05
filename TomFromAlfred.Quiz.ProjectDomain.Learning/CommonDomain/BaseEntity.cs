using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.CommonDomain
{
    public class BaseEntity
    {
        public char Letter { get; set; }
        public int Id { get; set; }
        public int Number { get; set; }
        public string? Content { get; set; }
    }
}
