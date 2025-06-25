using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShortieProgress.Api.Models;


public class DailyUnique
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int UrlId { get; set; }               // FK
    public Url Url { get; set; } = null!;

    public string IpAddress { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
}