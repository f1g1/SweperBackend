using AutoMapper;
using NetTopologySuite.Geometries;
using SweperBackend.Controllers;
using SweperBackend.Data;

namespace SweperBackend.Automap
{
    public class SweperProfile : Profile
    {
        public SweperProfile()
        {

            CreateMap<RentItemUI, RentItem>()
                .ForMember(x => x.Location, y => y.MapFrom(z => new Point(z.Latitude, z.Longitude) { SRID = 4326 }));
            CreateMap<RentItem, RentItemUI>()
                .ForMember(x => x.Latitude, y => y.MapFrom(z => z.Location.PointOnSurface.X))
                .ForMember(x => x.Longitude, y => y.MapFrom(z => z.Location.PointOnSurface.Y));

        }

    }
}
