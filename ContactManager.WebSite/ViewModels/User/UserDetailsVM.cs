using System.ComponentModel.DataAnnotations;

namespace ContactManager.WebSite.ViewModels.User;

public class UserDetailsVM {
    [Display(Name = "Id")]
    public Guid Id { get; set; }

    [Display(Name = "UserName")]
    public string UserName { get; set; }

    [Display(Name = "Role")]
    public string RoleName { get; set; }
}
