using Microsoft.AspNetCore.Mvc;
using ShortieProgress.Api.Data;
using ShortieProgress.Api.Models;
using System.Security.Cryptography;
using System.Text;

namespace ShortieProgress.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShortenController : ControllerBase
{
    private readonly ShortieDbContext _context;

    public ShortenController(ShortieDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateShortUrl([FromBody] UrlCreateRequest request)
    {
        if (!Uri.IsWellFormedUriString(request.LongUrl, UriKind.Absolute))
            return BadRequest("Invalid URL format.");

        var shortCode = GenerateShortCode();
        var secretCode = GenerateSecretCode(request.LongUrl);

        var url = new Url
        {
            LongUrl = request.LongUrl,
            ShortCode = shortCode,
            SecretCode = secretCode
        };

        _context.Urls.Add(url);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            status = 0,
            status_message = "Command completed successfully!",
            shortUrl = $"https://shortie.presiyangeorgiev.eu/r/{shortCode}",
            secretUrl = $"https://shortie.presiyangeorgiev.eu/s/{secretCode}"
        });
    }

    private string GenerateShortCode()
    {
        return Guid.NewGuid().ToString("N")[..6];
    }

    private string GenerateSecretCode(string longUrl)
    {
        var input = $"{longUrl}-{Guid.NewGuid()}";
        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(hash)[..20].ToLower();
    }
}

public class UrlCreateRequest
{
    public string LongUrl { get; set; } = string.Empty;
}