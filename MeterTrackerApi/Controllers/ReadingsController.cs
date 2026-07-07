using System;
using MeterTrackerApi;
using Microsoft.AspNetCore.Mvc;
namespace MeterTrackerApi.Controllers;
[ApiController]
[Route("[controller]")]
public class ReadingsController : ControllerBase
{
    private readonly ReadingService _readingService;
    public ReadingsController(ReadingService readingService)
    {
        _readingService=readingService;
    }
    [HttpGet]
    public ActionResult<Reading> Get()
    {
        return Ok(_readingService.GetAll());
    }
    [HttpGet("{id}")]
    public ActionResult<Reading> Get(int id)
    {
        var reading = _readingService.GetById(id);
        if(reading==null) return NotFound();
        return Ok(reading);
    }
    [HttpPost]
    public ActionResult<Reading> Create(CreateReadingDto dto)
    {
        int submittedById=1;
        var reading=_readingService.Create(dto,submittedById);
        return Created($"/readings/{reading.Id}",reading);
    }
    [HttpPut("{id}")]
    public ActionResult<Reading> Update(int id, UpdateReadingDto dto)
    {
        var success = _readingService.Update(id,dto);
        if(!success) return NotFound();
        return Ok(_readingService.GetById(id));
    }
}