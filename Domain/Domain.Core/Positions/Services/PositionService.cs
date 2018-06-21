using System;
using System.Threading.Tasks;
using Core.DataAccess.Interfaces;
using Domain.Core.Positions.Entities;
using Domain.Core.Positions.Interfaces;

namespace Domain.Core.Positions.Services
{
    /// <summary>
    /// Сервис работы с локациями
    /// </summary>
    public class PositionService : IPositionService
    {
        private readonly IDataStore DataStore;

        public PositionService(IDataStore dataStore)
        {
            this.DataStore = dataStore;
        }

        private bool Validate(decimal? latitude, decimal? longitude)
        {
            if (!latitude.HasValue || !longitude.HasValue)
            {
                return false;
            }

            if (latitude.HasValue && (latitude < -90 || latitude > 90))
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), "Широта может быть только в интервале от -90 до 90");
            }

            if (longitude.HasValue && (longitude < -180 || longitude > 180))
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), "Долгота может быть только в интервале от -180 до 180");
            }

            return true;
        }

        public Position Create(decimal? latitude, decimal? longitude)
        {
            var isValid = Validate(latitude, longitude);
            if (!isValid)
            {
                return null;
            }
            var entity = new Position
            {
                Latitude = latitude.Value,
                Longitude = longitude.Value
            };
            DataStore.Save(entity);

            return entity;
        }

        public async Task<Position> CreateAsync(decimal? latitude, decimal? longitude)
        {
            var isValid = Validate(latitude, longitude);
            if (!isValid)
            {
                return null;
            }
            var entity = new Position
            {
                Latitude = latitude.Value,
                Longitude = longitude.Value
            };
            await DataStore.SaveAsync(entity);

            return entity;
        }

        public void Update(long id, decimal? latitude, decimal? longitude)
        {
            var isValid = Validate(latitude, longitude);
            if (!isValid)
            {
                return;
            }
            var entity = DataStore.Get<Position>(id);

            entity.Latitude = latitude.Value;
            entity.Longitude = longitude.Value;

            DataStore.SaveChanges();
        }

        public async Task UpdateAsync(long id, decimal? latitude, decimal? longitude)
        {
            var isValid = Validate(latitude, longitude);
            if (!isValid)
            {
                return;
            }
            var entity = DataStore.Get<Position>(id);

            entity.Latitude = latitude.Value;
            entity.Longitude = longitude.Value;

            await DataStore.SaveChangesAsync();
        }

        public void Delete(long id)
        {
            var entity = DataStore.Get<Position>(id);

            DataStore.Delete(entity);
        }

        public async Task DeleteAsync(long id)
        {
            var entity = DataStore.Get<Position>(id);

            await DataStore.DeleteAsync(entity);
        }
    }
}
