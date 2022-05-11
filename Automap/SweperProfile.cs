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

            CreateMap<LocationUi, Point>()
                .ConstructUsing(x => new Point(0, 0))
                .ForMember(x => x.X, y => y.MapFrom(z => z.Latitude))
                .ForMember(x => x.Y, y => y.MapFrom(z => z.Longitude));

            CreateMap<Point, LocationUi>()
                .ForMember(x => x.Latitude, y => y.MapFrom(z => z.X))
                .ForMember(x => x.Longitude, y => y.MapFrom(z => z.Y));

            CreateMap<RentItem, RentItemUI>()
                .ForMember(x => x.Images, y => y.MapFrom(z => z.RentItemImages))
                .AfterMap((x, y) => y.Location.Radius = x.Radius);

            CreateMap<RentItemUI, RentItem>()
                .ForMember(x => x.RentItemImages, y => y.MapFrom(z => z.Images))
                .AfterMap((x, y) => y.Radius = x.Location.Radius);

            CreateMap<RentItemImage, ImageUi>()
                .ForMember(x => x.index, y => y.MapFrom(z => z.Index))
                .ForMember(x => x.path, y => y.MapFrom(z => z.Path))

                .ReverseMap();



        }

    }
}
