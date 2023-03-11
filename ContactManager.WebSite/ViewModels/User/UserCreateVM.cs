using ContactManager.Core.Domain.Validators;

using FluentValidation;

using System.ComponentModel.DataAnnotations;

namespace ContactManager.WebSite.ViewModels.User {
    public class UserCreateVM {
        [Display(Name = "UserName")]
        public string? UserName { get; set; }

        [Display(Name = "Role")]
        public Guid RoleId { get; set; }

        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Display(Name = "Password Confirmation")]
        public string? PasswordConfirmation { get; set; }

        public class Validator : AbstractValidator<UserCreateVM> {
            public Validator() {
                RuleFor(vm => vm.UserName)
                    .NotNull()
                    .WithMessage("Please provide a UserName.")
                    .SetValidator(new IdentityValidators.UsernameValidator());

                RuleFor(vm => vm.RoleId)
                    .NotEmpty()
                    .WithMessage("Please provide a RoleId.");

                RuleFor(vm => vm.Password)
                    .NotNull()
                    .WithMessage("Please provide a Password.")
                    .SetValidator(new IdentityValidators.PasswordValidator());

                RuleFor(vm => vm.PasswordConfirmation)
                    .NotNull()
                    .WithMessage("Please provide a Password Confirmation.")
                    .Equal(vm => vm.Password)
                    .WithMessage("The password and confirmation password do not match.");
            }
        }
    }
}
