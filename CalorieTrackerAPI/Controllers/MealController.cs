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
public class MealController(AppDbContext context, IMapper mapper) : ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    private int GetCurrentUserId() =>
        int.Parse(User.Claims.First(c => c.Type == "userId").Value);

    // GET api/meal?date=2026-04-17
    [HttpGet]
    public async Task<IActionResult> GetMyMeals([FromQuery] DateTime? date)
    {
        var userId = GetCurrentUserId();

        var query = _context.Meals
            .Include(m => m.MealEntries!)
                .ThenInclude(me => me.FoodItem)
            .Where(m => m.UserID == userId);

        if (date.HasValue)
            query = query.Where(m => m.Date.Date == date.Value.Date);

        var meals = await query.OrderBy(m => m.SelectedMealType).ToListAsync();
        return Ok(_mapper.Map<IEnumerable<MealResponseDto>>(meals));
    }

    // GET api/meal/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMealById(int id)
    {
        var userId = GetCurrentUserId();

        var meal = await _context.Meals
            .Include(m => m.MealEntries!)
                .ThenInclude(me => me.FoodItem)
            .FirstOrDefaultAsync(m => m.MealID == id && m.UserID == userId);

        if (meal == null) return NotFound();
        return Ok(_mapper.Map<MealResponseDto>(meal));
    }

    // GET api/meal/range?from=2026-04-01&to=2026-04-17
    [HttpGet("range")]
    public async Task<IActionResult> GetMealsInRange([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var userId = GetCurrentUserId();

        var meals = await _context.Meals
            .Include(m => m.MealEntries!)
                .ThenInclude(me => me.FoodItem)
            .Where(m => m.UserID == userId && m.Date.Date >= from.Date && m.Date.Date <= to.Date)
            .OrderBy(m => m.Date)
            .ThenBy(m => m.SelectedMealType)
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<MealResponseDto>>(meals));
    }

    // POST api/meal
    [HttpPost]
    public async Task<IActionResult> CreateMeal(CreateMealDto createDto)
    {
        var userId = GetCurrentUserId();

        var user = await _context.Users.FindAsync(userId);
        if (user == null) return NotFound("User not found");

        var meal = _mapper.Map<Meal>(createDto);
        meal.UserID = userId;
        meal.User = user;

        _context.Meals.Add(meal);
        await _context.SaveChangesAsync();

        var createdMeal = await _context.Meals
            .Include(m => m.MealEntries!)
                .ThenInclude(me => me.FoodItem)
            .FirstAsync(m => m.MealID == meal.MealID);

        return CreatedAtAction(nameof(GetMealById), new { id = meal.MealID },
            _mapper.Map<MealResponseDto>(createdMeal));
    }

    // PUT api/meal/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMeal(int id, UpdateMealDto updateDto)
    {
        var userId = GetCurrentUserId();

        var meal = await _context.Meals
            .Include(m => m.MealEntries!)
                .ThenInclude(me => me.FoodItem)
            .FirstOrDefaultAsync(m => m.MealID == id && m.UserID == userId);

        if (meal == null) return NotFound();

        if (updateDto.Date.HasValue) meal.Date = updateDto.Date.Value;
        if (updateDto.SelectedMealType.HasValue) meal.SelectedMealType = updateDto.SelectedMealType.Value;

        await _context.SaveChangesAsync();
        return Ok(_mapper.Map<MealResponseDto>(meal));
    }

    // DELETE api/meal/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMeal(int id)
    {
        var userId = GetCurrentUserId();

        var meal = await _context.Meals
            .FirstOrDefaultAsync(m => m.MealID == id && m.UserID == userId);

        if (meal == null) return NotFound();

        _context.Meals.Remove(meal);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // POST api/meal/{mealId}/entries
    [HttpPost("{mealId}/entries")]
    public async Task<IActionResult> AddMealEntry(int mealId, CreateMealEntryDto createDto)
    {
        var userId = GetCurrentUserId();

        var meal = await _context.Meals
            .FirstOrDefaultAsync(m => m.MealID == mealId && m.UserID == userId);
        if (meal == null) return NotFound("Meal not found or access denied");

        var foodItem = await _context.FoodItems.FindAsync(createDto.FoodItemID);
        if (foodItem == null) return NotFound("FoodItem not found");

        var mealEntry = _mapper.Map<MealEntry>(createDto);
        mealEntry.MealID = mealId;
        mealEntry.Meal = meal;
        mealEntry.FoodItem = foodItem;

        _context.MealEntries.Add(mealEntry);
        await _context.SaveChangesAsync();

        var createdEntry = await _context.MealEntries
            .Include(me => me.FoodItem)
            .FirstAsync(me => me.MealEntryID == mealEntry.MealEntryID);

        return Ok(_mapper.Map<MealEntryResponseDto>(createdEntry));
    }

    // PUT api/meal/entries/{entryId}
    [HttpPut("entries/{entryId}")]
    public async Task<IActionResult> UpdateMealEntry(int entryId, UpdateMealEntryDto updateDto)
    {
        var userId = GetCurrentUserId();

        var mealEntry = await _context.MealEntries
            .Include(me => me.Meal)
            .Include(me => me.FoodItem)
            .FirstOrDefaultAsync(me => me.MealEntryID == entryId && me.Meal.UserID == userId);

        if (mealEntry == null) return NotFound();

        if (updateDto.QuantityInGrams.HasValue)
            mealEntry.QuantityInGrams = updateDto.QuantityInGrams.Value;

        await _context.SaveChangesAsync();
        return Ok(_mapper.Map<MealEntryResponseDto>(mealEntry));
    }

    // DELETE api/meal/entries/{entryId}
    [HttpDelete("entries/{entryId}")]
    public async Task<IActionResult> DeleteMealEntry(int entryId)
    {
        var userId = GetCurrentUserId();

        var mealEntry = await _context.MealEntries
            .Include(me => me.Meal)
            .FirstOrDefaultAsync(me => me.MealEntryID == entryId && me.Meal.UserID == userId);

        if (mealEntry == null) return NotFound();

        _context.MealEntries.Remove(mealEntry);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}