using System;
using Microsoft.AspNetCore.Mvc;
namespace MeterTrackerApi.Controllers;
[ApiController] 
[Route("[controller]")]
public class MetersController : ControllerBase
{
private readonly MeterService _meterService;
public MetersController(MeterService meterService)
{
    _meterService=meterService;
}
[HttpGet]
public async Task<ActionResult<Meter>> Get()
{
    return Ok(await _meterService.GetAll());
}
[HttpGet("{id}")]
public async Task<ActionResult<Meter>> Get(int id)
    {
        var meter = await _meterService.GetById(id);
        if(meter==null){return NotFound();}
        return Ok(meter);
    }
[HttpPost]
public async Task<ActionResult<Meter>> Create(CreateMeterDto dto)
    {
        var meter = await _meterService.Create(dto);
        return Created($"/meters/{meter.Id}", meter);
    }
[HttpPut("{id}")]
public async Task<ActionResult<Meter>> Update(int id, UpdateMeterDto dto){
	var success = await _meterService.Update(id,dto);
	if(!success) return NotFound();
	return Ok(await _meterService.GetById(id));}    
}