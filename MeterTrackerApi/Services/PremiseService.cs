using System;
using Microsoft.EntityFrameworkCore;
namespace MeterTrackerApi;
public class PremiseService
{
    private readonly AppDbContext _db;

    public PremiseService(AppDbContext db)
    {
        _db = db;
    }
    public async Task<List<Premise>> GetAll()
    {
        return await _db.Premises.ToListAsync();
    }
    public async Task<Premise?> GetById(int Id)
    {
        var premise= await _db.Premises.FindAsync(Id);
        return premise;
    }
    public async Task<Premise> Create(CreatePremiseDto dto)
    {
        var premise = new Premise
        {
            TenantName = dto.TenantName,
            Address = dto.Address,
            ResponsibleUserId = dto.ResponsibleUserId
        };
        _db.Premises.Add(premise);
        await _db.SaveChangesAsync();
        return premise;
    }
    public async Task<bool> Update(int Id,UpdatePremiseDto dto)
    {
        var premise = await _db.Premises.FindAsync(Id);
        if(premise==null){return false;}
        else
        {
            premise.TenantName = dto.TenantName;
            premise.ResponsibleUserId=dto.ResponsibleUserId; 
            await _db.SaveChangesAsync();
            return true;
        }
    }

}