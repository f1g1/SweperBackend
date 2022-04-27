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

            CreateMap<LocationUi,Point>()
                .ConstructUsing(x=>new Point(0,0))
                .ForMember(x => x.X, y => y.MapFrom(z => z.Latitude))
                .ForMember(x => x.Y, y => y.MapFrom(z => z.Longitude))
                .ReverseMap();

            CreateMap<RentItem, RentItemUI>()
                .ForMember(x => x.Images, y => y.MapFrom(z => z.RentItemImages))
                .AfterMap((x, y) => y.Location.Radius=x.Radius);

            CreateMap<RentItemUI, RentItem>()
                .ForMember(x => x.RentItemImages, y => y.MapFrom(z => z.Images))
                .AfterMap((x, y) => y.Radius=x.Location.Radius);

            CreateMap<RentItemImage, ImageUi>()
                .ForMember(x => x.base64, y => y.MapFrom(z => z.Base64))
                .ForMember(x => x.index, y => y.MapFrom(z => z.Index))
                .ReverseMap();



        }

    }
}
