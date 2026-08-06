using System;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
namespace MeterTrackerApi;
public class PremiseService
{
    private readonly AppDbContext _db;
    private readonly IConnectionMultiplexer _redis;
    private const string PremisesCacheKey = "premises:all";

    public PremiseService(AppDbContext db, IConnectionMultiplexer redis)
    {
        _db = db;
        _redis=redis;
    }
    public async Task<List<Premise>> GetAll()
    {
        var db = _redis.GetDatabase();
        var cacheKey = PremisesCacheKey;
        var cached = await db.StringGetAsync(cacheKey);
        if (!cached.IsNullOrEmpty)
        {
            string json = cached.ToString();
            var option= new JsonSerializerOptions{};
            List<Premise> prem = JsonSerializer.Deserialize<List<Premise>>(json,option) ?? new List<Premise>();
            return prem;
        }
        var aw = await _db.Premises.ToListAsync();
        string json1 = JsonSerializer.Serialize(aw);
        await db.StringSetAsync(PremisesCacheKey, json1, TimeSpan.FromMinutes(5));
        return aw;
    }
    public async Task<Premise?> GetById(int Id)
    {
        var premise= await _db.Premises.FindAsync(Id);
        return premise;
    }
    public async Task<Premise> Create(CreatePremiseDto dto)
    {
        var db = _redis.GetDatabase();
        var premise = new Premise
        {
            TenantName = dto.TenantName,
            Address = dto.Address,
            ResponsibleUserId = dto.ResponsibleUserId
        };
        _db.Premises.Add(premise);
        await _db.SaveChangesAsync();
        await db.KeyDeleteAsync(PremisesCacheKey);
        return premise;
    }
    public async Task<bool> Update(int Id,UpdatePremiseDto dto)
    {
        var db = _redis.GetDatabase();
        var premise = await _db.Premises.FindAsync(Id);
        if(premise==null){return false;}
        else
        {
            premise.TenantName = dto.TenantName;
            premise.ResponsibleUserId=dto.ResponsibleUserId; 
            await _db.SaveChangesAsync();
            await db.KeyDeleteAsync(PremisesCacheKey);
            return true;
        }
    }
}