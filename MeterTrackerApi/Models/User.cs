using System;
namespace MeterTrackerApi;
public enum Role{Admin,User}
public class User
{
    public int Id{get;set;}
    public string Name{get;set;}
    public Role Role{get;set;}
}