using ContactManager.Core.Domain.Validators;

using FluentValidation;

using System.ComponentModel.DataAnnotations;

namespace ContactManager.WebSite.ViewModels.Contact {
    public class ContactEditVM {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public class Validator : AbstractValidator<ContactEditVM> {
            public Validator() {
                RuleFor(vm => vm.FirstName)
                    .SetValidator(new ContactPropertyValidators.FirstNameValidator());

                RuleFor(vm => vm.LastName)
                    .SetValidator(new ContactPropertyValidators.LastNameValidator());

                RuleFor(vm => vm.DateOfBirth)
                    .SetValidator(new ContactPropertyValidators.BirthDateValidator());
            }
        }
    }
}
