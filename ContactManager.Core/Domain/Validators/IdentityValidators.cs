
using FluentValidation;

namespace ContactManager.Core.Domain.Validators {
    public static class IdentityValidators {

        private const string REGEX_USERNAME = @"[^@\.]";
        private const string REGEX_UPPERCASE = @"[A-Z]";
        private const string REGEX_LOWERCASE = @"[a-z]";
        private const string REGEX_DIGIT = @"[0-9]";
        private const string REGEX_SPACE = @"[^\s]";

        public class UsernameValidator : AbstractValidator<string> {
            public UsernameValidator() {
                RuleFor(userName => userName)
                    .NotNull().NotEmpty()
                    .WithMessage("The UserName cannot be empty.")
                    .Matches(REGEX_USERNAME)
                    .WithMessage("The UserName contains invalid characters.");
            }
        }

        public class PasswordValidator : AbstractValidator<string> {
            public PasswordValidator() {
                RuleFor(password => password)
                    .NotNull().NotEmpty()
                    .WithMessage("The Password cannot be empty.")
                    .MinimumLength(6)
                    .WithMessage("The Password must have at least 6 characters.")
                    .Matches(REGEX_UPPERCASE)
                    .WithMessage("The Password must have at least one uppercase letter.")
                    .Matches(REGEX_LOWERCASE)
                    .WithMessage("The Password must have at least one lowercase letter.")
                    .Matches(REGEX_DIGIT)
                    .WithMessage("The Password must have at least one digit.")
                    .Matches(REGEX_SPACE)
                    .WithMessage("The Password cannot contain spaces.");
            }
        }
    }
}
