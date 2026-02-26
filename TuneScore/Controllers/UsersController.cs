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

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (userId == null)
            {
                return RedirectToAction(nameof(Login));
            }
            var user = await _userService.GetUserByIdAsync(userId.Value);
            if (user == null)
            {
                return RedirectToAction(nameof(Login));
            }
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (userId == null)
                return RedirectToAction(nameof(Login));

            var user = await _userService.GetUserByIdAsync(userId.Value);
            if (user == null)
                return RedirectToAction(nameof(Login));

            var model = new EditProfileViewModel
            {
                IdUser = user.Id,
                Username = user.Username,
                Email = user.Email
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (!string.IsNullOrEmpty(model.NewPassword) && model.NewPassword != model.ConfirmPassword)
                ModelState.AddModelError(nameof(model.ConfirmPassword), "The new password and confirmation do not match.");

            if (!ModelState.IsValid)
                return View(model);

            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (userId == null || userId != model.IdUser)
                return RedirectToAction(nameof(Login));

            var updated = await _userService.UpdateProfileAsync(
                model.IdUser,
                model.Username,
                model.Email,
                string.IsNullOrEmpty(model.NewPassword) ? null : model.NewPassword);

            if (!updated)
                return RedirectToAction(nameof(Login));

            return RedirectToAction(nameof(Profile));
        }
    }
}
