using System;
namespace MeterTrackerApi;
public class Premise
{
    public int Id{get;set;}
    public required string Address{get;set;}
    public int? ResponsibleUserId{get;set;}
    public User? ResponsibleUser{get;set;}
}