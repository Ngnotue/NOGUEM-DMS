using DocumentMS.Helpers;
using DocumentMS.Models;
using DocumentMS.Models.EmailConfigViewModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DocumentMS.Services
{
    public class EmailSender : IEmailSender
    {
        private IFunctional _functional { get; }
        private readonly ICommon _iCommon;

        public EmailSender(IFunctional functional, ICommon iCommon)
        {
            _functional = functional;
            _iCommon = iCommon;
        }
        public async Task<Task> SendEmailAsync(string email, string subject, string message)
        {
            //sendgrid is become default
            SendGridSetting _sendGridOptions = await _iCommon.GetSendGridEmailSetting();
            if (_sendGridOptions.IsDefault)
            {
                _functional.SendEmailBySendGridAsync(_sendGridOptions.SendGridKey,
                                                    _sendGridOptions.FromEmail,
                                                    _sendGridOptions.FromFullName,
                                                    subject,
                                                    message,
                                                    email)
                                                    .Wait();
            }

            //smtp is become default
            SMTPEmailSetting _smtpOptions = await _iCommon.GetSMTPEmailSetting();
            if (_smtpOptions.IsDefault)
            {
                _functional.SendEmailByGmailAsync(_smtpOptions.FromEmail,
                                            _smtpOptions.FromFullName,
                                            subject,
                                            message,
                                            email,
                                            email,
                                            _smtpOptions.UserName,
                                            _smtpOptions.Password,
                                            _smtpOptions.Host,
                                            _smtpOptions.Port,
                                            _smtpOptions.IsSSL)
                                            .Wait();
            }
            return Task.CompletedTask;
        }
        public async Task<Task> SendEmailByGmailAsync(SendEmailViewModel vm)
        {
            MailMessage _MailMessage = new();
            _MailMessage.From = new MailAddress(vm.SenderEmail, vm.SenderFullName);
            _MailMessage.To.Add(new MailAddress(vm.ReceiverEmail, vm.ReceiverFullName));

            _MailMessage.Subject = vm.Subject;
            _MailMessage.Body = vm.Body;
            _MailMessage.IsBodyHtml = true;

            if (vm.Document.IsFileSaveInDB)
            {
                foreach (var item in vm.listDocumentFile)
                {
                    var _DocByte = StaticUtility.Decrypt(item.DocByte, item.Name);
                    Stream _Stream = new MemoryStream(_DocByte);
                    var FileExtension = Regex.Split(item.ContentType, "/");
                    var FileName = item.Name + "." + FileExtension[1];
                    _MailMessage.Attachments.Add(new Attachment(_Stream, FileName, item.ContentType));
                }
            }
            else
            {
                foreach (var item in vm.listDocumentFile)
                {
                    var _FilePath = _iCommon.GetContentPath(item.FilePath);
                    var linuxPath = _FilePath.Replace('\\','/');
                    //Stream _Stream = new MemoryStream(System.IO.File.ReadAllBytes(linuxPath));
                    //var _Attachment = new Attachment(_Stream, item.Name, item.ContentType);
                    var _Attachment = new Attachment(linuxPath);
                    _MailMessage.Attachments.Add(_Attachment);
                }
            }


            using (var smtp = new SmtpClient())
            {
                smtp.UseDefaultCredentials = false;
                var credential = new NetworkCredential
                {
                    UserName = vm.UserName,
                    Password = vm.Password
                };
                smtp.Credentials = credential;
                smtp.Host = vm.Host;
                smtp.Port = vm.Port;
                smtp.EnableSsl = vm.IsSSL;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                await smtp.SendMailAsync(_MailMessage);
            }

            return Task.CompletedTask;
        }
    }
}
