using EC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Data.Contexts
{
    public  class EventCenterDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public EventCenterDbContext(DbContextOptions<EventCenterDbContext> options):base(options) { }
        public EventCenterDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EventCenterDb;Integrated Security=True;");
            //optionsBuilder.UseLazyLoadingProxies();
        }
    }

    
}
