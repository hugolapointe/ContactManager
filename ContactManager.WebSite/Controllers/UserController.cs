using ContactManager.Core.Domain.Entities;
using ContactManager.WebSite.Utilities;
using ContactManager.WebSite.ViewModels.User;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.WebSite.Controllers;

public class UserController : Controller {
    private readonly UserManager<User> userManager;
    private readonly RoleManager<IdentityRole<Guid>> roleManager;

    public UserController(
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager) {
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public async Task<IActionResult> Manage() {
        var userDetails = new List<UserDetailsVM>();

        foreach (var user in userManager.Users) {
            var userRoles = await userManager.GetRolesAsync(user);
            userDetails.Add(new UserDetailsVM {
                Id = user.Id,
                UserName = user.UserName,
                RoleName = userRoles.SingleOrDefault(string.Empty)
            });
        }

        return View(userDetails);
    }

    public IActionResult Create() {
        var passwordGenerated = PasswordGenerator.Generate();
        var vm = new UserCreateVM() {
            Password = passwordGenerated,
            PasswordConfirmation = passwordGenerated
        };

        ViewBag.Roles = roleManager.Roles.ToList();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserCreateVM vm) {
        if (!ModelState.IsValid) {
            ViewBag.Roles = roleManager.Roles.ToList();
            return View(vm);
        }

        var role = await roleManager.FindByIdAsync(vm.RoleId.ToString());

        if (role is null) {
            ModelState.AddModelError(string.Empty, "Unable to find the role.");
            ViewBag.Roles = roleManager.Roles.ToList();
            return View(vm);
        }

        var toCreate = new User(vm.UserName);
        var result = await userManager.CreateAsync(toCreate, vm.Password);

        if (!result.Succeeded) {
            ModelState.AddModelError(string.Empty, "User creation fail.");
            ViewBag.Roles = roleManager.Roles.ToList();
            return View(vm);
        }

        result = await userManager.AddToRoleAsync(toCreate, role.Name);

        if (!result.Succeeded) {
            ModelState.AddModelError(string.Empty, $"Unable to add the user to the role {role.Name}.");
            ViewBag.Roles = roleManager.Roles.ToList();
            return View(vm);
        }

        return RedirectToAction(nameof(Manage));
    }

    // TODO : GET /User/Update
    // TODO : POST /User/Update
    // TODO : GET /User/Remove
}
