using EC.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Repositories.Implementations
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly DbContext context;

        public IEventsRepository Events { get; }

        public ITicketsRepository Tickets { get; }
        public IUsersRepository Users { get; }
        public DbContext Context { get { return context; } }

        public UnitOfWork(DbContext context)
        {
            this.context = context;
            Events = new EventsRepository(context);
            Tickets = new TicketsRepository(context);
            Users = new UsersRepository(context);
        }

        public void Dispose() => this.Dispose(true);

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.context?.Dispose();
            }
        }

        public async Task<int> SaveChangesAsync() => await this.context.SaveChangesAsync();
    }
}
