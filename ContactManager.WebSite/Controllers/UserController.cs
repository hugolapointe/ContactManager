using ContactManager.Core.Domain.Entities;
using ContactManager.WebSite.Utilities;
using ContactManager.WebSite.ViewModels.User;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.WebSite.Controllers;

public class UserController : Controller {
    private readonly UserManager<User> userManager;
    private readonly RoleManager<IdentityRole<Guid>> roleManager;
    private readonly DomainAsserts asserts;

    public UserController(
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        DomainAsserts asserts) {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.asserts = asserts;
    }

    public async Task<IActionResult> Manage() {
        var vm = new List<UserDetailsVM>();

        foreach (var user in userManager.Users) {
            var userRoles = await userManager.GetRolesAsync(user);
            vm.Add(new UserDetailsVM {
                Id = user.Id,
                UserName = user.UserName,
                RoleName = userRoles.SingleOrDefault(string.Empty)
            });
        }

        return View(vm);
    }

    public IActionResult Create() {
        var passwordGenerated = PasswordGenerator.Generate();

        return View(new UserCreateVM() {
            Password = passwordGenerated,
            PasswordConfirmation = passwordGenerated
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserCreateVM vm) {
        if (!ModelState.IsValid) {
            return View(vm);
        }

        var role = await roleManager.FindByIdAsync(vm.RoleId.ToString());

        asserts.Exists(role, "Role not found.");

        var toCreate = new User(vm.UserName);
        var result = await userManager.CreateAsync(toCreate, vm.Password);

        if (!result.Succeeded) {
            ModelState.AddModelError(string.Empty, "User creation fail.");
            return View(vm);
        }

        result = await userManager.AddToRoleAsync(toCreate, role.Name);

        if (!result.Succeeded) {
            ModelState.AddModelError(string.Empty, $"Unable to add the user to the role {role.Name}.");
            return View(vm);
        }

        return RedirectToAction(nameof(Manage));
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(Guid id) {
        var user = await userManager.FindByIdAsync(id.ToString());

        asserts.Exists(user, "User not found.");

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var newPassword = PasswordGenerator.Generate();

        var result = await userManager.ResetPasswordAsync(user, token, newPassword);

        if (!result.Succeeded) {
            throw new Exception("Unable to reset password.");
        }

        return View(new ResetPasswordVM {
            UserName = user.UserName,
            NewPassword = newPassword,
        });
    }

    [HttpPost]
    public async Task<IActionResult> Remove(Guid id) {
        var user = await userManager.FindByIdAsync(id.ToString());

        asserts.Exists(user, "User not found.");

        var result = await userManager.DeleteAsync(user!);

        if (!result.Succeeded) {
            throw new Exception("Unable to remove the user.");
        }

        return RedirectToAction(nameof(Manage));
    }
}
