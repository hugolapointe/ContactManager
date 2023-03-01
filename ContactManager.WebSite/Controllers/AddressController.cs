using ContactManager.Core;
using ContactManager.Core.Domain.Entities;
using ContactManager.WebSite.Utilities;
using ContactManager.WebSite.ViewModels;
using ContactManager.WebSite.ViewModels.Address;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.WebSite.Controllers {
    [Authorize]
    public class AddressController : Controller {
        private readonly ContactManagerContext context;
        private readonly UserManager<User> userManager;
        private readonly ICheck check;

        public AddressController(
            ContactManagerContext context,
            UserManager<User> userManager,
            ICheck check) {
            this.context = context;
            this.userManager = userManager;
            this.check = check;
        }

        public IActionResult Manage(Guid contactId) {
            var contact = context.Contacts.Find(contactId);

            check.IsNotNull(contact, nameof(contact), "Contact not found.");
            check.IsOwnedByCurrentUser(contact, User);

            context.Entry(contact)
                .Collection(contact => contact.Addresses)
                .Load();

            var vm = contact.Addresses
                .Select(address => new AddressDetailsVM() {
                    Id = address.Id,
                    StreetNumber = address.StreetNumber,
                    StreetName = address.StreetName,
                    City = address.CityName,
                    PostalCode = address.PostalCode,
                });

            ViewBag.ContactId = contactId;
            return View(vm);
        }

        public IActionResult Create(Guid contactId) {
            var contact = context.Contacts.Find(contactId);

            check.IsNotNull(contact, nameof(contact), "Contact not found.");
            check.IsOwnedByCurrentUser(contact, User);

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

            check.IsNotNull(contact, nameof(contact), "Contact not found.");
            check.IsOwnedByCurrentUser(contact, User);

            contact.Addresses.Add(new Address() {
                StreetNumber = vm.StreetNumber,
                StreetName = vm.StreetName,
                CityName = vm.CityName,
                PostalCode = vm.PostalCode,
            });
            context.SaveChanges();

            return RedirectToAction(nameof(Manage), new { contactId });
        }

        public IActionResult Edit(Guid id) {
            var toEdit = context.Addresses.Find(id);

            check.IsNotNull(toEdit, nameof(toEdit), "Address not found.");

            context.Entry(toEdit)
                .Reference(address => address.Contact)
                .Load();

            check.IsOwnedByCurrentUser(toEdit.Contact, User);

            var vm = new AddressEditVM() {
                StreetNumber = toEdit.StreetNumber,
                StreetName = toEdit.StreetName,
                CityName = toEdit.CityName,
                PostalCode = toEdit.PostalCode,
            };

            ViewBag.Id = id;
            ViewBag.ContactId = toEdit.Contact?.Id;
            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(Guid id, AddressEditVM vm) {
            var toEdit = context.Addresses.Find(id);

            if (toEdit is null) {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            if (!ModelState.IsValid) {
                ViewBag.Id = id;
                ViewBag.ContactId = toEdit.ContactId;
                return View(vm);
            }
            
            context.Entry(toEdit)
                .Reference(address => address.Contact)
                .Load();

            var contact = toEdit.Contact;

            check.IsNotNull(contact, nameof(contact), "Contact not found.");
            check.IsOwnedByCurrentUser(contact, User);

            toEdit.StreetNumber = vm.StreetNumber;
            toEdit.StreetName = vm.StreetName;
            toEdit.CityName = vm.CityName;
            toEdit.PostalCode = vm.PostalCode;
            context.SaveChanges();

            return RedirectToAction(nameof(Manage),
                new { contactId = toEdit.Contact?.Id });
        }

        public IActionResult Remove(Guid id) {
            var toRemove = context.Addresses.Find(id);

            check.IsNotNull(toRemove, nameof(toRemove), "Address not found.");

            context.Entry(toRemove)
                .Reference(address => address.Contact)
                .Load();

            check.IsOwnedByCurrentUser(toRemove.Contact, User);

            context.Addresses.Remove(toRemove);
            context.SaveChanges();

            return RedirectToAction(nameof(Manage),
                new { contactId = toRemove.Contact?.Id });
        }
    }
}
