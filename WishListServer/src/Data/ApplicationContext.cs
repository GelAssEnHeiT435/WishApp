using Microsoft.EntityFrameworkCore;
using WishListServer.src.Data.Models.Database;

namespace WishListServer.src.Data
{
    public class ApplicationContext: DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Credential> Credential => Set<Credential>();
        public DbSet<Wishlist> Wishlists => Set<Wishlist>();
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

            modelBuilder.Entity<User>()
                .HasOne(p => p.Credential)
                .WithOne(i => i.User)
                .HasForeignKey<Credential>(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
               .HasOne(u => u.Wishlist)
               .WithOne(w => w.User)
               .HasForeignKey<Wishlist>(w => w.UserId)
               .OnDelete(DeleteBehavior.Cascade);  

            modelBuilder.Entity<Wishlist>()
                .HasMany(w => w.Wishes)
                .WithOne(w => w.Wishlist)         
                .HasForeignKey(w => w.WishlistId)  
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>().HasIndex(u => u.Username);
            modelBuilder.Entity<Wishlist>().HasIndex(w => w.ShareToken).IsUnique();
            modelBuilder.Entity<Wishlist>().HasIndex(w => w.UserId).IsUnique();
            modelBuilder.Entity<Wish>().HasIndex(w => w.WishlistId);
            modelBuilder.Entity<Wish>().HasIndex(w => new { w.WishlistId, w.IsReceived });
            modelBuilder.Entity<Image>().HasIndex(i => i.WishId).IsUnique();
            modelBuilder.Entity<Credential>().HasIndex(c => new { c.UserId, c.Login }).IsUnique();
        }
    }
}
