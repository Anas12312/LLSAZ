using Data.Services.Interfaces;
using Entities.DbSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Intefaces
{
    public interface IUserRepository
    {
        Task<bool> AddUser(User user);

        Task<User> GetUserById(Guid id);

        Task<User> GetUserByEmail(string email);

        Task<User> GetUserByIdentityId(Guid identityId);

        Task<IEnumerable<User>> GetAllUsers();

        Task<bool> Update(Guid id, User user);

        Task<bool> UpdateIdentityId(Guid id, User newUser);

        Task<bool> Delete(Guid id);

        Task<bool> DeleteRole(string roleName);

        Task<bool> Activate(Guid id);

        Task<IEnumerable<User>> GetAllStudnet();
        Task<IEnumerable<User>> GetAllTeacher();
    }
}
