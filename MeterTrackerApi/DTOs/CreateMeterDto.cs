using System;
using System.ComponentModel.DataAnnotations;

namespace MeterTrackerApi;
public class CreateMeterDto
{
    [Required]  
    public int PremiseId{get;set;}
    [Required]  
    public MeterType MeterType{get;set;}
    public Tariff? Tariff{get;set;}
}