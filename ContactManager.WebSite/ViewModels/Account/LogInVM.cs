using ContactManager.Core.Domain.Validators;

using FluentValidation;

using System.ComponentModel.DataAnnotations;

namespace ContactManager.WebSite.ViewModels.Account {
    public class LogInVM {
        [Display(Name = "Username")]
        public string? UserName { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; } = false;

        public class Validator : AbstractValidator<LogInVM> {
            public Validator() {
                RuleFor(x => x.UserName)
                    .NotNull()
                    .WithMessage("Please provide a UserName.")
                    .SetValidator(new IdentityValidators.UsernameValidator());

                RuleFor(vm => vm.Password)
                    .NotNull()
                    .WithMessage("Please provide a Password.")
                    .SetValidator(new IdentityValidators.PasswordValidator());
            }
        }
    }
}
