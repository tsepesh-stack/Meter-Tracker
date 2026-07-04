using System;
using System.ComponentModel.DataAnnotations;
namespace MeterTrackerApi;
public class CreateReadingDto
{
    [Required]    
    public int MeterId{get;set;}
    [Required]
    public decimal Value{get;set;}
    public string PhotoUrl{get;set;}
}