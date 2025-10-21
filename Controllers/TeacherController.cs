using Microsoft.AspNetCore.Mvc;
using OnlineQuizSystem.Models;
using OnlineQuizSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace OnlineQuizSystem.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeacherController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ------------------- Dashboard -------------------
        public IActionResult Dashboard()
        {
            return View();
        }

        // ------------------- Create Question -------------------
        public IActionResult CreateQuestion()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestion(Question question)
        {
            if (!ModelState.IsValid)
            {
                return View(question);
            }

            question.CreatedAt = DateTime.Now; // Add CreatedBy if you track teacher
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Question created successfully!";
            return RedirectToAction("Questions");
        }

        public async Task<IActionResult> Questions()
        {
            var questions = await _context.Questions
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();
            return View(questions);
        }

        // ------------------- Create Quiz -------------------
        public IActionResult CreateQuiz()
        {
            var questions = _context.Questions.ToList();
            return View(questions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateQuiz(string title, string description, int duration, List<int> selectedQuestions)
        {
            if (selectedQuestions == null || !selectedQuestions.Any())
            {
                TempData["Error"] = "Please select at least one question.";
                return RedirectToAction("CreateQuiz");
            }

            var quiz = new Quiz
            {
                Title = title,
                Description = description,
                Duration = duration,
                Questions = _context.Questions.Where(q => selectedQuestions.Contains(q.Id)).ToList()
            };

            _context.Quizzes.Add(quiz);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        // ------------------- Mark Student Answers -------------------
        public async Task<IActionResult> MarkAnswers()
        {
            var answers = await _context.StudentAnswers
                .Where(sa => !sa.IsMarked)
                .ToListAsync();
            return View(answers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMarks(int answerId, int marks)
        {
            if (marks < 0 || marks > 10)
            {
                TempData["Error"] = "Marks must be between 0 and 10";
                return RedirectToAction("MarkAnswers");
            }

            var answer = await _context.StudentAnswers.FindAsync(answerId);
            if (answer != null)
            {
                answer.TeacherMark = marks;
                answer.IsMarked = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("MarkAnswers");
        }
    }
}
