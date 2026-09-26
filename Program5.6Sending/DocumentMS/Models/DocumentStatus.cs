using System;

namespace DocumentMS.Models
{
    public class DocumentStatus : EntityBase
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
