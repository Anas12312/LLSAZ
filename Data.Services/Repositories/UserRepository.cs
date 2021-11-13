using Data.Services.Data;
using Data.Services.Intefaces;
using Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Repositories
{
    public class UserRepository : /*Repository<User> ,*/ IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddUser(User user)
        {
            var result = await _context.Users.AddAsync(user);
            if(result != null)
                return true;
            
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id && x.Status != 0);

            if (user != null)
            {
                user.Status = 0;

                return true;
            }
            return false;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = await _context.Users.Where(x => x.Status == 1).ToListAsync();

            return users;
        }

        public async Task<User> GetUserById(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id && x.Status == 1);

            return user;
        }

        public async Task<User> GetUserByIdentityId(Guid identityId)
        {
            var user = await _context.Users.Where(x => x.Status == 1 && x.IdentityId == identityId)
                                            .FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> Update(Guid id, User newUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                user.FirstName = newUser.FirstName;
                user.Lastname = newUser.Lastname;
                user.Email = newUser.Email;
                user.Country = newUser.Country;
                user.PhoneNumber = newUser.PhoneNumber;
                user.Role = newUser.Role;

                user.UpdateDate = DateTime.UtcNow;

                return true;
            }
            return false;
        }

        public async Task<bool> UpdateIdentityId(Guid id, User newUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.IdentityId == id);
            if (user != null)
            {
                user.FirstName = newUser.FirstName;
                user.Lastname = newUser.Lastname;
                user.Email = newUser.Email;
                user.Country = newUser.Country;
                user.PhoneNumber = newUser.PhoneNumber;
                user.Role = newUser.Role;

                user.UpdateDate = DateTime.UtcNow;

                return true;
            }
            return false;
        }

        public async Task<bool> Activate(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id && x.Status != 1);

            if (user != null)
            {
                user.Status = 1;

                return true;
            }
            return false;
        }

        public async Task<bool> DeleteRole(string roleName)
        {
            var users = await _context.Users.Where(x => x.Role == roleName)
                                            .ToListAsync();
            if(users == null)
            {
                return false;
            }

            foreach(var user in users)
            {
                user.Role = "User";
            }

            return true;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email && x.Status == 1);

            return user;
        }

        public async Task<IEnumerable<User>> GetAllStudnet()
        {
            var list = await _context.Users.Where(x => x.Role.ToLower() == "student" && x.Status == 1).ToListAsync();
            return list;
        }

        public async Task<IEnumerable<User>> GetAllTeacher()
        {
            var list = await _context.Users.Where(x => x.Role.ToLower() == "teacher" && x.Status == 1).ToListAsync();
            return list;
        }
    }
}
