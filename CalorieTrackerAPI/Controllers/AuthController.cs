using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalorieTrackerAPI.Data;
using CalorieTrackerAPI.Models;
using CalorieTrackerAPI.Models.DTOs;
using AutoMapper;
using CalorieTrackerAPI.Services.Interfaces;

namespace CalorieTrackerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AppDbContext context, IMapper mapper, IAuthService authService) : ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly IAuthService _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto registerDto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            return BadRequest("Email already exists");

        string passwordHash = _authService.HashPassword(registerDto.Password);

        var user = _mapper.Map<User>(registerDto);
        user.PasswordHash = passwordHash;

        user.ActivityLevel = await _context.ActivityLevels.FindAsync(registerDto.ActivityLevelID)
            ?? throw new Exception("Invalid ActivityLevelID");
        user.Goal = await _context.Goals.FindAsync(registerDto.GoalID)
            ?? throw new Exception("Invalid GoalID");

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = _authService.GenerateJwtToken(user);
        var userResponse = _mapper.Map<UserResponseDto>(user);
        return Ok(new AuthResponseDto { Token = token, User = userResponse });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto loginDto)
    {
        var user = await _context.Users
            .Include(u => u.ActivityLevel)
            .Include(u => u.Goal)
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user == null)
            return Unauthorized("Invalid email or password");

        if (!_authService.VerifyPassword(loginDto.Password, user.PasswordHash))
            return Unauthorized("Invalid email or password");

        var token = _authService.GenerateJwtToken(user);
        var userResponse = _mapper.Map<UserResponseDto>(user);
        return Ok(new AuthResponseDto { Token = token, User = userResponse });
    }
}