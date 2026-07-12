using System;
namespace MeterTrackerApi;
public enum Role{Admin,User}
public class User
{
    public int Id{get;set;}
    public required string Name{get;set;}
    public Role Role{get;set;}
    public required string Password{get;set;}
}