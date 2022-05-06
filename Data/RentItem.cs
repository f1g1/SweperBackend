using NetTopologySuite.Geometries;

namespace SweperBackend.Data
{
    public class RentItem
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Currency { get; set; }
        public string Level { get; set; }
        public string Neighborhood { get; set; }
        public int Price { get; set; }
        public int Rooms { get; set; }
        public int Surface { get; set; }
        public string Type { get; set; }


        public Point Location { get; set; } = new Point(0, 0);
        public User User { get; set; }
        public int Radius { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateLastLogin { get; set; }
        public DateTime? DateLastModified { get; set; }
        public List<RentItemImage> RentItemImages { get; set; }
    }

    public class RentItemImage
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int Index { get; set; }
        public string Timestamp { get; set; }
        public DateTime? DateCreated { get; set; }

        public RentItem RentItem { get; set; }

    }



}
