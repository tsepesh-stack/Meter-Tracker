using System.ComponentModel.DataAnnotations;
namespace MeterTrackerApi;
public class RegisterDto
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Password { get; set; }
}