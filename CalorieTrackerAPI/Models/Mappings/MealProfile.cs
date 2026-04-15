using AutoMapper;
using CalorieTrackerAPI.Models.DTOs;

namespace CalorieTrackerAPI.Models.Mappings
{
    public class MealProfile : Profile
    {
        public MealProfile()
        {
            CreateMap<Meal, MealResponseDto>()
                .ForMember(dest => dest.MealEntries,
                           opt => opt.MapFrom(src => src.MealEntries))
                .ForMember(dest => dest.TotalCalories,
                           opt => opt.MapFrom(src =>
                               src.MealEntries != null
                               ? src.MealEntries.Sum(me => (me.FoodItem.Calories / 100) * me.QuantityInGrams)
                               : 0))
                .ForMember(dest => dest.TotalProtein,
                           opt => opt.MapFrom(src =>
                               src.MealEntries != null
                               ? src.MealEntries.Sum(me => (me.FoodItem.Protein / 100) * me.QuantityInGrams)
                               : 0))
                .ForMember(dest => dest.TotalCarbohydrates,
                           opt => opt.MapFrom(src =>
                               src.MealEntries != null
                               ? src.MealEntries.Sum(me => (me.FoodItem.Carbohydrates / 100) * me.QuantityInGrams)
                               : 0))
                .ForMember(dest => dest.TotalFat,
                           opt => opt.MapFrom(src =>
                               src.MealEntries != null
                               ? src.MealEntries.Sum(me => (me.FoodItem.Fat / 100) * me.QuantityInGrams)
                               : 0));

            CreateMap<CreateMealDto, Meal>()
                .ForMember(dest => dest.User,
                           opt => opt.Ignore());
        }
    }
}
