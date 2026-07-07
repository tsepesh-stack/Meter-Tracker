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
    public ActionResult<Premise> Get()
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
    
    if (role == "Admin")
    {
        return Ok(_premiseService.GetAll());
    }
    
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var myPremises = _premiseService.GetAll().Where(p => p.ResponsibleUserId == int.Parse(userId));
    return Ok(myPremises);
    }
    [HttpGet("{id}")]
    public ActionResult<Premise> Get(int id)
    {
        var premise = _premiseService.GetById(id);
        if(premise==null) return NotFound();
        return Ok(premise);
    }
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public ActionResult<Premise> Create(CreatePremiseDto dto)
    {
        var premise = _premiseService.Create(dto);
        return Created($"/premises/{premise.Id}", premise);
    }
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult<Premise> Update(int id, UpdatePremiseDto dto)
    {
        var success = _premiseService.Update(id,dto);
        if(!success) return NotFound();
        return Ok(_premiseService.GetById(id));
    }
}