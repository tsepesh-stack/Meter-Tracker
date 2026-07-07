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
public ActionResult<Meter> Get()
{
    return Ok(_meterService.GetAll());
}
[HttpGet("{id}")]
public ActionResult<Meter> Get(int id)
    {
        var meter = _meterService.GetById(id);
        if(meter==null){return NotFound();}
        return Ok(meter);
    }
[HttpPost]
public ActionResult<Meter> Create(CreateMeterDto dto)
    {
        var meter = _meterService.Create(dto);
        return Created($"/meters/{meter.Id}", meter);
    }
[HttpPut("{id}")]
public ActionResult<Meter> Update(int id, UpdateMeterDto dto){
	var success = _meterService.Update(id,dto);
	if(!success) return NotFound();
	return Ok(_meterService.GetById(id));}    
}