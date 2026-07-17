using System;
using System.ComponentModel.DataAnnotations;
namespace MeterTrackerApi;
public class UpdatePremiseDto
{
    public string? TenantName{get;set;}      
    public int? ResponsibleUserId{get;set;}
}