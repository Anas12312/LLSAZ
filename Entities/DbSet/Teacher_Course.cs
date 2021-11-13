using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbSet
{
    public class Teacher_Course
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid TeacherId { get; set; }

        [ForeignKey(nameof(TeacherId))]
        public User User { get; set; }

        public Guid CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }
    }
}
