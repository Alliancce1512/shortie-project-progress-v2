using Microsoft.EntityFrameworkCore;
using ShortieProgress.Api.Models;

namespace ShortieProgress.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Url> Urls => Set<Url>();
    public DbSet<Visit> Visits => Set<Visit>();
    public DbSet<DailyUnique> DailyUniques => Set<DailyUnique>();
}