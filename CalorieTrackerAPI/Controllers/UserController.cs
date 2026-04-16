using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CalorieTrackerAPI.Data;
using CalorieTrackerAPI.Models;
using CalorieTrackerAPI.Models.DTOs;
using AutoMapper;
using System.Security.Claims;

namespace CalorieTrackerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController(AppDbContext context, IMapper mapper) : ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    private int GetCurrentUserId() =>
        int.Parse(User.Claims.First(c => c.Type == "userId").Value);

    // GET api/user/me
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId = GetCurrentUserId();

        var user = await _context.Users
            .Include(u => u.ActivityLevel)
            .Include(u => u.Goal)
            .FirstOrDefaultAsync(u => u.UserID == userId);

        if (user == null)
            return NotFound("User not found");

        return Ok(_mapper.Map<UserResponseDto>(user));
    }

    // PUT api/user/me
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe(UpdateUserDto updateDto)
    {
        var userId = GetCurrentUserId();

        var user = await _context.Users
            .Include(u => u.ActivityLevel)
            .Include(u => u.Goal)
            .FirstOrDefaultAsync(u => u.UserID == userId);

        if (user == null)
            return NotFound("User not found");

        if (updateDto.FirstName != null) user.FirstName = updateDto.FirstName;
        if (updateDto.LastName != null) user.LastName = updateDto.LastName;
        if (updateDto.Age != null) user.Age = updateDto.Age.Value;
        if (updateDto.Weight != null) user.Weight = updateDto.Weight.Value;
        if (updateDto.Height != null) user.Height = updateDto.Height.Value;
        if (updateDto.SelectedGender != null) user.SelectedGender = updateDto.SelectedGender.Value;

        if (updateDto.ActivityLevelID != null)
        {
            var activityLevel = await _context.ActivityLevels.FindAsync(updateDto.ActivityLevelID.Value);
            if (activityLevel == null)
                return BadRequest("Invalid ActivityLevelID");
            user.ActivityLevel = activityLevel;
            user.ActivityLevelID = activityLevel.ActivityLevelID;
        }

        if (updateDto.GoalID != null)
        {
            var goal = await _context.Goals.FindAsync(updateDto.GoalID.Value);
            if (goal == null)
                return BadRequest("Invalid GoalID");
            user.Goal = goal;
            user.GoalID = goal.GoalID;
        }

        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<UserResponseDto>(user));
    }

    // DELETE api/user/me
    [HttpDelete("me")]
    public async Task<IActionResult> DeleteMe()
    {
        var userId = GetCurrentUserId();

        var user = await _context.Users.FindAsync(userId);

        if (user == null)
            return NotFound("User not found");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}