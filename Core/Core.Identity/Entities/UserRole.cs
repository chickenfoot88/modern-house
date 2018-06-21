using Core.DataAccess.Interfaces;

namespace Core.Identity.Entities
{
    public class UserRole : IEntity
    {
        public virtual string RoleId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual long UserId { get; set; }
    }
}
