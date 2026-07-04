using System;
using System.ComponentModel.DataAnnotations;
namespace MeterTrackerApi;
public class CreatePremiseDto
{
    [Required]    
    public string Address{get;set;}
    [Required]       
    public int ResponsibleUserId{get;set;}
}