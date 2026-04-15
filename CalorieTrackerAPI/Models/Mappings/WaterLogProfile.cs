using AutoMapper;
using CalorieTrackerAPI.Models.DTOs;

namespace CalorieTrackerAPI.Models.Mappings
{
    public class WaterLogProfile : Profile
    {
        public WaterLogProfile()
        {
            CreateMap<WaterLog, WaterLogResponseDto>();

            CreateMap<CreateWaterLogDto, WaterLog>()
                .ForMember(dest => dest.User,
                           opt => opt.Ignore());
        }
    }
}
