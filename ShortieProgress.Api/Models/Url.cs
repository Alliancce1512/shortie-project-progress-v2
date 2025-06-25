namespace ShortieProgress.Api.Models;

public class Url
{
    public int Id { get; set; }
    public string LongUrl { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public string SecretCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}