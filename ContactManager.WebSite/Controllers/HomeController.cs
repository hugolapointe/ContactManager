using ContactManager.WebSite.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace ContactManager.WebSite.Controllers;

[Authorize]
public class HomeController : Controller {

    public IActionResult Index() {
        return RedirectToAction("Manage", "Contact");
    }

    [AllowAnonymous]
    public IActionResult Privacy() {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}