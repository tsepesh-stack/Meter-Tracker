using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace MeterTrackerApi.Controllers;
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly AppDbContext _db;

    public AuthController(IConfiguration config, AppDbContext db)
    {
        _config = config;
        _db= db;
    }
    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Name == dto.Name);
        if (user == null) return Unauthorized();
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password)) return Unauthorized();


        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: creds
        );

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto dto)
    {
        var username = await _db.Users.FirstOrDefaultAsync(u=> u.Name == dto.Name);
        if (username != null) return BadRequest("Пользователь уже существует");

        var user = new User
        {
            Name=dto.Name,
            Password= BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role= Role.User
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok();
    }
}
