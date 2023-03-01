using ContactManager.Core.Domain.Entities;

using System.Security.Claims;

namespace ContactManager.WebSite.Utilities {
    public interface ICheck {
        void IsNotNull(object value, string paramName, string errorMessage = "Value cannot be null.");
        void AreEqual(Guid expected, Guid actual, string errorMessage = "Values are not equal.");
        void IsOwnedByCurrentUser(Contact contact, ClaimsPrincipal user, string errorMessage = "You must own the resource.");
    }
}