
using FluentValidation;

using System.Text.RegularExpressions;

namespace ContactManager.Core.Domain.Validators;

public static class IdentityValidators {
    private const RegexOptions REGEX_OPTIONS = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;

    private const string USERNAME_REGEX = @"^([a-z0-9_.-])+$";
    private const string UPPERCASE_REGEX = @"[A-Z]";
    private const string LOWERCASE_REGEX = @"[a-z]";
    private const string DIGIT_REGEX = @"[0-9]";
    private const string SPACE_REGEX = @"[^\s]";

    public class UsernameValidator : AbstractValidator<string> {
        public UsernameValidator() {
            Transform(userName => userName, username => username.Trim().ToLower())
                .NotEmpty()
                    .WithMessage("The UserName cannot be empty.")
                .Matches(USERNAME_REGEX, REGEX_OPTIONS)
                    .WithMessage("The UserName contains invalid characters.");
        }
    }

    public class PasswordValidator : AbstractValidator<string> {
        public PasswordValidator() {
            RuleFor(password => password)
                .NotEmpty()
                    .WithMessage("The Password cannot be empty.")
                .MinimumLength(8)
                    .WithMessage("The Password must have at least 6 characters.")
                .Matches(UPPERCASE_REGEX)
                    .WithMessage("The Password must have at least one uppercase letter.")
                .Matches(LOWERCASE_REGEX)
                    .WithMessage("The Password must have at least one lowercase letter.")
                .Matches(DIGIT_REGEX)
                    .WithMessage("The Password must have at least one digit.")
                .Matches(SPACE_REGEX)
                    .WithMessage("The Password cannot contain spaces.");
        }
    }
}
