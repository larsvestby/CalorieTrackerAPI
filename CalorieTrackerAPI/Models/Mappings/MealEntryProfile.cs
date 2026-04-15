using AutoMapper;
using CalorieTrackerAPI.Models.DTOs;

namespace CalorieTrackerAPI.Models.Mappings
{
    public class MealEntryProfile : Profile
    {
        public MealEntryProfile()
        {
            CreateMap<MealEntry, MealEntryResponseDto>()
                .ForMember(dest => dest.TotalCalories,
                           opt => opt.MapFrom(src =>
                               (src.FoodItem.Calories / 100) * src.QuantityInGrams))
                .ForMember(dest => dest.TotalProtein,
                           opt => opt.MapFrom(src =>
                               (src.FoodItem.Protein / 100) * src.QuantityInGrams))
                .ForMember(dest => dest.TotalCarbohydrates,
                           opt => opt.MapFrom(src =>
                               (src.FoodItem.Carbohydrates / 100) * src.QuantityInGrams))
                .ForMember(dest => dest.TotalFat,
                           opt => opt.MapFrom(src =>
                               (src.FoodItem.Fat / 100) * src.QuantityInGrams));

            CreateMap<CreateMealEntryDto, MealEntry>()
                .ForMember(dest => dest.Meal,
                           opt => opt.Ignore())
                .ForMember(dest => dest.FoodItem,
                           opt => opt.Ignore());
        }
    }
}
