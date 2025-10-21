using Microsoft.AspNetCore.Mvc;
using OnlineQuizSystem.Models;
using OnlineQuizSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace OnlineQuizSystem.Controllers;

public class QuizController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    private bool IsStudentAuthenticated()
    {
        return HttpContext.Session.GetString("UserRole") == "Student";
    }

    private int? GetStudentId()
    {
        return HttpContext.Session.GetInt32("UserId");
    }

    public async Task<IActionResult> TakeQuiz(int id)
    {
        if (!IsStudentAuthenticated())
            return RedirectToAction("Login", "Account");

        var quiz = await _context.Quizzes
            .Include(q => q.Questions)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (quiz == null)
            return NotFound();

        HttpContext.Session.SetString("QuizStartTime", DateTime.Now.ToString());
        HttpContext.Session.SetInt32("QuizDuration", quiz.Duration);
        HttpContext.Session.SetInt32("CurrentQuiz", quiz.Id);

        return View(quiz);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitQuiz(List<StudentAnswer> answers)
    {
        if (!IsStudentAuthenticated())
            return RedirectToAction("Login", "Account");

        var studentId = GetStudentId();
        var quizId = HttpContext.Session.GetInt32("CurrentQuiz");

        if (studentId == null || quizId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        foreach (var answer in answers)
        {
            answer.StudentId = studentId.Value;
            answer.AnsweredAt = DateTime.Now;

            var question = await _context.Questions.FindAsync(answer.QuestionId);
            if (question != null)
            {
                answer.IsCorrect = (answer.SelectedOption == question.CorrectAnswer);
                answer.MarksObtained = answer.IsCorrect ? question.Marks : 0;
            }

            _context.StudentAnswers.Add(answer);
        }

        await _context.SaveChangesAsync();

        HttpContext.Session.Remove("QuizStartTime");
        HttpContext.Session.Remove("QuizDuration");
        HttpContext.Session.Remove("CurrentQuiz");

        return RedirectToAction("Dashboard", "Student");
    }
}