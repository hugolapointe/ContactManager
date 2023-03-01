using Microsoft.AspNetCore.Identity;

namespace ContactManager.Core.Domain.Entities {
    public class User : IdentityUser<Guid> {

        public List<Contact> Contacts { get; } = new();

        public User(string userName) :
            base(userName) { }
    }
}
