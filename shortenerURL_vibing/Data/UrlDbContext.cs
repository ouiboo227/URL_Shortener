using Microsoft.EntityFrameworkCore;
using shortenerURL_vibing.Models;

namespace shortenerURL_vibing.Data
{
    public class UrlDbContext : DbContext
    {
        public UrlDbContext(DbContextOptions<UrlDbContext> options) : base(options)
        {
        }

        public DbSet<UrlModel> Urls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the UrlModel entity
            modelBuilder.Entity<UrlModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OriginalUrl).IsRequired().HasMaxLength(2000);
                entity.Property(e => e.ShortenedUrl).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CustomAlias).HasMaxLength(50);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.ClickCount).HasDefaultValue(0);
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                // Create unique index on ShortenedUrl
                entity.HasIndex(e => e.ShortenedUrl).IsUnique();
                
                // Create unique index on CustomAlias if not null
                entity.HasIndex(e => e.CustomAlias).IsUnique().HasFilter("[CustomAlias] IS NOT NULL");
            });
        }
    }
}
