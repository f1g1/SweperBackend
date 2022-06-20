namespace SweperBackend.Data
{
    public class Message
    {
        public int Id { get; set; }
        public string UserOwnerId { get; set; }
        public User UserOwner { get; set; }
        public string UserRenterId { get; set; }
        public User UserRenter { get; set; }
        public RentItem RentItem { get; set; }
        public int RentItemId { get; set; }
        public bool IsFromOwner { get; set; }

        public string? Text { get; set; }
        public string? Media { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? DateServer { get; set; }
        public DateTime? DateSentFromServer { get; set; }
        public DateTime? DateViewed { get; set; }
    }
}
