using DocumentMS.ConHelper;
using DocumentMS.Data;
using DocumentMS.Models;
using DocumentMS.Models.CommonViewModel;
using DocumentMS.Models.DashboardViewModel;
using DocumentMS.Models.DocumentHistoryViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DocumentMS.Services
{
    public class Functional : IFunctional
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IRoles _roles;
        private readonly SuperAdminDefaultOptions _superAdminDefaultOptions;
        private readonly ApplicationInfo _applicationInfo;
        private readonly IAccount _iAccount;
        private readonly ICommon _iCommon;
        private readonly IDBOperation _iDBOperation;
        private readonly SeedData _SeedData = new();

        public Functional(UserManager<ApplicationUser> userManager,
           ApplicationDbContext context,
           IRoles roles,
           IOptions<SuperAdminDefaultOptions> superAdminDefaultOptions,
           IOptions<ApplicationInfo> applicationInfo,
           IAccount iAccount,
           ICommon iCommon,
           IDBOperation iDBOperation)
        {
            _userManager = userManager;
            _context = context;
            _roles = roles;
            _superAdminDefaultOptions = superAdminDefaultOptions.Value;
            _applicationInfo = applicationInfo.Value;
            _iAccount = iAccount;
            _iCommon = iCommon;
            _iDBOperation = iDBOperation;
        }

        public async Task SendEmailBySendGridAsync(string apiKey,
            string fromEmail,
            string fromFullName,
            string subject,
            string message,
            string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromFullName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email, email));
            await client.SendEmailAsync(msg);

        }

        public async Task SendEmailByGmailAsync(string fromEmail,
            string fromFullName,
            string subject,
            string messageBody,
            string toEmail,
            string toFullName,
            string smtpUser,
            string smtpPassword,
            string smtpHost,
            int smtpPort,
            bool smtpSSL)
        {
            var body = messageBody;
            var message = new MailMessage();
            message.To.Add(new MailAddress(toEmail, toFullName));
            message.From = new MailAddress(fromEmail, fromFullName);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                smtp.UseDefaultCredentials = false;
                var credential = new NetworkCredential
                {
                    UserName = smtpUser,
                    Password = smtpPassword
                };
                smtp.Credentials = credential;
                smtp.Host = smtpHost;
                smtp.Port = smtpPort;
                smtp.EnableSsl = smtpSSL;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                await smtp.SendMailAsync(message);

            }

        }
        public async Task InitAppData()
        {
            var _GetDocumentCategoriesList = _SeedData.GetDocumentCategoriesList();
            foreach (var item in _GetDocumentCategoriesList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.DocumentCategories.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetDocumentStatusList = _SeedData.GetDocumentStatusList();
            foreach (var item in _GetDocumentStatusList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.DocumentStatus.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetEmailConfigList = _SeedData.GetEmailConfigList();
            foreach (var item in _GetEmailConfigList)
            {
                item.SenderFullName = "Admin: Document Management System";
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.EmailConfig.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetManageRoleList = _SeedData.GetManageRoleList();
            foreach (var item in _GetManageRoleList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.ManageUserRoles.Add(item);
                await _context.SaveChangesAsync();
            }

            var _GetCompanyInfo = _SeedData.GetCompanyInfo();
            _GetCompanyInfo.CreatedDate = DateTime.Now;
            _GetCompanyInfo.ModifiedDate = DateTime.Now;
            _GetCompanyInfo.CreatedBy = "Admin";
            _GetCompanyInfo.ModifiedBy = "Admin";
            _context.CompanyInfo.Add(_GetCompanyInfo);
            await _context.SaveChangesAsync();
        }

        public async Task GenerateUserUserRole()
        {
            var _ManageRole = await _context.ManageUserRoles.ToListAsync();
            var _GetRoleList = await _roles.GetRoleList();

            foreach (var role in _ManageRole)
            {
                foreach (var item in _GetRoleList)
                {
                    ManageUserRolesDetails _ManageRoleDetails = new();
                    _ManageRoleDetails.ManageRoleId = role.Id;
                    _ManageRoleDetails.RoleId = item.RoleId;
                    _ManageRoleDetails.RoleName = item.RoleName;

                    if (role.Id == 1)
                    {
                        _ManageRoleDetails.IsAllowed = true;
                    }
                    else if (role.Id == 2 && item.RoleName == "User Profile" || item.RoleName == "Dashboard" || item.RoleName == "Document General")
                    {
                        _ManageRoleDetails.IsAllowed = true;
                    }
                    else
                    {
                        _ManageRoleDetails.IsAllowed = false;
                    }

                    _ManageRoleDetails.CreatedDate = DateTime.Now;
                    _ManageRoleDetails.ModifiedDate = DateTime.Now;
                    _ManageRoleDetails.CreatedBy = "Admin";
                    _ManageRoleDetails.ModifiedBy = "Admin";
                    _context.Add(_ManageRoleDetails);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task CreateDocument()
        {
            try
            {
                var _GetDocumentList = _SeedData.GetDocumentList();
                var _GetDocumentFileList = _SeedData.GetDocumentFileList().ToList();
                int count = 0;
                foreach (var item in _GetDocumentList)
                {
                    item.FilesDirName = "DefaultDoc";
                    item.FilesPath = _GetDocumentFileList[count].FilePath;
                    item.DocumentStatus = 1;
                    item.Notes = "TBD";
                    item.CreatedDate = DateTime.Now;
                    item.ModifiedDate = DateTime.Now;
                    item.CreatedBy = "Admin";
                    item.ModifiedBy = "Admin";
                    _context.Document.Add(item);
                    _context.SaveChanges();

                    //Add DocumentFile
                    DocumentFile _DocumentFile = new();
                    _DocumentFile = _GetDocumentFileList[count];
                    _DocumentFile.DocumentId = item.Id;
                    _DocumentFile.Name = item.Name;
                    _DocumentFile.Length = 10;
                    _DocumentFile.CreatedDate = DateTime.Now;
                    _DocumentFile.ModifiedDate = DateTime.Now;
                    _DocumentFile.CreatedBy = "Admin";
                    _DocumentFile.ModifiedBy = "Admin";
                    _context.DocumentFile.Add(_DocumentFile);
                    _context.SaveChanges();

                    List<DocumentFile> listDocumentFile = new();
                    listDocumentFile.Add(_DocumentFile);
                    if (_iCommon.IsAllowedFileExtension(_DocumentFile.FilePath))
                    {
                        var result2 = await _iDBOperation.AddDocumentFileContent(listDocumentFile, "Admin");
                    }
                    

                    var _GetCommentList = _SeedData.GetCommentList();
                    foreach (var _Comment in _GetCommentList)
                    {
                        _Comment.DocumentId = item.Id;
                        _Comment.CreatedDate = DateTime.Now;
                        _Comment.ModifiedDate = DateTime.Now;
                        _Comment.CreatedBy = "Admin";
                        _Comment.ModifiedBy = "Admin";
                        _context.Comment.Add(_Comment);
                        _context.SaveChanges();
                    }

                    DocumentHistoryCRUDViewModel _DocumentHistoryCRUDViewModel = new()
                    {
                        DocumentId = item.Id,
                        AssignEmployeeId = 0,
                        Action = "Document Created.",
                        UserName = "Admin"
                    };
                    await _iCommon.AddDocumentHistory(_DocumentHistoryCRUDViewModel);
                    count++;
                }
            }
            catch (Exception) { throw; }
        }
        public async Task CreateDefaultSuperAdmin()
        {
            try
            {
                await _roles.GenerateRolesFromPageList();

                ApplicationUser superAdmin = new ApplicationUser();
                superAdmin.Email = _superAdminDefaultOptions.Email;
                superAdmin.UserName = superAdmin.Email;
                superAdmin.EmailConfirmed = true;

                var result = await _userManager.CreateAsync(superAdmin, _superAdminDefaultOptions.Password);

                if (result.Succeeded)
                {
                    //add to user profile
                    UserProfile profile = new();
                    profile.ApplicationUserId = superAdmin.Id;
                    profile.RoleId = 1;

                    profile.FirstName = "Super";
                    profile.LastName = "Admin";
                    profile.PhoneNumber = "+8801674411603";
                    profile.Email = superAdmin.Email;
                    profile.Address = "R/A, Dhaka";
                    profile.Country = "Bangladesh";
                    profile.ProfilePicture = "/images/UserIcon/Admin.png";

                    profile.CreatedDate = DateTime.Now;
                    profile.ModifiedDate = DateTime.Now;
                    profile.CreatedBy = "Admin";
                    profile.ModifiedBy = "Admin";

                    await _context.UserProfile.AddAsync(profile);
                    await _context.SaveChangesAsync();
                    await _roles.AddToRoles(superAdmin);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task CreateDefaultOtherUser()
        {
            var _GetUserProfileList = _SeedData.GetUserProfileList();
            foreach (var item in _GetUserProfileList)
            {
                item.RoleId = 2;
                var _ApplicationUser = await _iAccount.CreateUserProfile(item, "Admin");
            }
        }
        public async Task<string> UploadFile(List<IFormFile> files, IWebHostEnvironment env, string uploadFolder)
        {
            var result = "";

            var webRoot = env.WebRootPath;
            var uploads = Path.Combine(webRoot, uploadFolder);
            var extension = "";
            var filePath = "";
            var fileName = "";


            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    extension = Path.GetExtension(formFile.FileName);
                    fileName = Guid.NewGuid().ToString() + extension;
                    filePath = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    result = fileName;

                }
            }

            return result;
        }
        public async Task CreateDefaultEmailSettings()
        {
            //SMTP
            var CountSMTPEmailSetting = _context.SMTPEmailSetting.Count();
            if (CountSMTPEmailSetting < 1)
            {
                SMTPEmailSetting _SMTPEmailSetting = new SMTPEmailSetting
                {
                    UserName = "devmlbd@gmail.com",
                    Password = "",
                    Host = "smtp.gmail.com",
                    Port = 587,
                    IsSSL = true,
                    FromEmail = "devmlbd@gmail.com",
                    FromFullName = "Web Admin Notification",
                    IsDefault = true,

                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = "Admin",
                    ModifiedBy = "Admin",
                };
                _context.Add(_SMTPEmailSetting);
                await _context.SaveChangesAsync();
            }
            //SendGridOptions
            var CountSendGridSetting = _context.SendGridSetting.Count();
            if (CountSendGridSetting < 1)
            {
                SendGridSetting _SendGridOptions = new SendGridSetting
                {
                    SendGridUser = "",
                    SendGridKey = "",
                    FromEmail = "devmlbd@gmail.com",
                    FromFullName = "Web Admin Notification",
                    IsDefault = false,

                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = "Admin",
                    ModifiedBy = "Admin",
                };
                _context.Add(_SendGridOptions);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<SharedUIDataViewModel> GetSharedUIData(ClaimsPrincipal _ClaimsPrincipal)
        {
            SharedUIDataViewModel _SharedUIDataViewModel = new();
            ApplicationUser _ApplicationUser = await _userManager.GetUserAsync(_ClaimsPrincipal);
            _SharedUIDataViewModel.UserProfile = _context.UserProfile.SingleOrDefault(x => x.ApplicationUserId.Equals(_ApplicationUser.Id));
            _SharedUIDataViewModel.MainMenuViewModel = await _roles.RolebaseMenuLoad(_ApplicationUser);
            _SharedUIDataViewModel.ApplicationInfo = _applicationInfo;
            return _SharedUIDataViewModel;
        }
        public async Task<DefaultIdentityOptions> GetDefaultIdentitySettings()
        {
            return await _context.DefaultIdentityOptions.Where(x => x.Id == 1).SingleOrDefaultAsync();
        }
        public async Task CreateDefaultIdentitySettings()
        {
            if (_context.DefaultIdentityOptions.Count() < 1)
            {
                DefaultIdentityOptions _DefaultIdentityOptions = new DefaultIdentityOptions
                {
                    PasswordRequireDigit = false,
                    PasswordRequiredLength = 3,
                    PasswordRequireNonAlphanumeric = false,
                    PasswordRequireUppercase = false,
                    PasswordRequireLowercase = false,
                    PasswordRequiredUniqueChars = 0,
                    LockoutDefaultLockoutTimeSpanInMinutes = 30,
                    LockoutMaxFailedAccessAttempts = 5,
                    LockoutAllowedForNewUsers = false,
                    UserRequireUniqueEmail = true,
                    SignInRequireConfirmedEmail = false,

                    CookieHttpOnly = true,
                    CookieExpiration = 150,
                    CookieExpireTimeSpan = 120,
                    LoginPath = "/Account/Login",
                    LogoutPath = "/Account/Logout",
                    AccessDeniedPath = "/Account/AccessDenied",
                    SlidingExpiration = true,

                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = "Admin",
                    ModifiedBy = "Admin",
                };
                _context.Add(_DefaultIdentityOptions);
                await _context.SaveChangesAsync();
            }
        }
    }
}
