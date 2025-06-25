namespace ShortieProgress.Api.Models;

public class Visit
{
    public int Id { get; set; }
    public int UrlId { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public DateTime VisitedAt { get; set; } = DateTime.UtcNow;
}