namespace SweperBackend.Controllers
{
    public class MessageUi
    {
        public int Id { get; set; }
        public string UserRenterId { get; set; }
        public string UserOwnerId { get; set; }
        public bool IsFromOwner { get; set; }
        public int RentItemId { get; set; }
        public string RentItemTitle { get; set; }
        public string Text { get; set; }
        public string Media { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? DateServer { get; set; }
        public DateTime? DateSentFromServer { get; set; }
        public DateTime? DateViewed { get; set; }
    }
}
