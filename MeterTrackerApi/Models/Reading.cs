using System;
namespace MeterTrackerApi;
public class Reading
{
    public int Id{get;set;}
    public int MeterId{get;set;}
    public Meter Meter{get;set;} = null!;
    public decimal Value{get;set;}
    public DateTime ReadingDate{get;set;}
    public int SubmittedById { get; set; }
    public User SubmittedBy { get; set; } = null!;
    public string? PhotoUrl{get;set;}
}