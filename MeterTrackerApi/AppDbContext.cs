using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

namespace MeterTrackerApi;

public class AppDbContext : DbContext
{
    public DbSet<User> Users {get; set;} 
    public DbSet<Premise> Premises {get; set;} 
    public DbSet<Reading> Readings {get; set;} 
    public DbSet<Meter> Meters {get; set;} 
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}