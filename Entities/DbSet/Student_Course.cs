using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbSet
{
    public class Student_Course
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid StudentId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public User User { get; set; }

        
        public Guid CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }
    }
}
