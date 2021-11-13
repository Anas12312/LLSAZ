using Entities.DbSet;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options)
        {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Student_Course
            builder.Entity<Student_Course>()
                .HasOne(b => b.User)
                .WithMany(ba => ba.Student_Courses)
                .HasForeignKey(bi => bi.StudentId);

            builder.Entity<Student_Course>()
                .HasOne(b => b.Course)
                .WithMany(ba => ba.Student_Courses)
                .HasForeignKey(bi => bi.CourseId);


            //Teacher_Course
            builder.Entity<Teacher_Course>()
                .HasOne(b => b.User)
                .WithMany(ba => ba.Teacher_Courses)
                .HasForeignKey(bi => bi.TeacherId);

            builder.Entity<Teacher_Course>()
                .HasOne(b => b.Course)
                .WithMany(ba => ba.Teacher_Courses)
                .HasForeignKey(bi => bi.CourseId);

            //Exp_Course
            builder.Entity<Exp_Course>()
                .HasOne(b => b.Expirment)
                .WithMany(ba => ba.Exp_Courses)
                .HasForeignKey(bi => bi.ExperimentId);

            builder.Entity<Exp_Course>()
                .HasOne(b => b.Course)
                .WithMany(ba => ba.Exp_Courses)
                .HasForeignKey(bi => bi.CourseId);

            //Student_ExpCourse
            builder.Entity<Student_ExpCourse>()
                .HasOne(b => b.User)
                .WithMany(ba => ba.Student_ExpCourses)
                .HasForeignKey(bi => bi.StudentId);

            builder.Entity<Student_ExpCourse>()
                .HasOne(b => b.Exp_Course)
                .WithMany(ba => ba.Student_ExpCourses)
                .HasForeignKey(bi => bi.Exp_CourseId);


            base.OnModelCreating(builder);


        }

        public DbSet<User> Users { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Expirment> Expirments { get; set; }

        public DbSet<Course> Courses { get; set; }

        //Joint Db
        public DbSet<Exp_Course> Exp_Courses { get; set; }
        public DbSet<Student_Course> Student_Courses { get; set; }
        public DbSet<Student_ExpCourse> Student_ExpCourses { get; set; }
        public DbSet<Teacher_Course> Teacher_Courses { get; set; }


    }
}
