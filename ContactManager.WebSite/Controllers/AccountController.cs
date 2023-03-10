using ContactManager.Core.Domain.Entities;
using ContactManager.WebSite.ViewModels.Account;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.WebSite.Controllers;

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
    public IActionResult LogIn(string? returnUrl = "") {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> LogIn(LogInVM vm, string? returnUrl = "") {
        if (!ModelState.IsValid) {
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }

        try {
            var result = await signInManager.PasswordSignInAsync(
                vm.UserName, vm.Password, vm.RememberMe, false);

            if (!result.Succeeded) {
                ModelState.AddModelError(string.Empty, "Log In Failed. Please try again.");
                ViewBag.ReturnUrl = returnUrl;
                return View(vm);
            }

        } catch {
            ModelState.AddModelError(string.Empty, "Something went wrong. Please try again.");
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }

        return Redirect(returnUrl ?? Url.Action("Home", "Index"));
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
                ModelState.AddModelError(string.Empty, "Unable to register a new user.");
                return View(vm);
            }

            await signInManager.SignInAsync(newUser, true);

        } catch {
            ModelState.AddModelError(string.Empty, "Something went wrong. Please try again.");
            return View(vm);
        }

        return RedirectToAction("Manage", "Contact");
    }

    [HttpPost]
    public async Task<IActionResult> LogOut() {
        await signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }
}
