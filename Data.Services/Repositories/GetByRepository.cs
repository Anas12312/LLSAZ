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
    public class GetByRepository : IGetBy
    {
        private readonly AppDbContext _context;

        public GetByRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetAuthorByExp(Guid id)
        {
            var exp = await _context.Expirments.FirstOrDefaultAsync(x => new Guid(x.AuthorId) == id && x.Status == 1);
            var result = await _context.Users.FirstOrDefaultAsync(x => x.Id == new Guid(exp.AuthorId) && x.Status == 1);
            return result;
        }

        public async Task<IEnumerable<Expirment>> GetExpsByAuthor(Guid id)
        {
            var expList = await _context.Expirments.Where(x => new Guid(x.AuthorId) == id && x.Status == 1).ToListAsync();

            return expList;
        }

        public async Task<IEnumerable<Guid>> GetCoursesByExp(Guid id)
        {
            var courses = await _context.Expirments.Where(n => n.Id == id).Select(x => x.Exp_Courses.Select(x => x.CourseId).ToList()).FirstOrDefaultAsync();

            return courses;
        }

        public async Task<IEnumerable<Guid>> GetExpByCourse(Guid id)
        {
            var exps = await _context.Courses.Where(n => n.Id == id).Select(x => x.Exp_Courses.Select(x => x.ExperimentId).ToList()).FirstOrDefaultAsync();
            return exps;
        }

        public async Task<IEnumerable<Guid>> GetTeacherByCourse(Guid id)
        {
            var teacher = await _context.Courses.Where(n => n.Id == id).Select(x => x.Teacher_Courses.Select(x => x.TeacherId).ToList()).FirstOrDefaultAsync();
            return teacher;
        }

        public async Task<IEnumerable<Expirment>> GetFullExpByCourse(Guid id)
        {
            var exps = await _context.Courses.Where(n => n.Id == id).Select(x => x.Exp_Courses.Select(x => x.Expirment).ToList()).FirstOrDefaultAsync();
            return exps;
        }

        public async Task<IEnumerable<User>> GetFullStudentByCourse(Guid id)
        {
            var students = await _context.Courses.Where(n => n.Id == id).Select(x => x.Student_Courses.Select(x => x.User).ToList()).FirstOrDefaultAsync();
            return students;
        }

        public async Task<IEnumerable<User>> GetFullTeacherByCourse(Guid id)
        {
            var teachers = await _context.Courses.Where(n => n.Id == id).Select(x => x.Teacher_Courses.Select(x => x.User).ToList()).FirstOrDefaultAsync();
            return teachers;
        }

        public async Task<IEnumerable<Guid>> GetCoursesByStudent(Guid id)
        {
            var courses = await _context.Users.Where(n => n.Id == id && n.Role.ToLower() == "student").Select(x => x.Student_Courses.Select(x => x.CourseId).ToList()).FirstOrDefaultAsync();
            return courses;
        }

        public async Task<IEnumerable<Guid>> GetStudentsByCourse(Guid id)
        {
            var students = await _context.Courses.Where(n => n.Id == id).Select(x => x.Student_Courses.Select(x => x.StudentId).ToList()).FirstOrDefaultAsync();
            return students;
        }




        public Task<IEnumerable<Guid>> GetExpsByStudent(Guid id)
        {
            throw new NotImplementedException();
        }

        
        public Task<IEnumerable<Guid>> GetStudentsByExp(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
