using AutoMapper;
using CalorieTrackerAPI.Models.DTOs;

namespace CalorieTrackerAPI.Models.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.ActivityLevel,
                           opt => opt.MapFrom(src => src.ActivityLevel.Name))
                .ForMember(dest => dest.Goal,
                           opt => opt.MapFrom(src => src.Goal.Name));

            CreateMap<RegisterUserDto, User>()
                .ForMember(dest => dest.PasswordHash,
                           opt => opt.Ignore())
                .ForMember(dest => dest.ActivityLevel,
                           opt => opt.Ignore())
                .ForMember(dest => dest.Goal,
                           opt => opt.Ignore());
        }
    }
}
