using Domain.Core.Positions.Entities;

namespace Domain.Core.Positions.Models
{
    public class PositionModel
    {
        public virtual decimal Latitude { get; set; }

        public virtual decimal Longitude { get; set; }

        public Position ToEntity()
        {
            return new Position
            {
                Latitude = this.Latitude,
                Longitude = this.Longitude
            };
        }

        public static PositionModel FromEntity(Position position)
        {
            if (position == null)
                return null;

            return new PositionModel
            {
                Latitude = position.Latitude,
                Longitude = position.Longitude
            };
        }
    }
}
