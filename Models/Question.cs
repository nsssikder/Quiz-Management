using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineQuizSystem.Models
{
    public class Question
    {
        public Question()
        {
            QuestionText = string.Empty;
            Option1 = string.Empty;
            Option2 = string.Empty;
            Option3 = string.Empty;
            Option4 = string.Empty;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Question text is required")]
        [Display(Name = "Question")]
        [DataType(DataType.MultilineText)]
        public string QuestionText { get; set; }

        [Required(ErrorMessage = "Option 1 is required")]
        [Display(Name = "Option 1")]
        public string Option1 { get; set; }

        [Required(ErrorMessage = "Option 2 is required")]
        [Display(Name = "Option 2")]
        public string Option2 { get; set; }

        [Required(ErrorMessage = "Option 3 is required")]
        [Display(Name = "Option 3")]
        public string Option3 { get; set; }

        [Required(ErrorMessage = "Option 4 is required")]
        [Display(Name = "Option 4")]
        public string Option4 { get; set; }

        [Required(ErrorMessage = "Correct answer is required")]
        [Range(1, 4, ErrorMessage = "Correct answer must be between 1 and 4")]
        [Display(Name = "Correct Answer")]
        public int CorrectAnswer { get; set; } = 1;

        [Required(ErrorMessage = "Marks are required")]
        [Range(1, 10, ErrorMessage = "Marks must be between 1 and 10")]
        [Display(Name = "Marks")]
        public int Marks { get; set; } = 1;

        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}