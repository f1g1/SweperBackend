using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SweperBackend.Data
{
    public class SweperBackendContext : DbContext
    {
        public SweperBackendContext(DbContextOptions<SweperBackendContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(p => p.InitialForm)
                .WithOne(b => b.User);

            modelBuilder.Entity<User>()
             .HasMany(p => p.PrefferedLocations)
             .WithOne(b => b.User);

            modelBuilder.Entity<InitialForm>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<UserPreferredLocation>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<UserPreferredLocation>()
                .HasIndex(x => x.UserId);
        }
        public DbSet<User> User { get; set; }
        public DbSet<InitialForm> InitialForm { get; set; }
    }
}
