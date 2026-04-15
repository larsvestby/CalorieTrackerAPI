using AutoMapper;
using CalorieTrackerAPI.Models.DTOs;

namespace CalorieTrackerAPI.Models.Mappings
{
    public class ActivityLevelProfile : Profile
    {
        public ActivityLevelProfile()
        {
            CreateMap<ActivityLevel, ActivityLevelResponseDto>();
            CreateMap<CreateActivityLevelDto, ActivityLevel>();
        }
    }
}
