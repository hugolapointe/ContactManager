using ContactManager.Core.Domain.Validators;

using FluentValidation;

using System.ComponentModel.DataAnnotations;

namespace ContactManager.WebSite.ViewModels;

public class AddressCreateVM {
    [Display(Name = "Street Number")]
    public int StreetNumber { get; set; }

    [Display(Name = "Street Name")]
    public string? StreetName { get; set; }

    [Display(Name = "City")]
    public string? CityName { get; set; }

    [Display(Name = "Postal Code")]
    public string? PostalCode { get; set; }

    public class Validator : AbstractValidator<AddressCreateVM> {
        public Validator() {
            RuleFor(vm => vm.StreetNumber)
                .SetValidator(new AddressPropertyValidators.StreetNumberValidator());

            RuleFor(vm => vm.StreetName)
                .NotNull()
                    .WithMessage("Please provide a Street Name.")
                .SetValidator(new AddressPropertyValidators.StreetNameValidator());

            RuleFor(vm => vm.CityName)
                .NotNull()
                    .WithMessage("Please provide a City Name.")
                .SetValidator(new AddressPropertyValidators.CityNameValidator());

            RuleFor(vm => vm.PostalCode)
                .NotNull()
                    .WithMessage("Please provide a Postal Code.")
                .SetValidator(new AddressPropertyValidators.PostalCodeValidator());
        }
    }
}
