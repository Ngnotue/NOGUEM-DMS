using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DocumentMS.Models.EmailConfigViewModel
{
    public class SendEmailViewModel
    {
        public Int64 DocumentId { get; set; }
        [Display(Name = "Sender Email")]
        [Required]
        public Int64 SenderEmailId { get; set; }        
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool IsSSL { get; set; }
        [Display(Name = "Sender Email")]
        [Required]
        public string SenderEmail { get; set; }
        public string SenderFullName { get; set; }
        [Display(Name = "Subject")]
        [Required]
        public string Subject { get; set; }
        [Display(Name = "Body")]
        [Required]
        public string Body { get; set; }
        [Display(Name = "Receiver Email")]
        [Required]
        public Int64 ReceiverEmailId { get; set; }
        [Display(Name = "Receiver Email")]
        [Required]
        public string ReceiverEmail { get; set; }
        public string ReceiverFullName { get; set; }
        public string FilePath { get; set; }
        public Document Document { get; set; }
        public DocumentFile DocumentFile { get; set; }
        public List<DocumentFile> listDocumentFile { get; set; }

        public static implicit operator SendEmailViewModel(EmailConfigCRUDViewModel _EmailConfigCRUDViewModel)
        {
            return new SendEmailViewModel
            {
                SenderEmail = _EmailConfigCRUDViewModel.Email,
                UserName = _EmailConfigCRUDViewModel.Email,
                Password = _EmailConfigCRUDViewModel.Password,
                Host = _EmailConfigCRUDViewModel.Hostname,
                Port = _EmailConfigCRUDViewModel.Port,
                SenderFullName = _EmailConfigCRUDViewModel.SenderFullName,
            };
        }
        public static implicit operator SendEmailViewModel(SMTPEmailSetting _SMTPEmailSetting)
        {
            return new SendEmailViewModel
            {
                SenderEmail = _SMTPEmailSetting.FromEmail,
                UserName = _SMTPEmailSetting.FromEmail,
                Password = _SMTPEmailSetting.Password,
                Host = _SMTPEmailSetting.Host,
                Port = _SMTPEmailSetting.Port,
                SenderFullName = _SMTPEmailSetting.FromFullName,
            };
        }
    }
}
