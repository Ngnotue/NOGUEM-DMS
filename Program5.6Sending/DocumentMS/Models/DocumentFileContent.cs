using System;

namespace DocumentMS.Models
{
    public class DocumentFileContent : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 DocumentId { get; set; }
        public Int64 DocumentFileId { get; set; }
        public string ContentType { get; set; }
        public string Content { get; set; }
    }
}
