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

            CreateMap<UserRentItem, UserRentItemUi>()
                .ForMember(x => x.DateViewd, y => y.MapFrom(z => ConvertDatetimeToUnixTimeStamp(z.DateViewd.Value)))
                .ForMember(x => x.DateInteraction, y => y.MapFrom(z => ConvertDatetimeToUnixTimeStamp(z.DateInteraction.Value)))
                .ForMember(x => x.DateLastChat, y => y.MapFrom(z => ConvertDatetimeToUnixTimeStamp(z.DateLastChat)));

            CreateMap<Message, MessageUi>();
            CreateMap<User, UserUi >();
        }


        public static long? ConvertDatetimeToUnixTimeStamp(DateTime? date)
        {
            if (!date.HasValue)
                return null;
            DateTime originDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.Value.ToUniversalTime() - originDate;
            return (long)Math.Floor(diff.TotalSeconds);
        }

    }
}
