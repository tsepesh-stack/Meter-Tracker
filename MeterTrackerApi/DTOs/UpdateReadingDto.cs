using System;
using System.ComponentModel.DataAnnotations;
namespace MeterTrackerApi;
public class UpdateReadingDto
{
    [Required]
    public decimal Value{get;set;}
    public string PhotoUrl{get;set;}
}