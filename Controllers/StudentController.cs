using Microsoft.AspNetCore.Mvc;
using OnlineQuizSystem.Models;
using OnlineQuizSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace OnlineQuizSystem.Controllers;

public class StudentController(ApplicationDbContext context) : Controller
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

    public IActionResult Dashboard()
    {
        if (!IsStudentAuthenticated())
            return RedirectToAction("Login", "Account");

        return View();
    }

    public async Task<IActionResult> AvailableQuizzes()
    {
        if (!IsStudentAuthenticated())
            return RedirectToAction("Login", "Account");

        var quizzes = await _context.Quizzes
            .Include(q => q.Questions)
            .ToListAsync();

        return View(quizzes);
    }

    public async Task<IActionResult> Results()
    {
        if (!IsStudentAuthenticated())
            return RedirectToAction("Login", "Account");

        var studentId = GetStudentId();
        if (studentId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var answers = await _context.StudentAnswers
            .Where(sa => sa.StudentId == studentId.Value)
            .ToListAsync();

        return View(answers);
    }
}