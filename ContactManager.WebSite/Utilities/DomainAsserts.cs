using ContactManager.Core.Domain.Entities;

using Microsoft.AspNetCore.Identity;

using System.Security.Claims;

namespace ContactManager.WebSite.Utilities;

public class DomainAsserts {
    private readonly UserManager<User> userManager;

    public DomainAsserts(UserManager<User> userManager) {
        this.userManager = userManager;
    }

    public void Exists(object entity, string errorMessage = "The resource cannot be found.") {
        if (entity is null) {
            throw new ArgumentNullException(errorMessage);
        }
    }

    public void IsOwnedByCurrentUser(object entity, ClaimsPrincipal user,
                string errorMessage = "You must own the resource.") {
        var userId = userManager.GetUserId(user);

        var userIdProperty = entity.GetType().GetProperty("UserId");

        if (userIdProperty is null || (userIdProperty.GetValue(entity) as string) != userId) {
            throw new UnauthorizedAccessException(errorMessage);
        }
    }
}
