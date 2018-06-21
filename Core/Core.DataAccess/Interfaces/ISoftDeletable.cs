using System;

namespace Core.DataAccess.Interfaces
{
    public interface ISoftDeletable : IEntity
    {
        bool Deleted { get; set; }
        DateTime? DeletedDate { get; set; }
    }
}
