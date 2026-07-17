using System;
using MeterTrackerApi;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace MeterTrackerApi.Controllers;
[ApiController]
[Route("[controller]")]
public class PremisesController : ControllerBase
{
    private readonly PremiseService _premiseService;
    public PremisesController(PremiseService premiseService)
    {
        _premiseService=premiseService;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<Premise>> Get()
    {
        var allPremises = await _premiseService.GetAll(); 
    
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        if (role == "Admin")
        {
            return Ok(allPremises);
        }
    
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();
        var myPremises = allPremises.Where(p => p.ResponsibleUserId == int.Parse(userId)).ToList();
        return Ok(myPremises);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Premise>> Get(int id)
    {
        var premise = await _premiseService.GetById(id);
        if(premise==null) return NotFound();
        return Ok(premise);
    }
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Premise>> Create(CreatePremiseDto dto)
    {
        var premise = await _premiseService.Create(dto);
        return Created($"/premises/{premise.Id}", premise);
    }
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Premise>> Update(int id, UpdatePremiseDto dto)
    {
        var success = await _premiseService.Update(id,dto);
        if(!success) return NotFound();
        return Ok(await _premiseService.GetById(id));
    }
}