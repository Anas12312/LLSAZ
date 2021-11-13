using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbSet
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }
        public int CourseIDD { get; set; }
        public string CreatedBy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int NumberOfStudents { get; set; }
        public int NumberOfExp { get; set; }


        //Assigend Student To Course
        public List<Student_Course> Student_Courses { get; set; }

        //Assigned Teacher to Course
        public List<Teacher_Course> Teacher_Courses { get; set; }

        //Assigned Exp to Course
        public List<Exp_Course> Exp_Courses { get; set; }

    }
}
