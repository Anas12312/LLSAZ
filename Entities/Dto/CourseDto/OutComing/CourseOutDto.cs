using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto.CourseDto.OutComing
{
    public class CourseOutDto
    {
        public int CourseIDD { get; set; }
        public string CourseName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NOStudents { get; set; }
        public int NOExpirments { get; set; }
    }
}
