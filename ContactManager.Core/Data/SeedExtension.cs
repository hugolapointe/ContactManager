using ContactManager.Core.Domain.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Core.Data;

public static class SeedExtension {
    private static readonly PasswordHasher<User> passwordHasher = new();

    public static void Seed(this ModelBuilder builder) {
        var adminRole = addRole("Administrator");
        _ = addRole("Utilisateur");
        var hugoUser = addUser("hlapointe", "Admin123!");
        addUserToRole(hugoUser, adminRole);
        var cegepAddress = addAddress(3000, "Boulevard Boullé", "Saint-Hyacinthe", "J2S 1H9");
        _ = addContactWithDefaultAddressToUser("Sébastien", "Pouliot", new DateTime(1980, 02, 06), cegepAddress, hugoUser);

        IdentityRole<Guid> addRole(string name) {
            var newRole = new IdentityRole<Guid> {
                Id = Guid.NewGuid(),
                Name = name,
                NormalizedName = name.ToUpper()
            };
            builder.Entity<IdentityRole<Guid>>().HasData(newRole);

            return newRole;
        }

        void addUserToRole(User user, IdentityRole<Guid> role) {
            builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid> {
                UserId = user.Id,
                RoleId = role.Id,
            });
        }

        User addUser(string userName, string password) {
            var newUser = new User(userName) {
                Id = Guid.NewGuid(),
                NormalizedUserName = userName.ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            newUser.PasswordHash = passwordHasher.HashPassword(newUser, password);
            builder.Entity<User>().HasData(newUser);

            return newUser;
        }

        Address addAddress(int streetNumber, string streetName, string city, string postalCode) {
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

        Contact addContactWithDefaultAddressToUser(string firstName, string lastName, DateTime dob, Address address, User user) {
            var newContact = new Contact() {
                Id = Guid.NewGuid(),
                OwnerId = user.Id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dob,
            };
            address.ContactId = newContact.Id;
            builder.Entity<Contact>().HasData(newContact);

            return newContact;
        }
    }
}