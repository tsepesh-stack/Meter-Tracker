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
    public async Task<Reading> Create(CreateReadingDto dto, int submittedById)
    {
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
            reading.PhotoUrl=dto.PhotoUrl;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}