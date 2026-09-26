using System;

namespace DocumentMS.Models.EmailTemplateViewModel
{
    public class EmailTemplateGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string CCTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string InvoiceImg { get; set; }
        public string InvoiceId { get; set; }
        public bool IsActive { get; set; }
    }
}

