using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Repositories.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        DbContext Context { get; }

        IEventsRepository Events { get; }

        ITicketsRepository Tickets { get; }
        IUsersRepository Users { get; }

        Task<int> SaveChangesAsync();
    }
}
