using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbSet
{
    public class User : BaseEntity
    {
        public Guid IdentityId { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public string Country { get; set; }
        public string Role { get; set; }
        public string AcademicYear { get; set; }

        //Assigned Courses for Student
        public List<Student_Course> Student_Courses { get; set; }

        //Assigned Exp in sepcifec Course for Student
        public List<Student_ExpCourse> Student_ExpCourses { get; set; }

        //Assigned Courses for Teacher
        public List<Teacher_Course> Teacher_Courses { get; set; }
    }
}
