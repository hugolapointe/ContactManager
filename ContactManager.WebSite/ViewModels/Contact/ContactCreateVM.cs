using ContactManager.Core.Domain.Validators;

using FluentValidation;

using System.ComponentModel.DataAnnotations;

namespace ContactManager.WebSite.ViewModels.Contact;

public class ContactCreateVM {
    [Display(Name = "First Name")]
    public string? FirstName { get; set; }

    [Display(Name = "Last Name")]
    public string? LastName { get; set; }

    [Display(Name = "Date Of Birth")]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [Display(Name = "Street Number")]
    public int Address_StreetNumber { get; set; }

    [Display(Name = "Street Name")]
    public string? Address_StreetName { get; set; }

    [Display(Name = "City")]
    public string? Address_CityName { get; set; }

    [Display(Name = "Postal Code")]
    public string? Address_PostalCode { get; set; }

    [Display(Name = "Terms Accepted?")]
    public bool TermsAccepted { get; set; } = false;

    public class Validator : AbstractValidator<ContactCreateVM> {
        public Validator() {
            RuleFor(vm => vm.FirstName)
                .NotNull()
                .WithMessage("Please provide a First Name.")
                .SetValidator(new ContactPropertyValidators.FirstNameValidator());

            RuleFor(vm => vm.LastName)
                .NotNull()
                .WithMessage("Please provide a Last Name.")
                .SetValidator(new ContactPropertyValidators.LastNameValidator());

            RuleFor(vm => vm.DateOfBirth)
                .NotNull()
                    .WithMessage("Please provide a Date Of Birth.")
                .SetValidator(new ContactPropertyValidators.BirthDateValidator());

            RuleFor(vm => vm.Address_StreetNumber)
                .SetValidator(new AddressPropertyValidators.StreetNumberValidator());

            RuleFor(vm => vm.Address_StreetName)
                .NotNull()
                    .WithMessage("Please provide a Street Name.")
                .SetValidator(new AddressPropertyValidators.StreetNameValidator());

            RuleFor(vm => vm.Address_CityName)
                .NotNull()
                    .WithMessage("Please provide a City Name.")
                .SetValidator(new AddressPropertyValidators.CityNameValidator());

            RuleFor(vm => vm.Address_PostalCode)
                .NotNull()
                    .WithMessage("Please provide a Postal Code.")
                .SetValidator(new AddressPropertyValidators.PostalCodeValidator());

            RuleFor(vm => vm.TermsAccepted)
                .Must(terms => terms == true)
                    .WithMessage("Make sure your contact accept the terms.");
        }
    }
}
