using ContactManager.Core;
using ContactManager.Core.Domain.Entities;
using ContactManager.WebSite.Utilities;
using ContactManager.WebSite.ViewModels.Contact;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.WebSite.Controllers;

[Authorize]
public class ContactController : Controller {
    private readonly ContactManagerContext context;
    private readonly UserManager<User> userManager;
    private readonly DomainAsserts asserts;

    public ContactController(
        ContactManagerContext context,
        UserManager<User> userManager,
        DomainAsserts asserts) {
        this.context = context;
        this.userManager = userManager;
        this.asserts = asserts;
    }

    public async Task<IActionResult> Manage() {
        var user = await userManager.GetUserAsync(User);

        context.Entry(user).Collection(u => u.Contacts).Load();

        var contacts = user.Contacts
            .Select(contact => new ContactDetailsVM() {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Age = contact.Age,
            });

        return View(contacts);
    }

    public IActionResult Create() {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ContactCreateVM vm) {
        if (!ModelState.IsValid) {
            return View(vm);
        }

        var toAdd = new Contact() {
            FirstName = vm.FirstName,
            LastName = vm.LastName,
            DateOfBirth = vm.DateOfBirth.Value,
        };
        toAdd.Addresses.Add(new Address() {
            StreetNumber = vm.Address_StreetNumber,
            StreetName = vm.Address_StreetName,
            CityName = vm.Address_CityName,
            PostalCode = vm.Address_PostalCode,
        });

        var user = await userManager.GetUserAsync(User);
        user.Contacts.Add(toAdd);
        context.SaveChanges();

        return RedirectToAction(nameof(Manage));
    }

    public IActionResult Edit(Guid id) {
        var toEdit = context.Contacts.Find(id);

        asserts.Exists(toEdit, "Contact not found.");
        asserts.IsOwnedByCurrentUser(toEdit, User);

        var vm = new ContactEditVM() {
            FirstName = toEdit.FirstName,
            LastName = toEdit.LastName,
            DateOfBirth = toEdit.DateOfBirth,
        };

        ViewBag.Id = id;
        return View(vm);
    }

    [HttpPost]
    public IActionResult Edit(Guid id, ContactEditVM vm) {
        if (!ModelState.IsValid) {
            ViewBag.Id = id;
            return View(vm);
        }

        var toEdit = context.Contacts.Find(id);

        asserts.Exists(toEdit, "Contact not found.");
        asserts.IsOwnedByCurrentUser(toEdit, User);

        toEdit.FirstName = vm.FirstName;
        toEdit.LastName = vm.LastName;
        toEdit.DateOfBirth = vm.DateOfBirth.Value;
        context.SaveChanges();

        return RedirectToAction(nameof(Manage));
    }

    public IActionResult Remove(Guid id) {
        var toRemove = context.Contacts.Find(id);

        asserts.Exists(toRemove, "Contact not found.");
        asserts.IsOwnedByCurrentUser(toRemove, User);

        context.Contacts.Remove(toRemove);
        context.SaveChanges();

        return RedirectToAction(nameof(Manage));
    }
}