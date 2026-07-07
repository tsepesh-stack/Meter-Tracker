using System;
using MeterTrackerApi;
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
    public ActionResult<Premise> Get()
    {
        return Ok(_premiseService.GetAll());
    }
    [HttpGet("{id}")]
    public ActionResult<Premise> Get(int id)
    {
        var premise = _premiseService.GetById(id);
        if(premise==null) return NotFound();
        return Ok(premise);
    }
    [HttpPost]
    public ActionResult<Premise> Create(CreatePremiseDto dto)
    {
        var premise = _premiseService.Create(dto);
        return Created($"/premises/{premise.Id}", premise);
    }
    [HttpPut("{id}")]
    public ActionResult<Premise> Update(int id, UpdatePremiseDto dto)
    {
        var success = _premiseService.Update(id,dto);
        if(!success) return NotFound();
        return Ok(_premiseService.GetById(id));
    }
}