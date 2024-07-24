using Market.Domain.Entities.Common;
using Market.Domain.Enums;

namespace Market.Domain.Entities;

public class User : EntityBase
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public Role Role { get; set; } = Role.Admin;
    public Guid CreatedBy { get; set; }
}
