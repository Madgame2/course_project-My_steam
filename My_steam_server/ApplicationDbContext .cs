using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using My_steam_server.Models;

namespace My_steam_server
{
    public class ApplicationDbContext: IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }



        public DbSet<Game> Games { get; set; }
        public DbSet<UserLibraryEntry> UserLibraryEntries { get; set; }
        public DbSet<Screenshot> Screenshots { get; set; }
        public DbSet<ReportMessageModel> Reports { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<PurchaseOption> PurchaseOptions { get; set; }
        public DbSet<GoodReceived> GoodsReceived { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserLibraryEntry>()
                .HasKey(ule => new { ule.UserId, ule.GameId });

            builder.Entity<UserLibraryEntry>()
                .Property(ule => ule.UserId)
                .HasMaxLength(450); // ограничение длины строки для индекса

            builder.Entity<UserLibraryEntry>()
                .HasOne(ule => ule.User)
                .WithMany(u => u.Library)
                .HasForeignKey(ule => ule.UserId)
                .OnDelete(DeleteBehavior.Cascade); // предотвращает циклы каскадов

            builder.Entity<UserLibraryEntry>()
                .HasOne(ule => ule.Game)
                .WithMany()
                .HasForeignKey(ule => ule.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Screenshot>()
                .HasOne(s => s.Game)
                .WithMany(g => g.imageSource)
                .HasForeignKey(s => s.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PurchaseOption>()
                .HasOne(po => po.Game)
                .WithMany(g => g.PurchaseOptions)
                .HasForeignKey(po => po.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<GoodReceived>()
                .HasOne(gr => gr.PurchaseOption)
                .WithMany(po => po.GoodsReceived)
                .HasForeignKey(gr => gr.PurchaseOptionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CartItem>()
                .HasOne(ci => ci.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(ci => ci.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CartItem>()
                .HasOne(ci => ci.PurchaseOption)
                .WithMany()
                .HasForeignKey(ci => ci.PurchaseOptionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
