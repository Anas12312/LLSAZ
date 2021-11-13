using Data.Services.Intefaces;
using Data.Services.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Data
{
    public class UnitOfWork : IUnitOfWork ,IDisposable
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IOptionsMonitor<JwtConfig> _optionsMonitor;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public UnitOfWork(AppDbContext context,
                         UserManager<IdentityUser> userManager,
                         RoleManager<IdentityRole> roleManager,
                         IOptionsMonitor<JwtConfig> optionsMonitor,
                         TokenValidationParameters tokenValidationParameters)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _optionsMonitor = optionsMonitor;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public IUserRepository Users => new UserRepository(_context);

        public IAccountRepository AccountRepository => new AccountRepository(_userManager,
                                                                             _roleManager,
                                                                             _optionsMonitor,
                                                                             Users,
                                                                             _tokenValidationParameters,
                                                                             _context);

        public IExpirmentRepository ExpirmentRepository => new ExpirmentRepository(_context);

        public ICourseRepository CourseRepository => new CourseReporsitory(_context);

        public IGetBy GetBy => new GetByRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<bool> SaveAsync()
        {
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
