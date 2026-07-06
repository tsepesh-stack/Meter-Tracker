using System;
namespace MeterTrackerApi;
public class MeterService
{
    private List<Meter> _meters = new List<Meter>
    {
           
    };
    public List<Meter> GetAll(){return _meters;}
    public Meter? GetById(int Id){var meter = _meters.FirstOrDefault(t=>t.Id==Id); return meter;}
    public Meter Create(CreateMeterDto dto){
	var meter=new Meter{
	Id=_meters.Any() ? _meters.Max(t=>t.Id)+1 : 1,
	MeterType =dto.MeterType,  
	Tariff=dto.Tariff,
    PremiseId=dto.PremiseId};
	_meters.Add(meter);
	return meter;}
    public bool Update(int Id,UpdateMeterDto dto)
    {
        var meter = _meters.FirstOrDefault(t=>t.Id==Id);
        if(meter==null){return false;}
        else
        {
            meter.IsActive=dto.IsActive; return true;
        }

    }
}