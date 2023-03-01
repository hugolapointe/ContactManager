using FluentValidation;

namespace ContactManager.Core.Domain.Validators {
    public static class ContactPropertyValidators {
        public class FirstNameValidator : AbstractValidator<string> {
            public FirstNameValidator() {
                RuleFor(firstName => firstName)
                    .NotNull().NotEmpty()
                    .WithMessage("Please provide a First Name.")
                    .Length(3, 30)
                    .WithMessage("Please provide a First Name between 3 and 30 caracters.")
                    .IsValidName()
                    .WithMessage("Please provide a First Name who contains only letters.");
            }
        }

        public class LastNameValidator : AbstractValidator<string> {
            public LastNameValidator() {
                RuleFor(firstName => firstName)
                    .NotNull().NotEmpty()
                    .WithMessage("Please provide a Last Name.")
                    .Length(3, 30)
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
