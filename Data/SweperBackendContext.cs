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

            modelBuilder.Entity<RentItem>()
              .Property(x => x.Id)
              .ValueGeneratedOnAdd();

            modelBuilder.Entity<RentItem>()
           .HasMany(p => p.RentItemImages)
           .WithOne(b => b.RentItem)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RentItemImage>()
           .Property(x => x.Id)
           .ValueGeneratedOnAdd();

            modelBuilder.Entity<UserRentItem>()
              .Property(x => x.Id)
              .ValueGeneratedOnAdd();
        }
        public DbSet<User> User { get; set; }
        public DbSet<InitialForm> InitialForm { get; set; }
        public DbSet<RentItem> RentItem { get; set; }
        public DbSet<RentItemImage> RentItemImage { get; set; }
        public DbSet<UserRentItem> UserRentItem { get; set; }
    }
}
