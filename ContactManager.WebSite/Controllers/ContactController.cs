using ContactManager.Core;
using ContactManager.Core.Domain.Entities;
using ContactManager.WebSite.Utilities;
using ContactManager.WebSite.ViewModels.Contact;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.WebSite.Controllers {
    [Authorize]
    public class ContactController : Controller {
        private readonly ContactManagerContext context;
        private readonly UserManager<User> userManager;
        private readonly ICheck check;

        public ContactController(
            ContactManagerContext context,
            UserManager<User> userManager,
            ICheck check) {
            this.context = context;
            this.userManager = userManager;
            this.check = check;
        }

        public async Task<IActionResult> Manage() {
            var user = await userManager.GetUserAsync(User);

            context.Entry(user)
                .Collection(user => user.Contacts)
                .Load();

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
        public IActionResult Create(ContactCreateVM vm) {
            if (!ModelState.IsValid) {
                return View(vm);
            }

            var newContact = new Contact() {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                DateOfBirth = vm.DateOfBirth,
                OwnerId = Guid.Parse(userManager.GetUserId(User))
            };
            newContact.Addresses.Add(new Address() {
                StreetNumber = vm.Address_StreetNumber,
                StreetName = vm.Address_StreetName.Trim(),
                CityName = vm.Address_CityName.Trim(),
                PostalCode = vm.Address_PostalCode.Trim(),
            });

            context.Contacts.Add(newContact);
            context.SaveChanges();

            return RedirectToAction(nameof(Manage));
        }

        public IActionResult Edit(Guid id) {
            var toEdit = context.Contacts.Find(id);

            check.IsNotNull(toEdit, "Contact not found.");
            check.IsOwnedByCurrentUser(toEdit, User);

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

            check.IsNotNull(toEdit, "Contact not found.");
            check.IsOwnedByCurrentUser(toEdit, User);

            toEdit.FirstName = vm.FirstName;
            toEdit.LastName = vm.LastName;
            toEdit.DateOfBirth = vm.DateOfBirth;
            context.SaveChanges();

            return RedirectToAction(nameof(Manage));
        }

        public IActionResult Remove(Guid id) {
            var toRemove = context.Contacts.Find(id);

            check.IsNotNull(toRemove, "Contact not found.");
            check.IsOwnedByCurrentUser(toRemove, User);

            context.Contacts.Remove(toRemove);
            context.SaveChanges();

            return RedirectToAction(nameof(Manage));
        }
    }
}