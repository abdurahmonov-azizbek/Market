using Market.Domain.Enums;

namespace Market.Domain.Models
{
    public class GrandRoleDetails
    {
        public long FromUserId { get; set; }
        public long ToUserId { get; set; }
        public Role Role { get; set; }
    }
}
