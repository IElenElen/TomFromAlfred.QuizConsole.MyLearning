using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.CommonDomain
{
    public class AdministratorsStepsOverview
    {
        public int CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}
