using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server_try.Data
{
    public class server_tryContext : DbContext
    {
        public server_tryContext (DbContextOptions<server_tryContext> options)
            : base(options)
        {
        }

        public DbSet<server.Models.User>? User { get; set; }

        public DbSet<server.Models.Contact>? Contact { get; set; }

        public DbSet<server.Models.Message>? Message { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>().HasKey(u => new
            {
                u.id,
                u.UserId
            });
        }
    }
}
