using ContactManager.Core.Domain.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Core.Data {
    public static class SeedExtension {
        public static readonly PasswordHasher<User> PASSWORD_HASHER = new();

        public static void Seed(this ModelBuilder builder) {
            AddRoles(builder);
            _ = AddRole(builder, "Utilisateur");
            var hugoUser = AddUser(builder, "hlapointe", "Admin123!");
            AssignRoleToUser(builder, hugoUser, adminRole);
            var cegepAddress = AddAddress(builder, 3000, "Boulevard Boullé", "Saint-Hyacinthe", "J2S 1H9");
            _ = AddContactWithDefaultAddressToUser(builder, "Sébastien", "Pouliot", cegepAddress, hugoUser);
        }

        private static void AssignRoleToUser(ModelBuilder builder, User hugoUser, IdentityRole<Guid> adminRole) {
            builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid> {
                UserId = hugoUser.Id,
                RoleId = adminRole.Id,
            });
        }

        private void IdentityRole<Guid> AddRoles(ModelBuilder builder) {
            builder.Entity<IdentityRole<Guid>>().HasData(Roles.Administrator);
            builder.Entity<IdentityRole<Guid>>().HasData(Roles.Administrator);
        }

        private static Contact AddContactWithDefaultAddressToUser(ModelBuilder builder,
            string firstName, string lastName, Address address, User user) {
            var newContact = new Contact() {
                Id = Guid.NewGuid(),
                OwnerId = user.Id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = new DateTime(1990, 10, 23)
            };
            address.ContactId = newContact.Id;
            builder.Entity<Contact>().HasData(newContact);

            return newContact;
        }

        private static Address AddAddress(ModelBuilder builder,
            int streetNumber, string streetName, string city, string postalCode) {
            var newAddress = new Address() {
                Id = Guid.NewGuid(),
                StreetNumber = streetNumber,
                StreetName = streetName,
                CityName = city,
                PostalCode = postalCode
            };
            builder.Entity<Address>().HasData(newAddress);

            return newAddress;
        }

        private static User AddUser(ModelBuilder builder,
            string userName, string password) {
            var newUser = new User(userName) {
                Id = Guid.NewGuid(),
                NormalizedUserName = userName.ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString()
            };
            newUser.PasswordHash = PASSWORD_HASHER.HashPassword(newUser, password);
            builder.Entity<User>().HasData(newUser);

            return newUser;
        }
    }
}
