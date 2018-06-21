using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DataAccess.Params;
using Domain.Core.Positions.Models;
using Domain.Dictionary.Polygons.Models;

namespace Domain.Dictionary.Polygons.Interfaces
{
    public interface IPolygonService
    {
        List<PolygonGetModel> GetAllPolygonModels(StoreLoadParams storeLoadParams);

        PolygonGetModel GetPolygonModel(long id);

        void Create(PolygonSaveModel polygonModel);
        Task CreateAsync(PolygonSaveModel polygonModel);

        void Update(long id, PolygonSaveModel polygonModel);
        Task UpdateAsync(long id, PolygonSaveModel polygonModel);

        void UpdatePosition(long id, PositionModel positionModel);
        Task UpdatePositionAsync(long id, PositionModel positionModel);

        void Delete(long id);
        Task DeleteAsync(long id);
    }
}
