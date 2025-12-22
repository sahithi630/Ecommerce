using Microsoft.EntityFrameworkCore;

namespace UserServices.Models
{
    public partial class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C9C6F81B0");

                entity.HasIndex(e => e.UserEmail, "UQ__Users__08638DF812446416").IsUnique();

                //entity.Property(e => e.Role).HasMaxLength(50);
                entity.Property(e => e.UserContactNumber).HasMaxLength(15);
                entity.Property(e => e.UserEmail).HasMaxLength(100);
                entity.Property(e => e.UserName).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
