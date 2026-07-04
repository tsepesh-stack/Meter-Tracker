using System;
namespace MeterTrackerApi;
public enum MeterType{electricity, water_hot, water_cold}
public enum Tariff{T1,T2,T3}
public class Meter
{
    public int Id{get;set;}
    public int PremiseId{get;set;}
    public Premise Premise{get;set;}
    public MeterType MeterType{get;set;}
    public Tariff? Tariff{get;set;}
    public bool IsActive { get; set; } = true;
}