using ContactManager.Core.Domain.Validators;

using FluentValidation;

using System.ComponentModel.DataAnnotations;

namespace ContactManager.WebSite.ViewModels.Account;

public class RegisterVM {
    [Display(Name = "UserName")]
    public string? UserName { get; set; }

    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display(Name = "Password Confirmation")]
    [DataType(DataType.Password)]
    public string? PasswordConfirmation { get; set; }

    [Display(Name = "Do you accept the terms?")]
    public bool TermsAccepted { get; set; } = false;

    public class Validator : AbstractValidator<RegisterVM> {
        public Validator() {
            RuleFor(x => x.UserName)
                .NotNull()
                    .WithMessage("Please provide a UserName.")
                .SetValidator(new IdentityValidators.UsernameValidator());

            RuleFor(vm => vm.Password)
                .NotNull()
                    .WithMessage("Please provide a Password.")
                .SetValidator(new IdentityValidators.PasswordValidator());

            RuleFor(vm => vm.PasswordConfirmation)
                .NotNull()
                    .WithMessage("Please provide a Password Confirmation.")
                .Equal(vm => vm.Password)
                    .WithMessage("The password and confirmation password do not match.");

            RuleFor(vm => vm.TermsAccepted)
                .Must(terms => terms == true)
                    .WithMessage("You must accept the Terms.");
        }
    }
}
