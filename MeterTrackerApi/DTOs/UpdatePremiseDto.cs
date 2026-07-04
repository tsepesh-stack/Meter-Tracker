using System;
using System.ComponentModel.DataAnnotations;
namespace MeterTrackerApi;
public class UpdatePremiseDto
{
    [Required]       
    public int ResponsibleUserId{get;set;}
}