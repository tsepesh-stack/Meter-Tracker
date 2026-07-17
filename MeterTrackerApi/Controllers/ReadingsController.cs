using System;
using System.Security.Claims;
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
    public async Task<ActionResult<Reading>> Get()
    {
        return Ok(await _readingService.GetAll());
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Reading>> Get(int id)
    {
        var reading = await _readingService.GetById(id);
        if(reading==null) return NotFound();
        return Ok(reading);
    }
    [HttpPost]
    public async Task<ActionResult<Reading>> Create(CreateReadingDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.GivenName)?.Value;
        if (userId==null) return Unauthorized();
        var reading= await _readingService.Create(dto,int.Parse(userId));
        return Created($"/readings/{reading.Id}",reading);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<Reading>> Update(int id, UpdateReadingDto dto)
    {
        var success =await  _readingService.Update(id,dto);
        if(!success) return NotFound();
        return Ok(await _readingService.GetById(id));
    }
}