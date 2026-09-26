using System;

namespace DocumentMS.Models
{
    public class Document : EntityBase
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public Int64 CategoriesId { get; set; }
        public string Notes { get; set; }
        public Int64 AssignEmployeeId { get; set; }
        public int DocumentStatus { get; set; }
        public bool IsFileSaveInDB { get; set; }
        public string FilesDirName { get; set; }
        public string FilesPath { get; set; }
        public string Tag01 { get; set; }
        public string Tag02 { get; set; }
        public string Tag03 { get; set; }
        public string Tag04 { get; set; }
        public string Tag05 { get; set; }
        // Extention list of users allowed to view this Document
        public ICollection<UserProfile> SharedUsers { get; set; }
    }
}
