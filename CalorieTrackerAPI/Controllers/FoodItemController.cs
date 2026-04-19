using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalorieTrackerAPI.Data;
using CalorieTrackerAPI.Models.DTOs;
using AutoMapper;
using CalorieTrackerAPI.Models;

namespace CalorieTrackerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FoodItemController(AppDbContext context, IMapper mapper) : ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    // GET api/fooditem?search=fooditem
    [HttpGet]
    public async Task<IActionResult> SearchFoodItems([FromQuery] string? search)
    {
        var query = _context.FoodItems.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(f => f.Name.Contains(search) || (f.Brand != null && f.Brand.Contains(search)));
        }
        var items = await query.OrderBy(f => f.Name).ToListAsync();
        return Ok(_mapper.Map<IEnumerable<FoodItemResponseDto>>(items));
    }

    // GET api/fooditem/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFoodItem(int id)
    {
        var item = await _context.FoodItems.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(_mapper.Map<FoodItemResponseDto>(item));
    }

    // POST api/fooditem
    [HttpPost]
    public async Task<IActionResult> CreateFoodItem(CreateFoodItemDto createDto)
    {
        var foodItem = _mapper.Map<FoodItem>(createDto);
        _context.FoodItems.Add(foodItem);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetFoodItem), new { id = foodItem.FoodItemID }, _mapper.Map<FoodItemResponseDto>(foodItem));
    }

    // PUT api/fooditem/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFoodItem(int id, UpdateFoodItemDto updateDto)
    {
        var foodItem = await _context.FoodItems.FindAsync(id);
        if (foodItem == null) return NotFound();

        if (updateDto.Name != null) foodItem.Name = updateDto.Name;
        if (updateDto.Brand != null) foodItem.Brand = updateDto.Brand;
        if (updateDto.Calories.HasValue) foodItem.Calories = updateDto.Calories.Value;
        if (updateDto.Protein.HasValue) foodItem.Protein = updateDto.Protein.Value;
        if (updateDto.Carbohydrates.HasValue) foodItem.Carbohydrates = updateDto.Carbohydrates.Value;
        if (updateDto.Fat.HasValue) foodItem.Fat = updateDto.Fat.Value;

        await _context.SaveChangesAsync();
        return Ok(_mapper.Map<FoodItemResponseDto>(foodItem));
    }

    // DELETE api/fooditem/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFoodItem(int id)
    {
        var foodItem = await _context.FoodItems.FindAsync(id);
        if (foodItem == null) return NotFound();
        _context.FoodItems.Remove(foodItem);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}