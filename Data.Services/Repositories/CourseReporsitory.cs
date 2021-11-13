using Data.Services.Data;
using Data.Services.Intefaces;
using Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Repositories
{
    public class CourseReporsitory : ICourseRepository
    {
        private readonly AppDbContext _context;
        public CourseReporsitory(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CtreateCourse(Course course)
        {
            var result = await _context.Courses.AddAsync(course);

            if (result == null) return false;

            return true;
        }
        public async Task<bool> Delete(Guid id)
        {
            var user = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id && x.Status != 0);

            if (user != null)
            {
                user.Status = 0;

                return true;
            }
            return false;
        }
        public async Task<bool> Activate(Guid id)
        {
            var user = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id && x.Status != 1);

            if (user != null)
            {
                user.Status = 1;

                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            var courses = await _context.Courses.Where(x => x.Status == 1).ToListAsync();

            return courses;
        }

        public async Task<Course> GetCourseById(Guid id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id && x.Status == 1);

            return course;
        }
        public async Task<Course> GetCourseByName(string courseName)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Name.ToLower() == courseName.ToLower() && x.Status == 1);

            return course;
        }
        public async Task<Guid> GetIdByName(string courseName)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Name.ToLower() == courseName.ToLower() && x.Status == 1);

            return course.Id;
        }

        //Assiging in Course
        public async Task<bool> AssignTeacherToCourse(Guid teacherId, Guid courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null)
                return false;

            var teacher_course = new Teacher_Course()
            {
                TeacherId = teacherId,
                CourseId = courseId
            };

            var result = await _context.Teacher_Courses.AddAsync(teacher_course);
            return true;
        }
        public async Task<bool> AssignStudentToCourse(Guid studentId, Guid courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null)
                return false;

            var student_course = new Student_Course()
            {
                StudentId = studentId,
                CourseId = courseId
            };

            var result = await _context.Student_Courses.AddAsync(student_course);
            course.NumberOfStudents++;
            return true;
        }
        public async Task<bool> AssignExpToCourse(Guid expId, Guid courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null)
                return false;

            var exp_Course = new Exp_Course()
            {
                ExperimentId = expId,
                CourseId = courseId
            };

            var result = await _context.Exp_Courses.AddAsync(exp_Course);
            course.NumberOfExp++;

            return true;
        }
        public async Task<bool> AssignStudentToExpCourse(Guid student, Guid courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null)
                return false;

            var exp_courses = await _context.Courses.Where(n => n.Id == courseId).Select(x => x.Exp_Courses.Select(x => x.Id).ToList()).FirstOrDefaultAsync();

            foreach (var x in exp_courses)
            {
                var student_expCourse = new Student_ExpCourse()
                {
                    StudentId = student,
                    Exp_CourseId = x
                };

                await _context.Student_ExpCourses.AddAsync(student_expCourse);
            }

            return true;
        }

        //Reomoving in Course
        public async Task<bool> RemoveTeacherFromCourse(Guid teacherId, Guid courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null)
                return false;

            var result = await _context.Teacher_Courses.FirstOrDefaultAsync(x => x.TeacherId == teacherId);
            _context.Teacher_Courses.Remove(result);
            return true;
        }
        public async Task<bool> RemoveStudentFromCourse(Guid studentId, Guid courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null)
                return false;


            var result = await _context.Student_Courses.FirstOrDefaultAsync(x => x.StudentId == studentId);
            _context.Student_Courses.Remove(result);
            return true;
        }
        public async Task<bool> RemoveExpFromCourse(Guid expId, Guid courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null)
                return false;

            var result = await _context.Exp_Courses.FirstOrDefaultAsync(x => x.ExperimentId == expId);
            _context.Exp_Courses.Remove(result);
            return true;
        }

        
    }
}
