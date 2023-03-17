﻿
using ContactManager.Core.Domain.Validators;

using FluentValidation;

using System.ComponentModel.DataAnnotations;

namespace ContactManager.WebSite.ViewModels;

public class AddressCreateVM {
    [Display(Name = "Street Number")]
    public int? StreetNumber { get; set; }

    [Display(Name = "Street Name")]
    public string? StreetName { get; set; }

    [Display(Name = "City")]
    public string? CityName { get; set; }

    [Display(Name = "Postal Code")]
    public string? PostalCode { get; set; }

    public class Validator : AbstractValidator<AddressCreateVM> {
        public Validator() {
            RuleFor(vm => vm.StreetNumber)
                .NotNull()
                    .WithMessage("Please provide a street number.")
                .SetValidator(new StreetNumberValidator());

            RuleFor(vm => vm.StreetName)
                .NotNull()
                    .WithMessage("Please provide a street name.")
                .SetValidator(new StreetNameValidator());

            RuleFor(vm => vm.CityName)
                .NotNull()
                    .WithMessage("Please provide a city name.")
                .SetValidator(new CityNameValidator());

            RuleFor(vm => vm.PostalCode)
                .NotNull()
                    .WithMessage("Please provide a postal code.")
                .SetValidator(new PostalCodeValidator());
        }
    }
}
