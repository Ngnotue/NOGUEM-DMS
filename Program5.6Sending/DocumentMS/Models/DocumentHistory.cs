using System;

namespace DocumentMS.Models
{
    public class DocumentHistory : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 DocumentId { get; set; }
        public Int64? AssignEmployeeId { get; set; }
        public string Action { get; set; }
        public string Note { get; set; }
    }
}
