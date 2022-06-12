namespace SweperBackend.Controllers
{
    public class UserRentItemUi
    {
        public Guid Id { get; set; }
        public int RentItemId { get; set; }
        public long DateViewd { get; set; }
        public long DateInteraction { get; set; }
        public bool Liked { get; set; }
        public RentItemUI? RentItem { get; set; }

    }
}
