using AutoMapper;
using CalorieTrackerAPI.Models.DTOs;

namespace CalorieTrackerAPI.Models.Mappings
{
    public class FoodItemProfile : Profile
    {
        public FoodItemProfile()
        {
            CreateMap<FoodItem, FoodItemResponseDto>();
            CreateMap<CreateFoodItemDto, FoodItem>();
        }
    }
}
