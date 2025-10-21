using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineQuizSystem.Models;

public class Quiz
{
    public Quiz()
    {
        Title = string.Empty;
        Description = string.Empty;
        // Simplified collection initialization using []
        Questions = [];
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
    [Display(Name = "Quiz Title")]
    public string Title { get; set; }

    [DataType(DataType.MultilineText)]
    [Display(Name = "Description")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Duration is required")]
    [Range(1, 120, ErrorMessage = "Duration must be between 1 and 120 minutes")]
    [Display(Name = "Duration (minutes)")]
    public int Duration { get; set; } = 10;

    [DataType(DataType.DateTime)]
    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Display(Name = "Created By")]
    public int CreatedBy { get; set; }

    // Simplified collection initialization
    public List<Question> Questions { get; set; }
}