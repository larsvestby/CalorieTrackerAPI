using System.ComponentModel.DataAnnotations;

namespace CalorieTrackerAPI.Models.DTOs
{
    public class WaterLogDTO
    {
    }

    public class CreateWaterLogDto
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Range(1, 10000)]
        public int MlIngested { get; set; }
    }

    public class UpdateWaterLogDto
    {
        [Range(1, 10000)]
        public int? MlIngested { get; set; }
    }

    public class WaterLogResponseDto
    {
        public int WaterLogID { get; set; }
        public int UserID { get; set; }
        public DateTime Date { get; set; }
        public int MlIngested { get; set; }
    }
}
