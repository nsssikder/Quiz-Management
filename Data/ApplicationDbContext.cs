using Microsoft.EntityFrameworkCore;
using OnlineQuizSystem.Models;

namespace OnlineQuizSystem.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<Quiz> Quizzes { get; set; } = null!;
    public DbSet<StudentAnswer> StudentAnswers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed default teacher account
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "teacher",
                Password = "teacher123",
                Role = "Teacher"
            }
        );

        // Configure relationships
        modelBuilder.Entity<Quiz>()
            .HasMany(q => q.Questions)
            .WithMany(); // Many-to-many relationship between Quiz and Question
    }
}