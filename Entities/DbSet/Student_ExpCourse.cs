using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbSet
{
    public class Student_ExpCourse
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        //Relation
        public Guid StudentId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public User User { get; set; }


        public Guid Exp_CourseId { get; set; }

        [ForeignKey(nameof(Exp_CourseId))]
        public Exp_Course Exp_Course { get; set; }

        //Gradding
        public string Answers { get; set; }
        public int Grading { get; set; }
        public bool IsCompleted { get; set; }
    }
}
