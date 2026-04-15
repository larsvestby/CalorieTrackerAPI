using CalorieTrackerAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CalorieTrackerAPI.Models.DTOs
{
    public class UserDTO
    {
    }

    public class RegisterUserDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public required string LastName { get; set; }

        [Range(0, 120)]
        public int Age { get; set; }

        [Range(0, 500)]
        public double Weight { get; set; }

        [Range(0, 300)]
        public double Height { get; set; }

        [Required]
        public Gender SelectedGender { get; set; }

        [Required]
        public int ActivityLevelID { get; set; }

        [Required]
        public int GoalID { get; set; }
    }

    public class LoginUserDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }

    public class UpdateUserDto
    {
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [Range(0, 120)]
        public int? Age { get; set; }

        [Range(0, 500)]
        public double? Weight { get; set; }

        [Range(0, 300)]
        public double? Height { get; set; }

        public Gender? SelectedGender { get; set; }

        public int? ActivityLevelID { get; set; }

        public int? GoalID { get; set; }
    }

    public class UserResponseDto
    {
        public int UserID { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public Gender SelectedGender { get; set; }
        public required string ActivityLevel { get; set; }
        public required string Goal { get; set; }
    }

    public class AuthResponseDto
    {
        public required string Token { get; set; }
        public required UserResponseDto User { get; set; }
    }
}
