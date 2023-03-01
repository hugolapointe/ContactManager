using FluentValidation;

using System.Text.RegularExpressions;

namespace ContactManager.Core.Domain.Validators {
    public static class AddressPropertyValidators {
        private const RegexOptions REGEX_OPTIONS = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;

        private const int VALID_NAME_LENGTH_MIN = 5;
        private const int VALID_NAME_LENGTH_MAX = 30;
        private const string REGEX_POSTAL_CODE = @"^[A-Z]\d[A-Z] ?\d[A-Z]\d$";

        public class StreetNumberValidator : AbstractValidator<int> {
            public StreetNumberValidator() {
                RuleFor(streetNumber => streetNumber)
                    .NotEmpty()
                    .WithMessage("Please provide a Street Number.")
                    .GreaterThan(0)
                    .WithMessage("Please provide a positive Street Number.");
            }
        }

        public class StreetNameValidator : AbstractValidator<string> {
            public StreetNameValidator() {
                Transform(streetName => streetName, streetName => streetName.Trim())
                    .NotEmpty()
                    .WithMessage("Please provide a Street Name.")
                    .Length(VALID_NAME_LENGTH_MIN, VALID_NAME_LENGTH_MAX)
                    .WithMessage("Please provide a Street Name between 5 and 30 caracters.")
                    .IsValidName()
                    .WithMessage("Please provide a Street Name who contains only letters and spaces.");
            }
        }

        public class CityNameValidator : AbstractValidator<string> {
            public CityNameValidator() {
                Transform(cityName => cityName, cityName => cityName.Trim())
                    .NotEmpty()
                    .WithMessage("Please provide a City Name.")
                    .Length(VALID_NAME_LENGTH_MIN, VALID_NAME_LENGTH_MAX)
                    .WithMessage("Please provide a City Name between 5 and 30 caracters.")
                    .IsValidName()
                    .WithMessage("Please provide a City Name who contains only letters and spaces.");
            }
        }

        public class PostalCodeValidator : AbstractValidator<string> {
            public PostalCodeValidator() {
                Transform(postalCode => postalCode, postalCode => postalCode.Trim().ToUpper())
                    .NotEmpty()
                    .WithMessage("Please provide a Postal Code.")
                    .Matches(REGEX_POSTAL_CODE, REGEX_OPTIONS)
                    .WithMessage("Please provide a valid Postal Code.");
            }
        }
    }
}
