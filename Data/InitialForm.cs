namespace SweperBackend.Data
{
    public class InitialForm
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int PriceCategory { get; set; }
        public int SpaceCategory { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? DateLastModified { get; set; }
    }



}
