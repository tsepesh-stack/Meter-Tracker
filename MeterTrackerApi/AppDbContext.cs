using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

namespace MeterTrackerApi;

public class AppDbContext : DbContext
{
    public DbSet<User> Users {get; set;} 
    public DbSet<Premise> Premises {get; set;} 
    public DbSet<Reading> Readings {get; set;} 
    public DbSet<Meter> Meters {get; set;} 
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Premise>()
        .HasOne(p=>p.ResponsibleUser)
        .WithMany()
        .HasForeignKey(p => p.ResponsibleUserId)
        .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Reading>()
        .HasOne(r=>r.SubmittedBy)
        .WithMany()
        .HasForeignKey(r=>r.SubmittedById)
        .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Meter>()
        .HasOne(m=>m.Premise)
        .WithMany()
        .HasForeignKey(m=>m.PremiseId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}