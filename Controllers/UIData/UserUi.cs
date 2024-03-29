﻿namespace SweperBackend.Data
{
    public class UserUi
    {
        public string Id { get; set; }
        public string Photo { get; set; }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }


        public DateTime? DateCreated { get; set; }
        public DateTime? DateLastLogin { get; set; }
        public DateTime? DateLastModified { get; set; }
    }



}
