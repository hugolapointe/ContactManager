using ContactManager.Core.Domain.Entities;
using ContactManager.WebSite.ViewModels.Account;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.WebSite.Controllers {
    [Authorize]
    public class AccountController : Controller {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager) {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult LogIn(string? returnUrl = null) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(LogInVM vm, string? returnUrl = null) {
            if (!ModelState.IsValid) {
                ViewBag.ReturnUrl = returnUrl;
                return View(vm);
            }

            try {
                var result = await signInManager.PasswordSignInAsync(
                    vm.UserName, vm.Password, vm.RememberMe, false);

                if (!result.Succeeded) {
                    ModelState.AddModelError(string.Empty, "LogIn Failed!");
                    return View(vm);
                }

                if (!string.IsNullOrEmpty(returnUrl)) {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");

            } catch {
                return RedirectToAction("Error", "Home");
            }
        }

        [AllowAnonymous]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterVM vm) {
            if (!ModelState.IsValid) {
                return View(vm);
            }

            try {
                var newUser = new User(vm.UserName);
                var result = await userManager.CreateAsync(newUser);

                if (!result.Succeeded) {
                    ModelState.AddModelError(string.Empty, "Unable to create a new User.");
                    return View(vm);
                }

                await signInManager.SignInAsync(newUser, true);

            } catch {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Manage", "Contact");
        }

        [HttpPost]
        public async Task<IActionResult> LogOut() {
            try {
                await signInManager.SignOutAsync();

                return RedirectToAction("Index", "Home");

            } catch {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
