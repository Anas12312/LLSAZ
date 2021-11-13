using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto.CourseDto
{
    public class CourseDto
    {
        public string Name { get; set; }
        public int IDD { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfStudents { get; set; }
    }
}
