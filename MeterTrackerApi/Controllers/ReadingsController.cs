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
    private readonly CloudinaryService _cloudinaryService;
    public ReadingsController(ReadingService readingService, CloudinaryService cloudinaryService)
    {
        _readingService=readingService;
        _cloudinaryService = cloudinaryService;
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
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId==null) return Unauthorized();
        var reading= await _readingService.Create(dto,int.Parse(userId));
        if (reading == null)
        {
           return BadRequest("Новое показание не может быть меньше предыдущего.");
        }
        return Created($"/readings/{reading.Id}",reading);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<Reading>> Update(int id, UpdateReadingDto dto)
    {
        var success =await  _readingService.Update(id,dto);
        if(!success) return NotFound();
        return Ok(await _readingService.GetById(id));
    }
    [HttpPost("{id}/photo")]
    public async Task<ActionResult> UploadPhoto(int id, IFormFile file)
    {
        var reading = await _readingService.GetById(id);
        if (reading == null) return NotFound();
    
        var url = await _cloudinaryService.UploadPhoto(file);
    
        var success = await _readingService.UpdatePhoto(id, url);
        if (!success) return BadRequest();
    
        return Ok(new { photoUrl = url });
    }
}