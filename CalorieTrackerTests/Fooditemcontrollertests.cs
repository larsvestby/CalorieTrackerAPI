using AutoMapper;
using CalorieTrackerAPI.Controllers;
using CalorieTrackerAPI.Data;
using CalorieTrackerAPI.Models;
using CalorieTrackerAPI.Models.DTOs;
using CalorieTrackerAPI.Models.Mappings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace CalorieTrackerTests;

public class FoodItemControllerTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly FoodItemController _controller;

    public FoodItemControllerTests()
    {
        var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(dbOptions);

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<FoodItemProfile>();
        }, NullLoggerFactory.Instance);
        _mapper = mapperConfig.CreateMapper();

        _controller = new FoodItemController(_context, _mapper);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private async Task<FoodItem> SeedFoodItemAsync(string name = "Apple", string? brand = "From the farm")
    {
        var item = new FoodItem
        {
            Name = name,
            Brand = brand,
            Calories = 52,
            Protein = 0.3,
            Carbohydrates = 14,
            Fat = 0.2
        };
        _context.FoodItems.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    [Fact]
    public async Task SearchFoodItems_NoSearch_ReturnsAllItems()
    {
        await SeedFoodItemAsync("Apple");
        await SeedFoodItemAsync("Banana");

        var result = await _controller.SearchFoodItems(null);

        var ok = Assert.IsType<OkObjectResult>(result);
        var items = Assert.IsAssignableFrom<IEnumerable<FoodItemResponseDto>>(ok.Value);
        Assert.Equal(2, items.Count());
    }

    [Fact]
    public async Task SearchFoodItems_WithNameSearch_ReturnsMatchingItems()
    {
        await SeedFoodItemAsync("Apple");
        await SeedFoodItemAsync("Banana");

        var result = await _controller.SearchFoodItems("App");

        var ok = Assert.IsType<OkObjectResult>(result);
        var items = Assert.IsAssignableFrom<IEnumerable<FoodItemResponseDto>>(ok.Value).ToList();
        Assert.Single(items);
        Assert.Equal("Apple", items[0].Name);
    }

    [Fact]
    public async Task SearchFoodItems_WithBrandSearch_ReturnsMatchingItems()
    {
        var item = new FoodItem { Name = "Oats", Brand = "Quaker", Calories = 370, Protein = 13, Carbohydrates = 67, Fat = 7 };
        _context.FoodItems.Add(item);
        await _context.SaveChangesAsync();
        await SeedFoodItemAsync("Apple", brand: null);

        var result = await _controller.SearchFoodItems("Quaker");

        var ok = Assert.IsType<OkObjectResult>(result);
        var items = Assert.IsAssignableFrom<IEnumerable<FoodItemResponseDto>>(ok.Value).ToList();
        Assert.Single(items);
        Assert.Equal("Oats", items[0].Name);
    }

    [Fact]
    public async Task GetFoodItem_ReturnsItem()
    {
        var seeded = await SeedFoodItemAsync("Apple");

        var result = await _controller.GetFoodItem(seeded.FoodItemID);

        var ok = Assert.IsType<OkObjectResult>(result);
        var dto = Assert.IsType<FoodItemResponseDto>(ok.Value);
        Assert.Equal("Apple", dto.Name);
        Assert.Equal(seeded.FoodItemID, dto.FoodItemID);
    }

    [Fact]
    public async Task CreateFoodItem_ReturnsCreatedAtAction()
    {
        var dto = new CreateFoodItemDto
        {
            Name = "Chicken Breast",
            Brand = "Rema 1000",
            Calories = 165,
            Protein = 31,
            Carbohydrates = 0,
            Fat = 3.6
        };

        var result = await _controller.CreateFoodItem(dto);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        var responseDto = Assert.IsType<FoodItemResponseDto>(created.Value);
        Assert.Equal("Chicken Breast", responseDto.Name);
        Assert.True(responseDto.FoodItemID > 0);
    }

    [Fact]
    public async Task UpdateFoodItem_ReturnsUpdatedItem()
    {
        var seeded = await SeedFoodItemAsync("Apple");
        var updateDto = new UpdateFoodItemDto { Name = "Green Apple", Calories = 55 };

        var result = await _controller.UpdateFoodItem(seeded.FoodItemID, updateDto);

        var ok = Assert.IsType<OkObjectResult>(result);
        var dto = Assert.IsType<FoodItemResponseDto>(ok.Value);
        Assert.Equal("Green Apple", dto.Name);
        Assert.Equal(55, dto.Calories);
    }

    [Fact]
    public async Task UpdateFoodItem_OnlyChangesProvidedFields()
    {
        var seeded = await SeedFoodItemAsync("Apple");
        var updateDto = new UpdateFoodItemDto { Name = "Red Apple" };

        await _controller.UpdateFoodItem(seeded.FoodItemID, updateDto);

        var updated = await _context.FoodItems.FindAsync(seeded.FoodItemID);
        Assert.Equal("Red Apple", updated!.Name);
        Assert.Equal(52, updated.Calories);
        Assert.Equal(0.3, updated.Protein);
    }

    [Fact]
    public async Task DeleteFoodItem_ReturnsNoContent()
    {
        var seeded = await SeedFoodItemAsync("Apple");

        var result = await _controller.DeleteFoodItem(seeded.FoodItemID);

        Assert.IsType<NoContentResult>(result);
    }
}