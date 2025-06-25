using Microsoft.EntityFrameworkCore;
using ShortieProgress.Api.Data;
using ShortieProgress.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB context
builder.Services.AddDbContext<ShortieDbContext>(options =>
    options.UseSqlServer("Server=localhost,1433;Database=ShortieDb;User Id=sa;Password=Nenormalen0252;TrustServerCertificate=True"));

var app = builder.Build();

// HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Short code
app.MapGet("/r/{shortCode}", async (string shortCode, HttpContext httpContext, ShortieDbContext db) =>
{
    var url = await db.Urls.FirstOrDefaultAsync(u => u.ShortCode == shortCode);
    if (url is null)
        return Results.NotFound(new { status = 1, status_message = "Short URL not found" });

    var ip = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
             ?? httpContext.Connection.RemoteIpAddress?.ToString()
             ?? "unknown";

    var today = DateOnly.FromDateTime(DateTime.UtcNow);

    bool alreadyExists = await db.DailyUniques.AnyAsync(x =>
        x.UrlId == url.Id && x.IpAddress == ip && x.Date == today);

    if (!alreadyExists)
    {
        db.DailyUniques.Add(new DailyUnique
        {
            UrlId = url.Id,
            IpAddress = ip,
            Date = today
        });
    }

    db.Visits.Add(new Visit
    {
        UrlId = url.Id,
        IpAddress = ip,
        VisitedAt = DateTime.UtcNow
    });

    await db.SaveChangesAsync();

    return Results.Redirect(url.LongUrl);
});

// Secret code
app.MapGet("/s/{secretCode}", async (string secretCode, ShortieDbContext db) =>
{
    var url = await db.Urls.FirstOrDefaultAsync(u => u.SecretCode == secretCode);

    if (url == null)
        return Results.NotFound(new { status = 1, status_message = "Secret URL not found" });

    return Results.Redirect($"http://localhost:3000/stats/{secretCode}");
});

// Stats
app.MapGet("/api/stats/{secretCode}", async (string secretCode, ShortieDbContext db) =>
{
    var url = await db.Urls.FirstOrDefaultAsync(u => u.SecretCode == secretCode);
    if (url == null)
        return Results.NotFound(new { status = 1, status_message = "URL not found" });

    // Daily unique visits
    var dailyVisits = await db.DailyUniques
        .Where(d => d.UrlId == url.Id)
        .GroupBy(d => d.Date)
        .Select(g => new {
            date = g.Key,
            count = g.Count()
        })
        .OrderByDescending(d => d.date)
        .ToListAsync();

    // Top 10 IPs
    var topIps = await db.Visits
        .Where(v => v.UrlId == url.Id)
        .GroupBy(v => v.IpAddress)
        .Select(g => new {
            ip = g.Key,
            count = g.Count()
        })
        .OrderByDescending(g => g.count)
        .Take(10)
        .ToListAsync();

    return Results.Ok(new {
        dailyVisits,
        topIps
    });
});

app.Run();