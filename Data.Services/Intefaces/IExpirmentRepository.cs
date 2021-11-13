using Entities.DbSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Intefaces
{
    public interface IExpirmentRepository
    {
        Task<bool> AddExpirment(Expirment expirment);

        Task<IEnumerable<Expirment>> GetAllExpirments();

        Task<bool> Update(Guid id, Expirment newExpirment);

        Task<bool> Delete(Guid id);

        Task<bool> Activate(Guid id);

        Task<Expirment> GetExpById(Guid id);

        Task<Expirment> GetExpByName(string expName);

        Task<IEnumerable<Expirment>> GetAllExpFromAuthor(string id);
    }
}
