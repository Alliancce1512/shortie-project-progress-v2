using Microsoft.EntityFrameworkCore;
using ShortieProgress.Api.Models;

namespace ShortieProgress.Api.Data
{
    public class ShortieDbContext : DbContext
    {
        public ShortieDbContext(DbContextOptions<ShortieDbContext> options) : base(options)
        {
        }

        public DbSet<Url> Urls { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<DailyUnique> DailyUniques { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Url>()
                .HasIndex(u => u.ShortCode)
                .IsUnique();

            modelBuilder.Entity<Url>()
                .HasIndex(u => u.SecretCode)
                .IsUnique();

            modelBuilder.Entity<DailyUnique>()
                .HasKey(d => new { d.UrlId, d.IpAddress, d.Date });
        }
    }
}
