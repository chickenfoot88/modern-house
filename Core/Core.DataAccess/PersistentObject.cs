using Core.DataAccess.Interfaces;

namespace Core.DataAccess
{
    public class PersistentObject : IEntity<long>
    {
        public virtual long Id { get; set; }
    }
}
