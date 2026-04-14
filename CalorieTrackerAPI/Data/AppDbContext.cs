using CalorieTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CalorieTrackerAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<ActivityLevel> ActivityLevels { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealEntry> MealEntries { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<WaterLog> WaterLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityLevel>().HasData(
                new ActivityLevel { ActivityLevelID = 1, Name = "Sedentary", Multiplier = 1.2 },
                new ActivityLevel { ActivityLevelID = 2, Name = "Lightly active", Multiplier = 1.375 },
                new ActivityLevel { ActivityLevelID = 3, Name = "Moderately active", Multiplier = 1.55 },
                new ActivityLevel { ActivityLevelID = 4, Name = "Highly active", Multiplier = 1.725 },
                new ActivityLevel { ActivityLevelID = 5, Name = "Extremely active", Multiplier = 1.9 }
            );
            modelBuilder.Entity<Goal>().HasData(
                new Goal { GoalID = 1, Name = "Weightloss", CalorieAdjustment = -500 },
                new Goal { GoalID = 2, Name = "Maintain", CalorieAdjustment = 0 },
                new Goal { GoalID = 3, Name = "Musclebuilding", CalorieAdjustment = 300 }
            );
        }
    }
}