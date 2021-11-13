using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<bool> Add(T entity);
        Task<T> GetById(Guid Id);
        Task<IEnumerable<T>> GetAll();
        Task<bool> Delete(Guid Id);
        Task<bool> Upsert(Guid Id, T entity);
    }
}
