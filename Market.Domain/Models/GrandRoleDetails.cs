using Market.Domain.Enums;

namespace Market.Domain.Models
{
    public class GrandRoleDetails
    {
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
        public Role Role { get; set; }
    }
}
