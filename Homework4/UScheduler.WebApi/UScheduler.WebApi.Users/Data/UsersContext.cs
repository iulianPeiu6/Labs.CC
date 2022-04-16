using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using UScheduler.WebApi.Users.Data.Entities;

namespace UScheduler.WebApi.Users.Data
{
    public sealed class UsersContext : DbContext
    {
        public DbSet<User>? Users { get; set; }

        public DbSet<AccountSettings>? AccountSettings { get; set; }

        public UsersContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(builder =>
            {
                builder.Property(user => user.UserName)
                    .HasMaxLength(32)
                    .IsRequired();

                builder.HasIndex(user => user.UserName)
                    .IsUnique();

                builder.Property(user => user.Email)
                    .IsRequired();

                builder.HasIndex(user => user.Email)
                    .IsUnique();

                builder.Property(user => user.HashedPassword)
                    .IsRequired();

                builder.Property(e => e.RegistrationDate)
                    .Metadata
                    .SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                builder.Property(e => e.AccountSettingsId)
                    .Metadata
                    .SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                builder.HasOne(user => user.AccountSettings)
                    .WithOne()
                    .HasForeignKey<User>(user => user.AccountSettingsId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
