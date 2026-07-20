using System;
using Microsoft.EntityFrameworkCore;
namespace MeterTrackerApi;
public class ReadingService
{
    private readonly AppDbContext _db;

    public ReadingService(AppDbContext db)
    {
        _db = db;
    }
    public async Task<List<Reading>> GetAll()
    {
        return await _db.Readings.ToListAsync();
    }
    public async Task<Reading?> GetById(int Id)
    {
        var reading= await _db.Readings.FindAsync(Id); 
        return reading;
    }
    public async Task<Reading?> Create(CreateReadingDto dto, int submittedById)
    {
        var lastReading = await _db.Readings
            .Where(r => r.MeterId == dto.MeterId)
            .OrderByDescending(r => r.ReadingDate)
            .FirstOrDefaultAsync();
        if(lastReading!=null && dto.Value < lastReading.Value)
        {
            return null;
        }
            
        var reading = new Reading
        {
            MeterId=dto.MeterId,
            Value=dto.Value,
            ReadingDate=DateTime.UtcNow,
            PhotoUrl=dto.PhotoUrl,
            SubmittedById = submittedById
        };
        _db.Readings.Add(reading);
        await _db.SaveChangesAsync();
        return reading;
    }
    public async Task<bool> Update(int Id, UpdateReadingDto dto)
    {
        var reading = await _db.Readings.FindAsync(Id);
        if (reading == null)
        {
            return false;
        }
        else
        {
            reading.Value=dto.Value;
            await _db.SaveChangesAsync();
            return true;
        }
    }
    public async Task<bool> UpdatePhoto(int Id, string url)
    {
        var reading = await _db.Readings.FindAsync(Id);
        if (reading == null)
        {
            return false;
        }
        else
        {
            reading.PhotoUrl=url;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}