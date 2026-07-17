using System;
using System.ComponentModel.DataAnnotations;
namespace MeterTrackerApi;
public class CreatePremiseDto
{
    [Required]    
    public string? Address{get;set;}
    public string? TenantName{get;set;}    
    public int? ResponsibleUserId{get;set;}
}