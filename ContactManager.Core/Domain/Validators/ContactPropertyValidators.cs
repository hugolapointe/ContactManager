using FluentValidation;

namespace ContactManager.Core.Domain.Validators {
    public static class ContactPropertyValidators {
        private const int VALID_NAME_LENGTH_MIN = 3;
        private const int VALID_NAME_LENGTH_MAX = 30;

        public class FirstNameValidator : AbstractValidator<string> {

            public FirstNameValidator() {
                RuleFor(firstName => firstName)
                    .NotEmpty()
                    .WithMessage("Please provide a First Name.")
                    .Length(VALID_NAME_LENGTH_MIN, VALID_NAME_LENGTH_MAX)
                    .WithMessage("Please provide a First Name between 3 and 30 caracters.")
                    .IsValidName()
                    .WithMessage("Please provide a First Name who contains only letters.");
            }
        }

        public class LastNameValidator : AbstractValidator<string> {
            public LastNameValidator() {
                RuleFor(firstName => firstName)
                    .NotEmpty()
                    .WithMessage("Please provide a Last Name.")
                    .Length(VALID_NAME_LENGTH_MIN, VALID_NAME_LENGTH_MAX)
                    .WithMessage("Please provide a Last Name between 3 and 30 caracters.")
                    .IsValidName()
                    .WithMessage("Please provide a Last Name who contains only letters.");
            }
        }

        public class BirthDateValidator : AbstractValidator<DateTime> {
            public BirthDateValidator() {
                RuleFor(birthDate => birthDate)
                    .NotNull().NotEmpty()
                    .WithMessage("Please provide a Birth Date.")
                    .LessThan(DateTime.Now)
                    .WithMessage("Please provide a valid Birth Date in the past.");
            }
        }
    }
}
