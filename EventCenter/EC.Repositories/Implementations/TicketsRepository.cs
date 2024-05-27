using EC.Data.Entities;
using EC.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Repositories.Implementations
{
    public class TicketsRepository:Repository<Ticket>,ITicketsRepository
    {
        public TicketsRepository(DbContext context) : base(context) { }

        public override async Task<IEnumerable<Ticket>> GetAllAsync(bool isActive = true)
        {
            return await SoftDeleteQueryFilter(this.DbSet, isActive).ToListAsync();
        }
    }
}
