using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbSet
{
    public class Expirment : BaseEntity
    {
        public string Name { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string LLOPath { get; set; }

        //Assigned Courses for this Expirment
        public List<Exp_Course> Exp_Courses { get; set; }

    }
}
