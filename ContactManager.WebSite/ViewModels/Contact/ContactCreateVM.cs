using ContactManager.Core.Domain.Validators;

using FluentValidation;

using System.ComponentModel.DataAnnotations;

namespace ContactManager.WebSite.ViewModels.Contact {
    public class ContactCreateVM {
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Street Number")]
        public int? Address_StreetNumber { get; set; }

        [Display(Name = "Street Name")]
        public string? Address_StreetName { get; set; }

        [Display(Name = "City")]
        public string? Address_City { get; set; }

        [Display(Name = "Postal Code")]
        public string? Address_PostalCode { get; set; }

        [Display(Name ="Terms Accepted?")]
        public bool TermsAccepted { get; set; } = false;

        public class Validator : AbstractValidator<ContactCreateVM> {
            public Validator() {
                RuleFor(vm => vm.FirstName)
                    .SetValidator(new ContactPropertyValidators.FirstNameValidator());

                RuleFor(vm => vm.LastName)
                    .SetValidator(new ContactPropertyValidators.LastNameValidator());

                RuleFor(vm => vm.DateOfBirth)
                    .SetValidator(new ContactPropertyValidators.BirthDateValidator());
                
                RuleFor(vm => vm.Address_StreetNumber)
                    .SetValidator(new AddressPropertyValidators.StreetNumberValidator());

                RuleFor(vm => vm.Address_StreetName)
                    .SetValidator(new AddressPropertyValidators.StreetNameValidator());

                RuleFor(vm => vm.Address_City)
                    .SetValidator(new AddressPropertyValidators.CityValidator());

                RuleFor(vm => vm.Address_PostalCode)
                    .SetValidator(new AddressPropertyValidators.PostalCodeValidator());

                RuleFor(vm => vm.TermsAccepted)
                    .Must(terms => terms == true)
                    .WithMessage("Make sure your contact accept the terms.");
            }
        }
    }
}
