using System;

namespace DocumentMS.Models
{
    public class UserProfile : EntityBase
    {
        public Int64 UserProfileId { get; set; }
        public string ApplicationUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string ProfilePicture { get; set; }
        public Int64 RoleId { get; set; }
        // Extention list of document this user is allowed to view
        public ICollection<Document> SharedDocuments { get; set; }
    }
}
