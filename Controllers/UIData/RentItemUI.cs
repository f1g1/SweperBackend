namespace SweperBackend.Controllers.UIData
{
    public class RentItemUI
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Currency { get; set; }
        public string Level { get; set; }
        public string Neighborhood { get; set; }
        public int Price { get; set; }
        public int Rooms { get; set; }
        public int Surface { get; set; }
        public string Type { get; set; }
        public List<ImageUi> Images { get; set; }
        public string UserId { get; set; }

        public LocationUi Location { get; set; } = new LocationUi();
        public DateTime? DateCreated { get; set; }
        public DateTime? DateLastLogin { get; set; }
        public DateTime? DateLastModified { get; set; }
    }

}