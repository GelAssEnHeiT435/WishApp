using Microsoft.EntityFrameworkCore;
using WishListServer.src.Data.Models.Database;

namespace WishListServer.src.Data
{
    public class ApplicationContext: DbContext
    {
        public DbSet<Wish> Wishes => Set<Wish>();
        public DbSet<Image> Images => Set<Image>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Wish>()
                .HasOne(p => p.Image)
                .WithOne(i => i.Wish)
                .HasForeignKey<Image>(i => i.WishId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
