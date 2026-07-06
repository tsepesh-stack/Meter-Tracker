using System;
namespace MeterTrackerApi;
public class PremiseService
{
    private List<Premise> _premise = new List<Premise>{};
    public List<Premise> GetAll(){return _premise;}
    public Premise? GetById(int Id){var premise= _premise.FirstOrDefault(t=>t.Id==Id); return premise;}
    public Premise Create(CreatePremiseDto dto)
    {
        var premise = new Premise
        {
            Id=_premise.Any() ? _premise.Max(t=>t.Id)+1 : 1,
            Address=dto.Address,
            ResponsibleUserId=dto.ResponsibleUserId
        };
        _premise.Add(premise);
        return premise;
    }
    public bool Update(int Id,UpdatePremiseDto dto)
    {
        var premise = _premise.FirstOrDefault(t=>t.Id==Id);
        if(premise==null){return false;}
        else
        {
            premise.ResponsibleUserId=dto.ResponsibleUserId; return true;
        }
    }

}