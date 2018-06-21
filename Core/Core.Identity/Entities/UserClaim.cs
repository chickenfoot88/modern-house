using Core.DataAccess.Interfaces;

namespace Core.Identity.Entities
{
    public class UserClaim: IEntity
    {
        public virtual string ClaimType { get; set; }
        public virtual string ClaimValue { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual long UserId { get; set; }
    }
}
