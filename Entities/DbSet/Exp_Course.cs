using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbSet
{
    public class Exp_Course
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ExperimentId { get; set; }
        public Guid CourseId { get; set; }
        public string LLO { get; set; }


        [ForeignKey(nameof(ExperimentId))]
        public Expirment Expirment { get; set; }

        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }


        //Info for Specifiec Student in This Exp
        public List<Student_ExpCourse> Student_ExpCourses { get; set; }

    }
}
