using Microsoft.AspNetCore.Identity;

namespace ContactManager.Core.Domain.Entities;

public sealed class Roles {
    public static IdentityRole<Guid> Administrator => new IdentityRole<Guid>("Administrator");
    public static IdentityRole<Guid> User => new IdentityRole<Guid>("User");
}
