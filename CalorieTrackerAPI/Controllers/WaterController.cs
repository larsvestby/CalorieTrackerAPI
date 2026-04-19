using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalorieTrackerAPI.Data;
using CalorieTrackerAPI.Models;
using CalorieTrackerAPI.Models.DTOs;
using AutoMapper;

namespace CalorieTrackerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WaterController(AppDbContext context, IMapper mapper) : ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    private int GetCurrentUserId() =>
        int.Parse(User.Claims.First(c => c.Type == "userId").Value);

    // GET api/water?date=2026-04-17
    [HttpGet]
    public async Task<IActionResult> GetMyWaterLogs([FromQuery] DateTime? date)
    {
        var userId = GetCurrentUserId();

        var query = _context.WaterLogs
            .Where(w => w.UserID == userId);

        if (date.HasValue)
            query = query.Where(w => w.Date.Date == date.Value.Date);

        var logs = await query.OrderBy(w => w.Date).ToListAsync();
        return Ok(_mapper.Map<IEnumerable<WaterLogResponseDto>>(logs));
    }

    // GET api/water/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWaterLogById(int id)
    {
        var userId = GetCurrentUserId();

        var log = await _context.WaterLogs
            .FirstOrDefaultAsync(w => w.WaterLogID == id && w.UserID == userId);

        if (log == null) return NotFound();
        return Ok(_mapper.Map<WaterLogResponseDto>(log));
    }

    // POST api/water
    [HttpPost]
    public async Task<IActionResult> CreateWaterLog(CreateWaterLogDto createDto)
    {
        var userId = GetCurrentUserId();

        var user = await _context.Users.FindAsync(userId);
        if (user == null) return NotFound("User not found");

        var log = _mapper.Map<WaterLog>(createDto);
        log.UserID = userId;
        log.User = user;

        _context.WaterLogs.Add(log);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetWaterLogById), new { id = log.WaterLogID },
            _mapper.Map<WaterLogResponseDto>(log));
    }

    // PUT api/water/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWaterLog(int id, UpdateWaterLogDto updateDto)
    {
        var userId = GetCurrentUserId();

        var log = await _context.WaterLogs
            .FirstOrDefaultAsync(w => w.WaterLogID == id && w.UserID == userId);

        if (log == null) return NotFound();

        if (updateDto.MlIngested.HasValue)
            log.MlIngested = updateDto.MlIngested.Value;

        await _context.SaveChangesAsync();
        return Ok(_mapper.Map<WaterLogResponseDto>(log));
    }

    // DELETE api/water/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWaterLog(int id)
    {
        var userId = GetCurrentUserId();

        var log = await _context.WaterLogs
            .FirstOrDefaultAsync(w => w.WaterLogID == id && w.UserID == userId);

        if (log == null) return NotFound();

        _context.WaterLogs.Remove(log);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}