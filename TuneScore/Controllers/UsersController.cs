using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TuneScore.Models;
using TuneScore.Services;
using TuneScore.Helpers;

namespace TuneScore.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (!UserValidationHelper.ValidateLoginInput(ModelState, username, password))
            {
                ViewData["Username"] = username;
                return View();
            }

            var user = await _userService.AuthenticateAsync(username, password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                ViewData["Username"] = username;
                return View();
            }

            // Store basic user info in session for later access checks
            HttpContext.Session.SetInt32(SessionHelper.UserIdKey, user.Id);
            HttpContext.Session.SetString(SessionHelper.UsernameKey, user.Username);
            HttpContext.Session.SetString(SessionHelper.EmailKey, user.Email);

            return RedirectToAction("Index", "Albums");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new Register());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register model, string ConfirmPassword)
        {
            UserValidationHelper.ValidateRegisterPasswords(ModelState, model.PasswordPlain, ConfirmPassword);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // For now this just calls a placeholder. Implement in UserService.RegisterAsync.
            await _userService.RegisterAsync(model);

            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public IActionResult Logout()
        {
            _userService.Logout();
            return RedirectToAction(nameof(Login));
        }
    }
}
