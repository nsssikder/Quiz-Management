using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineQuizSystem.Models
{
    public class StudentAnswer
    {
        public StudentAnswer()
        {
            AnsweredAt = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Student ID")]
        public int StudentId { get; set; }

        [Required]
        [Display(Name = "Question ID")]
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "Please select an option")]
        [Range(1, 4, ErrorMessage = "Selected option must be between 1 and 4")]
        [Display(Name = "Selected Option")]
        public int SelectedOption { get; set; }

        [Display(Name = "Is Correct")]
        public bool IsCorrect { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Answered At")]
        public DateTime AnsweredAt { get; set; }

        [Display(Name = "Marks Obtained")]
        public int MarksObtained { get; set; }

        [Range(0, 10, ErrorMessage = "Marks must be between 0 and 10")]
        [Display(Name = "Teacher Mark")]
        public int TeacherMark { get; set; }

        [Display(Name = "Is Marked")]
        public bool IsMarked { get; set; }
    }
}