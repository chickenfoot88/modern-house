using System.Threading.Tasks;
using Domain.Core.Positions.Entities;

namespace Domain.Core.Positions.Interfaces
{
    public interface IPositionService
    {
        Position Create(decimal? latitude, decimal? longitude);
        Task<Position> CreateAsync(decimal? latitude, decimal? longitude);

        void Update(long id, decimal? latitude, decimal? longitude);
        Task UpdateAsync(long id, decimal? latitude, decimal? longitude);

        void Delete(long id);
        Task DeleteAsync(long id);
    }
}
