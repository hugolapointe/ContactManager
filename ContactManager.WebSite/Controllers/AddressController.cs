using ContactManager.Core;
using ContactManager.Core.Domain.Entities;
using ContactManager.WebSite.ViewModels;
using ContactManager.WebSite.ViewModels.Address;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.WebSite.Controllers {
    [Authorize]
    public class AddressController : Controller {
        private readonly ContactManagerContext context;

        public AddressController(ContactManagerContext context) {
            this.context = context;
        }

        public IActionResult Manage(Guid contactId) {
            var contact = context.Contacts.Find(contactId);

            if (contact is null) {
                throw new ArgumentOutOfRangeException(nameof(contactId));
            }

            context.Entry(contact)
                .Collection(contact => contact.Addresses)
                .Load();

            var vm = contact.Addresses
                .Select(address => new AddressDetailsVM() {
                    Id = address.Id,
                    StreetNumber = address.StreetNumber,
                    StreetName = address.StreetName,
                    City = address.City,
                    PostalCode = address.PostalCode,
                });

            ViewBag.ContactId = contactId;
            return View(vm);
        }

        public IActionResult Create(Guid contactId) {
            var contact = context.Contacts.Find(contactId);

            if (contact is null) {
                throw new ArgumentOutOfRangeException(nameof(contactId));
            }

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

            if (contact is null) {
                throw new ArgumentOutOfRangeException(nameof(contactId));
            }

            contact.Addresses.Add(new Address() {
                StreetNumber = vm.StreetNumber,
                StreetName = vm.StreetName.Trim(),
                City = vm.City.Trim(),
                PostalCode = vm.PostalCode.Trim(),
            });
            context.SaveChanges();

            return RedirectToAction(nameof(Manage), new { contactId });
        }

        public IActionResult Edit(Guid id) {
            var toEdit = context.Addresses.Find(id);

            if (toEdit is null) {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            var vm = new AddressEditVM() {
                StreetNumber = toEdit.StreetNumber,
                StreetName = toEdit.StreetName,
                City = toEdit.City,
                PostalCode = toEdit.PostalCode,
            };

            context.Entry(toEdit)
                .Reference(address => address.Contact)
                .Load();

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

            toEdit.StreetNumber = vm.StreetNumber;
            toEdit.StreetName = vm.StreetName.Trim();
            toEdit.City = vm.City.Trim();
            toEdit.PostalCode = vm.PostalCode.Trim();
            context.SaveChanges();

            context.Entry(toEdit)
                .Reference(address => address.Contact)
                .Load();

            return RedirectToAction(nameof(Manage),
                new { contactId = toEdit.Contact?.Id });
        }

        public IActionResult Remove(Guid id) {
            var toRemove = context.Addresses.Find(id);

            if (toRemove is null) {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            context.Entry(toRemove)
                .Reference(address => address.Contact)
                .Load();

            context.Addresses.Remove(toRemove);
            context.SaveChanges();

            return RedirectToAction(nameof(Manage),
                new { contactId = toRemove.Contact?.Id });
        }
    }
}
