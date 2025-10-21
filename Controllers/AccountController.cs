using Microsoft.AspNetCore.Mvc;
using OnlineQuizSystem.Models;
using OnlineQuizSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace OnlineQuizSystem.Controllers;

public class AccountController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(User user)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }

        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == user.Username && u.Password == user.Password);

        if (existingUser != null)
        {
            HttpContext.Session.SetInt32("UserId", existingUser.Id);
            HttpContext.Session.SetString("UserRole", existingUser.Role);
            HttpContext.Session.SetString("Username", existingUser.Username);

            if (existingUser.Role == "Teacher")
                return RedirectToAction("Dashboard", "Teacher");
            else
                return RedirectToAction("Dashboard", "Student");
        }

        ViewBag.Error = "Invalid username or password";
        return View();
    }

    public IActionResult StudentRegister()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StudentRegister(User user)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }

        if (await _context.Users.AnyAsync(u => u.Username == user.Username))
        {
            ModelState.AddModelError("Username", "Username already exists");
            return View(user);
        }

        user.Role = "Student";
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return RedirectToAction("Login");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}