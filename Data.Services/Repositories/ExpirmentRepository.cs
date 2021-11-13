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
    public class ExpirmentRepository : IExpirmentRepository
    {
        private readonly AppDbContext _context;

        public ExpirmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Activate(Guid id)
        {
            var user = await _context.Expirments.FirstOrDefaultAsync(x => x.Id == id && x.Status != 1);

            if (user != null)
            {
                user.Status = 1;

                return true;
            }
            return false;
        }

        public async Task<bool> AddExpirment(Expirment expirment)
        {
            var result = await _context.Expirments.AddAsync(expirment);
            if (result != null)
                return true;

            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var user = await _context.Expirments.FirstOrDefaultAsync(x => x.Id == id && x.Status != 0);

            if (user != null)
            {
                user.Status = 0;

                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Expirment>> GetAllExpirments()
        {
            var expirments = await _context.Expirments.Where(x => x.Status == 1).ToListAsync();

            return expirments;
        }

        public async Task<Expirment> GetExpById(Guid id)
        {
            var expirment = await _context.Expirments.FirstOrDefaultAsync(x => x.Id == id && x.Status == 1);

            return expirment;
        }

        public async Task<Expirment> GetExpByName(string expName)
        {
            var expirment = await _context.Expirments.FirstOrDefaultAsync(x => x.Name.ToLower() == expName.ToLower() && x.Status == 1);

            return expirment;
        }

        public async Task<bool> Update(Guid id, Expirment newExpirment)
        {
            var expirment = await _context.Expirments.FirstOrDefaultAsync(x => x.Id == id);
            if(expirment != null)
            {
                expirment.Name = newExpirment.Name;
                expirment.LLOPath = newExpirment.LLOPath;
                expirment.UpdateDate = DateTime.UtcNow;
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Expirment>> GetAllExpFromAuthor(string mail)
        {
            var expList = await _context.Expirments.Where(x => x.AuthorName == mail && x.Status == 1).ToListAsync();

            return expList;
        }

    }
}
