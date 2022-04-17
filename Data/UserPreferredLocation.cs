using NetTopologySuite.Geometries;

namespace SweperBackend.Data
{
    public class UserPreferredLocation
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Point Location { get; set; }
        public int Radius { get; set; }
        public User User { get; set; }
        public DateTime DateCreated { get; set; }


    }



}
