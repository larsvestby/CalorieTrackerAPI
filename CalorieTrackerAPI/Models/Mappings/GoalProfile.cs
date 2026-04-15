using AutoMapper;
using CalorieTrackerAPI.Models.DTOs;

namespace CalorieTrackerAPI.Models.Mappings
{
    public class GoalProfile : Profile
    {
        public GoalProfile()
        {
            CreateMap<Goal, GoalResponseDto>();
            CreateMap<CreateGoalDto, Goal>();
        }
    }
}
