namespace SweperBackend.Data
{
    public class UserRentItem
    {
        public string Id { get; set; }
        public User User { get; set; }
        public int RentItemId { get; set; }
        public RentItem RentItem { get; set; }
        public bool Liked { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? DateViewd { get; set; }
        public DateTime? DateInteraction { get; set; }
    }
}
