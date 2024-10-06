using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using LiveElectric2.Server.Models;

namespace LiveElectric2.Server.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        public DbSet<UserProfile> Profiles { get; set; } = null!;
        public DbSet<UserProfile> UserProfiles { get; set; } = null!;

        public DbSet<Product> Products { get; set; } = null!;

        public DbSet<Ingredient> Ingredients { get; set; } = null!;


        public ApplicationContext(DbContextOptions<ApplicationContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserProfile)
                .WithOne(up => up.User)
                .HasForeignKey<UserProfile>(up => up.UserId);

            base.OnModelCreating(modelBuilder);
        }

    }

}
