using ContactManager.Core;
using ContactManager.Core.Domain.Entities;
using ContactManager.WebSite.Utilities;
using ContactManager.WebSite.ViewModels;
using ContactManager.WebSite.ViewModels.Address;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.WebSite.Controllers;

[Authorize]
public class AddressController : Controller {
    private readonly ContactManagerContext context;
    private readonly DomainAsserts asserts;

    public AddressController(
        ContactManagerContext context,
        DomainAsserts asserts) {
        this.context = context;
        this.asserts = asserts;
    }

    public IActionResult Manage(Guid contactId) {
        var contact = context.Contacts.Find(contactId);

        asserts.Exists(contact, "Contact not found.");
        asserts.IsOwnedByCurrentUser(contact, User);

        context.Entry(contact).Collection(c => c.Addresses).Load();

        var addresses = contact.Addresses
            .Select(address => new AddressDetailsVM() {
                Id = address.Id,
                StreetNumber = address.StreetNumber,
                StreetName = address.StreetName,
                City = address.CityName,
                PostalCode = address.PostalCode,
            });

        ViewBag.ContactId = contactId;
        return View(addresses);
    }

    public IActionResult Create(Guid contactId) {
        var contact = context.Contacts.Find(contactId);

        asserts.Exists(contact, "Contact not found.");
        asserts.IsOwnedByCurrentUser(contact, User);

        ViewBag.ContactId = contactId;
        return View();
    }

    [HttpPost]
    public IActionResult Create(Guid contactId, AddressCreateVM vm) {
        if (!ModelState.IsValid) {
            ViewBag.ContactId = contactId;
            return View(vm);
        }

        var contact = context.Contacts.Find(contactId);

        asserts.Exists(contact, "Contact not found.");
        asserts.IsOwnedByCurrentUser(contact, User);

        Address toAdd = new Address() {
            StreetNumber = vm.StreetNumber,
            StreetName = vm.StreetName,
            CityName = vm.CityName,
            PostalCode = vm.PostalCode,
        };
        contact.Addresses.Add(toAdd);
        context.SaveChanges();

        return RedirectToAction(nameof(Manage), new { contactId });
    }

    public IActionResult Edit(Guid id) {
        var toEdit = context.Addresses.Find(id);

        asserts.Exists(toEdit, "Address not found.");

        context.Entry(toEdit).Reference(a => a.Contact).Load();

        asserts.Exists(toEdit.Contact, "Contact not found.");
        asserts.IsOwnedByCurrentUser(toEdit.Contact, User);

        var vm = new AddressEditVM() {
            StreetNumber = toEdit.StreetNumber,
            StreetName = toEdit.StreetName,
            CityName = toEdit.CityName,
            PostalCode = toEdit.PostalCode,
        };

        ViewBag.Id = id;
        ViewBag.ContactId = toEdit.Contact.Id;
        return View(vm);
    }

    [HttpPost]
    public IActionResult Edit(Guid id, AddressEditVM vm) {
        var toEdit = context.Addresses.Find(id);

        asserts.Exists(toEdit, "Address not found.");

        if (!ModelState.IsValid) {
            ViewBag.Id = id;
            ViewBag.ContactId = toEdit.ContactId;
            return View(vm);
        }

        context.Entry(toEdit).Reference(a => a.Contact).Load();

        asserts.Exists(toEdit.Contact, "Contact not found.");
        asserts.IsOwnedByCurrentUser(toEdit.Contact, User);

        toEdit.StreetNumber = vm.StreetNumber;
        toEdit.StreetName = vm.StreetName;
        toEdit.CityName = vm.CityName;
        toEdit.PostalCode = vm.PostalCode;
        context.SaveChanges();

        return RedirectToAction(nameof(Manage),
            new { contactId = toEdit.Contact.Id });
    }

    public IActionResult Remove(Guid id) {
        var toRemove = context.Addresses.Find(id);

        asserts.Exists(toRemove, "Address not found.");

        context.Entry(toRemove).Reference(a => a.Contact).Load();

        asserts.Exists(toRemove.Contact, "Contact not found.");
        asserts.IsOwnedByCurrentUser(toRemove.Contact, User);

        context.Addresses.Remove(toRemove);
        context.SaveChanges();

        return RedirectToAction(nameof(Manage),
            new { contactId = toRemove.Contact.Id });
    }
}
