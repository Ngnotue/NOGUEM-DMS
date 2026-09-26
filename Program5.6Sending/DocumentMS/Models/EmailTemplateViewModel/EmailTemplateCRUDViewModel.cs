using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentMS.Models.EmailTemplateViewModel
{
    public class EmailTemplateCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Required]
        public string From { get; set; }
        [Required]
        public string To { get; set; }
        [Display(Name = "CC To")]
        public string CCTo { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
        [Display(Name = "Invoice Img")]
        public string InvoiceImg { get; set; }
        [Display(Name = "Invoice Id")]
        public string InvoiceId { get; set; }
        public bool IsActive { get; set; }

        public static implicit operator EmailTemplateCRUDViewModel(EmailTemplate _EmailTemplate)
        {
            return new EmailTemplateCRUDViewModel
            {
                Id = _EmailTemplate.Id,
                From = _EmailTemplate.From,
                To = _EmailTemplate.To,
                CCTo = _EmailTemplate.CCTo,
                Subject = _EmailTemplate.Subject,
                Body = _EmailTemplate.Body,
                InvoiceImg = _EmailTemplate.InvoiceImg,
                InvoiceId = _EmailTemplate.InvoiceId,
                IsActive = _EmailTemplate.IsActive,
                CreatedDate = _EmailTemplate.CreatedDate,
                ModifiedDate = _EmailTemplate.ModifiedDate,
                CreatedBy = _EmailTemplate.CreatedBy,
                ModifiedBy = _EmailTemplate.ModifiedBy,
                Cancelled = _EmailTemplate.Cancelled,
            };
        }

        public static implicit operator EmailTemplate(EmailTemplateCRUDViewModel vm)
        {
            return new EmailTemplate
            {
                Id = vm.Id,
                From = vm.From,
                To = vm.To,
                CCTo = vm.CCTo,
                Subject = vm.Subject,
                Body = vm.Body,
                InvoiceImg = vm.InvoiceImg,
                InvoiceId = vm.InvoiceId,
                IsActive = vm.IsActive,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
