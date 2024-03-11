using ContactManager.Core.Domain.Entities;
using ContactManager.WebSite.ViewModels.Account;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.WebSite.Controllers;

[Authorize]
public class AccountController(
    UserManager<User> userManager,
    SignInManager<User> signInManager) : Controller {
    private readonly UserManager<User> userManager = userManager;
    private readonly SignInManager<User> signInManager = signInManager;

    [HttpGet]
    [AllowAnonymous]
    public IActionResult LogIn(string? returnUrl = null) {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogIn(LogInVM vm, string? returnUrl = null) {
        if (!ModelState.IsValid) {
            return View(vm);
        }

        var result = await signInManager.PasswordSignInAsync(
            vm.UserName, vm.Password, isPersistent: vm.RememberMe, lockoutOnFailure: false);

        if (!result.Succeeded) {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(vm);
        }

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) {
            return RedirectToAction("Index", "Home");
        }

        return LocalRedirect(returnUrl);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register() => View();

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterVM vm) {
        if (!ModelState.IsValid) {
            return View(vm);
        }

        var newUser = new User(vm.UserName);
        var result = await userManager.CreateAsync(newUser);

        if (!result.Succeeded) {
            foreach (var error in result.Errors) {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(vm);
        }

        await signInManager.SignInAsync(newUser, isPersistent: true);

        return RedirectToAction("Manage", "Contact");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOut() {
        await signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }
}
