using FluentValidation;

using System.Text.RegularExpressions;

namespace ContactManager.Core.Domain.Validators {
    public static class AddressPropertyValidators {
        private const RegexOptions REGEX_OPTIONS = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;

        private const string REGEX_POSTAL_CODE = @"^[A-Z]\d[A-Z] ?\d[A-Z]\d$";

        public class StreetNumberValidator : AbstractValidator<int?> {
            public StreetNumberValidator() {
                RuleFor(streetNumber => streetNumber)
                    .NotNull().NotEmpty()
                    .WithMessage("Please provide a Street Number.")
                    .GreaterThan(0)
                    .WithMessage("Please provide a positive Street Number.");
            }
        }

        public class StreetNameValidator : AbstractValidator<string?> {
            public StreetNameValidator() {
                RuleFor(streetName => streetName)
                    .NotNull().NotEmpty()
                    .WithMessage("Please provide a Street Name.")
                    .Length(5, 30)
                    .WithMessage("Please provide a Street Name between 5 and 30 caracters.")
                    .IsValidName()
                    .WithMessage("Please provide a Street Name who contains only letters and spaces.");
            }
        }

        public class CityValidator : AbstractValidator<string?> {
            public CityValidator() {
                RuleFor(city => city)
                    .NotNull().NotEmpty()
                    .WithMessage("Please provide a City Name.")
                    .Length(5, 30)
                    .WithMessage("Please provide a City Name between 5 and 30 caracters.")
                    .IsValidName()
                    .WithMessage("Please provide a City Name who contains only letters and spaces.");
            }
        }

        public class PostalCodeValidator : AbstractValidator<string?> {
            public PostalCodeValidator() {
                RuleFor(postalCode => postalCode)
                    .NotNull().NotEmpty()
                    .WithMessage("Please provide a Postal Code.")
                    .Matches(REGEX_POSTAL_CODE, REGEX_OPTIONS)
                    .WithMessage("Please provide a valid Postal Code.");
            }
        }
    }
}
