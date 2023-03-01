using ContactManager.Core.Domain.Entities;

using Microsoft.AspNetCore.Identity;

using System.Security.Claims;

namespace ContactManager.WebSite.Utilities {
    public class Check : ICheck {
        private readonly UserManager<User> userManager;

        public Check(UserManager<User> userManager) {
            this.userManager = userManager;
        }

        public void IsNotNull(object value, string paramName, string errorMessage) {
            if (value is null) {
                throw new ArgumentNullException(paramName, errorMessage);
            }
        }

        public void AreEqual(Guid expected, Guid actual, string errorMessage) {
            if (expected != actual) {
                throw new InvalidOperationException(errorMessage);
            }
        }

        public void IsOwnedByCurrentUser(Contact contact, ClaimsPrincipal user, string errorMessage) {
            var userId = userManager.GetUserId(user);
            if (contact.OwnerId != Guid.Parse(userId)) {
                throw new UnauthorizedAccessException(errorMessage);
            }
        }
    }
}
