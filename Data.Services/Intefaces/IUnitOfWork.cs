using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Intefaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }

        IAccountRepository AccountRepository { get; }

        IExpirmentRepository ExpirmentRepository { get; }

        ICourseRepository CourseRepository { get; }

        IGetBy GetBy { get; }

        //IRoleRepository RoleRepository { get; }

        Task<bool> SaveAsync();
    }
}
