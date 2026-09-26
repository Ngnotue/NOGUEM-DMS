using System;

namespace DocumentMS.Models
{
    public class DocumentFile : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 DocumentId { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }
        public double Length { get; set; }
        public bool IsFileSaveInDB { get; set; }
        public byte[] DocByte { get; set; }
    }
}
