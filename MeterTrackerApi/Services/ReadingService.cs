using System;
namespace MeterTrackerApi;
public class ReadingService
{
    private List<Reading> _reading = new List<Reading>{};
    public List<Reading> GetAll(){return _reading;}
    public Reading ? GetById(int Id){var reading=_reading.FirstOrDefault(t=>t.Id==Id); return reading;}
    public Reading Create(CreateReadingDto dto, int submittedById)
    {
        var reading = new Reading
        {
            Id=_reading.Any() ? _reading.Max(t=>t.Id)+1 : 1,
            MeterId=dto.MeterId,
            Value=dto.Value,
            ReadingDate=DateTime.Now,
            PhotoUrl=dto.PhotoUrl,
            SubmittedById = submittedById
        };
        _reading.Add(reading);
        return reading;
    }
    public bool Update(int Id, UpdateReadingDto dto)
    {
        var reading = _reading.FirstOrDefault(t=>t.Id==Id);
        if (reading == null)
        {
            return false;
        }
        else
        {
            reading.Value=dto.Value;
            reading.PhotoUrl=dto.PhotoUrl;
            return true;
        }
    }
}