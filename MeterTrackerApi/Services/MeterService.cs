using System;
using Microsoft.EntityFrameworkCore;
namespace MeterTrackerApi;
public class MeterService
{
    private readonly AppDbContext _db;

    public MeterService(AppDbContext db)
    {
        _db = db;
    }
    public async Task<List<Meter>> GetAll()
    {
        return await _db.Meters.ToListAsync();
    }
    public async Task<Meter?> GetById(int Id)
    {
        var meter = await _db.Meters.FindAsync(Id);
        return meter;
    }
    public async Task<Meter> Create(CreateMeterDto dto)
    {
	    var meter=new Meter
        {
	        MeterType = dto.MeterType,  
	        Tariff = dto.Tariff,
            PremiseId = dto.PremiseId
        };
	    _db.Meters.Add(meter);
        await _db.SaveChangesAsync();
	    return meter;
    }
    public async Task<bool> Update(int Id,UpdateMeterDto dto)
    {
        var meter = await _db.Meters.FindAsync(Id);
        if(meter==null){return false;}
        else
        {
            meter.IsActive=dto.IsActive;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}