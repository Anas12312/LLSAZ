using Entities.DbSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Intefaces
{
    public interface IGetBy
    {
        //Author(User) -- Expirment
        Task<IEnumerable<Expirment>> GetExpsByAuthor(Guid id);
        Task<User> GetAuthorByExp(Guid id);
        //Student(User) -- Course
        Task<IEnumerable<Guid>> GetStudentsByCourse(Guid id);
        Task<IEnumerable<Guid>> GetCoursesByStudent(Guid id);
        //Course -- Expirment
        Task<IEnumerable<Guid>> GetCoursesByExp(Guid id);
        Task<IEnumerable<Guid>> GetExpByCourse(Guid id);
        //Student(User) -- Expirment
        Task<IEnumerable<Guid>> GetStudentsByExp(Guid id);
        Task<IEnumerable<Guid>> GetExpsByStudent(Guid id);

        Task<IEnumerable<Guid>> GetTeacherByCourse(Guid id);
        Task<IEnumerable<Expirment>> GetFullExpByCourse(Guid id);
        Task<IEnumerable<User>> GetFullStudentByCourse(Guid id);
        Task<IEnumerable<User>> GetFullTeacherByCourse(Guid id);
    }
}
