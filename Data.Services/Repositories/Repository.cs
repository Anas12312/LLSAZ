using Data.Services.Data;
using Data.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        private readonly DbSet<T> _set;
        public Repository(AppDbContext context,ILogger logger)
        {
            _context = context;
            _logger = logger;
            _set = _context.Set<T>();
        }
        public virtual async Task<bool> Add(T entity)
        {
            var result =  await _set.AddAsync(entity);
            if(result != null)
                return true;

            return false;
        }

        public virtual Task<bool> Delete(Guid Id)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            var users = await  _set.ToListAsync();
            return users;
        }

        public virtual async Task<T> GetById(Guid Id)
        {
            var user = await _set.FindAsync(Id);
            return user;
        }

        public virtual Task<bool> Upsert(Guid Id, T entity)
        {
            throw new NotImplementedException();
        }
    }
}
