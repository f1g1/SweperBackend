﻿namespace SweperBackend.Controllers
{
    public class UserRentItemUi
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RentItemId { get; set; }
        public long DateViewd { get; set; }
        public long DateInteraction { get; set; }
        public bool Liked { get; set; }
        public RentItemUI? RentItem { get; set; }
        public int ChatCount { get; set; }
        public DateTime? DateLastChat { get; set; }
        public DateTime? DateCreated { get; set; }

    }
}
