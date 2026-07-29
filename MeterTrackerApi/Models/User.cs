using System;
namespace MeterTrackerApi;
using System.Text.Json.Serialization;
public enum Role{Admin,User}
public class User
{
    public int Id{get;set;}
    public required string Name{get;set;}
    public Role Role{get;set;}
    [JsonIgnore]
    public string Password{get;set;} = string.Empty;
}