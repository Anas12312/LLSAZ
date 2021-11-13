using Entities.DbSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Intefaces
{
    public interface ICourseRepository
    {
        Task<bool> CtreateCourse(Course course);
        Task<bool> Delete(Guid id);
        Task<bool> Activate(Guid id);

        Task<IEnumerable<Course>> GetAllCourses();
        Task<Course> GetCourseById(Guid id);
        Task<Course> GetCourseByName(string courseName);
        Task<Guid> GetIdByName(string courseName);

        Task<bool> AssignTeacherToCourse(Guid teacherId, Guid courseId);
        Task<bool> AssignStudentToCourse(Guid studentId, Guid courseId);
        Task<bool> AssignExpToCourse(Guid expId, Guid courseId);
        Task<bool> AssignStudentToExpCourse(Guid student, Guid courseId);

        Task<bool> RemoveTeacherFromCourse(Guid teacherId, Guid courseId);
        Task<bool> RemoveStudentFromCourse(Guid studentId, Guid courseId);
        Task<bool> RemoveExpFromCourse(Guid expId, Guid courseId);
    }
}
