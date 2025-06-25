namespace ShortieProgress.Api.Models;

public class DailyUnique
{
    public int Id { get; set; }
    public int UrlId { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
}