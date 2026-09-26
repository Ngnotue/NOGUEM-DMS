using DocumentMS.Data;
using DocumentMS.Models;
using DocumentMS.Models.CommonViewModel;
using DocumentMS.Models.DocumentHistoryViewModel;
using DocumentMS.Models.DocumentViewModel;
using DocumentMS.Models.ManageUserRolesVM;
using DocumentMS.Models.UserProfileViewModel;
using Microsoft.EntityFrameworkCore;
using System.Net;
using UAParser;

namespace DocumentMS.Services
{
    public class Common : ICommon
    {
        private readonly IWebHostEnvironment _iHostingEnvironment;
        private readonly ApplicationDbContext _context;
        public Common(IWebHostEnvironment iHostingEnvironment,
            ApplicationDbContext context)
        {
            _iHostingEnvironment = iHostingEnvironment;
            _context = context;
        }
        public string UploadedFile(IFormFile _IFormFile)
        {
            string FileName = null;
            if (_IFormFile != null)
            {
                string _FileServerDir = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload");

                if (_IFormFile.FileName == null)
                    FileName = Guid.NewGuid().ToString() + "_" + "blank-person.png";
                else
                    FileName = Guid.NewGuid().ToString() + "_" + _IFormFile.FileName;

                string filePath = Path.Combine(_FileServerDir, FileName);
                using (var _FileStream = new FileStream(filePath, FileMode.Create))
                {
                    _IFormFile.CopyTo(_FileStream);
                }
            }
            return FileName;
        }
        public string UploadedFile(IFormFile _IFormFile, string CreatedDirName)
        {
            string FileName = null;
            if (_IFormFile != null)
            {
                string _FileServerDir = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload/" + CreatedDirName);
                if (_IFormFile.FileName == null)
                    FileName = Guid.NewGuid().ToString() + "_" + "blank-person.png";
                else
                    FileName = Guid.NewGuid().ToString() + "_" + _IFormFile.FileName;

                string filePath = Path.Combine(_FileServerDir, FileName);
                using (var _FileStream = new FileStream(filePath, FileMode.Create))
                {
                    _IFormFile.CopyTo(_FileStream);
                }
            }
            return FileName;
        }
        public string GetServerFileDir()
        {
            string _Path = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload");
            return _Path;
        }
        public string GetWWWRootPath()
        {
            string _wwwrootDir = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot");
            return _wwwrootDir;
        }
        public string GetContentPath(string DBFilePath)
        {
            string _FileServerDir = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot");
            var _CombinePath = _FileServerDir + DBFilePath;
            return _CombinePath;
        }

        public async Task<SMTPEmailSetting> GetSMTPEmailSetting()
        {
            return await _context.Set<SMTPEmailSetting>().Where(x => x.Id == 1).SingleOrDefaultAsync();
        }
        public async Task<SendGridSetting> GetSendGridEmailSetting()
        {
            return await _context.Set<SendGridSetting>().Where(x => x.Id == 1).SingleOrDefaultAsync();
        }

        public UserProfile GetByUserProfile(Int64 id)
        {
            return _context.UserProfile.Where(x => x.UserProfileId == id).SingleOrDefault();
        }
        public UserProfileCRUDViewModel GetByUserProfileInfo(Int64 id)
        {
            UserProfileCRUDViewModel _UserProfileCRUDViewModel = _context.UserProfile.Where(x => x.UserProfileId == id).SingleOrDefault();
            return _UserProfileCRUDViewModel;
        }
        public bool InsertLoginHistory(LoginHistory _LoginHistory, ClientInfo _ClientInfo)
        {
            try
            {
                _LoginHistory.PublicIP = GetPublicIP();
                _LoginHistory.CreatedDate = DateTime.Now;
                _LoginHistory.ModifiedDate = DateTime.Now;

                _context.Add(_LoginHistory);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetPublicIP()
        {
            try
            {
                string url = "http://checkip.dyndns.org/";
                WebRequest req = WebRequest.Create(url);
                WebResponse resp = req.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                string response = sr.ReadToEnd().Trim();
                string[] a = response.Split(':');
                string a2 = a[1].Substring(1);
                string[] a3 = a2.Split('<');
                string a4 = a3[0];
                return a4;
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw new Exception("No network adapters with an IPv4 address in the system!");
            }
        }
        public IQueryable<DocumentCRUDViewModel> GetDocumentGridItem()
        {
            try
            {
                return (from _Document in _context.Document
                        join _DocumentCategories in _context.DocumentCategories on _Document.CategoriesId equals _DocumentCategories.Id
                        join _DocumentStatus in _context.DocumentStatus on _Document.DocumentStatus equals _DocumentStatus.Id
                        into listDocumentStatus
                        from _DocumentStatus in listDocumentStatus.DefaultIfEmpty()
                        join _UserProfile in _context.UserProfile on _Document.AssignEmployeeId equals _UserProfile.UserProfileId
                        into listEmployee
                        from _UserProfile in listEmployee.DefaultIfEmpty()
                        where _Document.Cancelled == false
                        select new DocumentCRUDViewModel
                        {
                            Id = _Document.Id,
                            Name = _Document.Name,
                            CategoriesId = _Document.CategoriesId,
                            CategoriesDisplay = _DocumentCategories.Name,
                            AssignEmployeeId = _Document.AssignEmployeeId,
                            AssignEmployeeDisplay = _Document.AssignEmployeeId == 0 ? "Unassigned" : _UserProfile.FirstName + " " + _UserProfile.LastName,
                            DocumentStatus = _Document.DocumentStatus,
                            DocumentStatusDisplay = _Document.DocumentStatus == 0 ? "New" : _DocumentStatus.Name,
                            FilesDirName = _Document.FilesDirName,
                            Tag01 = _Document.Tag01,
                            Tag02 = _Document.Tag02,
                            Tag03 = _Document.Tag03,
                            Tag04 = _Document.Tag04,
                            Tag05 = _Document.Tag05,

                            CreatedDate = _Document.CreatedDate
                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception) { throw; }
        }
        public IQueryable<DocumentCRUDViewModel> GetDocumentList()
        {
            try
            {
                return (from _Document in _context.Document
                        join _DocumentCategories in _context.DocumentCategories on _Document.CategoriesId equals _DocumentCategories.Id
                        join _DocumentStatus in _context.DocumentStatus on _Document.DocumentStatus equals _DocumentStatus.Id
                        into listDocumentStatus
                        from _DocumentStatus in listDocumentStatus.DefaultIfEmpty()
                        join _UserProfile in _context.UserProfile on _Document.AssignEmployeeId equals _UserProfile.UserProfileId
                        into listEmployee
                        from _UserProfile in listEmployee.DefaultIfEmpty()
                        where _Document.Cancelled == false
                        select new DocumentCRUDViewModel
                        {
                            Id = _Document.Id,
                            Name = _Document.Name,
                            CategoriesId = _Document.CategoriesId,
                            CategoriesDisplay = _DocumentCategories.Name,
                            Notes = _Document.Notes,
                            AssignEmployeeId = _Document.AssignEmployeeId,
                            AssignEmployeeDisplay = _Document.AssignEmployeeId == 0 ? "Unassigned" : _UserProfile.FirstName + " " + _UserProfile.LastName,
                            DocumentStatus = _Document.DocumentStatus,
                            DocumentStatusDisplay = _Document.DocumentStatus == 0 ? "New" : _DocumentStatus.Name,
                            IsFileSaveInDB = _Document.IsFileSaveInDB,
                            FilesDirName = _Document.FilesDirName,
                            FilesPath = _Document.FilesPath,
                            Tag01 = _Document.Tag01,
                            Tag02 = _Document.Tag02,
                            Tag03 = _Document.Tag03,
                            Tag04 = _Document.Tag04,
                            Tag05 = _Document.Tag05,

                            CreatedDate = _Document.CreatedDate,
                            ModifiedDate = _Document.ModifiedDate,
                            CreatedBy = _Document.CreatedBy,
                            ModifiedBy = _Document.ModifiedBy,
                            // Add shared user information
                            listUsersAllowedCRUDViewModel = _Document.SharedUsers.ToList()
                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception) { throw; }
        }
        public IQueryable<DocumentFileCRUDViewModel> GetDocumentFileList()
        {
            try
            {
                var result = (from _DocumentFile in _context.DocumentFile
                              where _DocumentFile.Cancelled == false
                              select new DocumentFileCRUDViewModel
                              {
                                  Id = _DocumentFile.Id,
                                  DocumentId = _DocumentFile.DocumentId,
                                  Name = _DocumentFile.Name,
                                  FilePath = _DocumentFile.FilePath,
                                  ContentType = _DocumentFile.ContentType,
                                  Length = _DocumentFile.Length,
                                  IsFileSaveInDB = _DocumentFile.IsFileSaveInDB,
                                  CreatedDate = _DocumentFile.CreatedDate,
                                  CreatedBy = _DocumentFile.CreatedBy,
                              }).OrderByDescending(x => x.Id);
                return result;
            }
            catch (Exception) { throw; }
        }
        public Tuple<byte[], string> GetDownloadDetails(Int64 id)
        {
            byte[] bytes = null;
            try
            {
                var _DocumentFile = _context.DocumentFile.Where(x => x.Id == id).SingleOrDefault();
                string _WebRootPath = _iHostingEnvironment.WebRootPath + _DocumentFile.FilePath;
                using (var _MemoryStream = new MemoryStream())
                {
                    using (FileStream file = new FileStream(_WebRootPath, FileMode.Open, FileAccess.Read))
                        file.CopyTo(_MemoryStream);
                    bytes = _MemoryStream.ToArray();
                }
                string _GetExtension = Path.GetExtension(_DocumentFile.FilePath);
                var _Tuple = new Tuple<byte[], string>(bytes, _DocumentFile.Name + "." + _GetExtension);
                return _Tuple;
            }
            catch (Exception) { throw; }
        }
        public IQueryable<DocumentHistoryCRUDViewModel> GetDocumentHistoryList()
        {
            try
            {
                var result = (from _DocumentHistory in _context.DocumentHistory
                              join _UserProfile in _context.UserProfile on _DocumentHistory.AssignEmployeeId equals _UserProfile.UserProfileId
                              into listEmployee
                              from _UserProfile in listEmployee.DefaultIfEmpty()
                              where _DocumentHistory.Cancelled == false
                              select new DocumentHistoryCRUDViewModel
                              {
                                  Id = _DocumentHistory.Id,
                                  DocumentId = _DocumentHistory.DocumentId,
                                  Action = _DocumentHistory.Action,
                                  AssignEmployeeId = _DocumentHistory.AssignEmployeeId,
                                  AssignEmployeeDisplay = _DocumentHistory.AssignEmployeeId == 0 ? "Unassigned" : _UserProfile.FirstName + " " + _UserProfile.LastName,
                                  Note = _DocumentHistory.Note,
                                  CreatedDate = _DocumentHistory.CreatedDate,
                                  CreatedDateDisplay = String.Format("{0:f}", _DocumentHistory.CreatedDate),
                              }).OrderByDescending(x => x.Id);
                return result;
            }
            catch (Exception) { throw; }
        }
        public async Task<DocumentHistory> AddDocumentHistory(DocumentHistoryCRUDViewModel vm)
        {
            try
            {
                DocumentHistory _AssetHistory = new();
                _AssetHistory = vm;
                _AssetHistory.CreatedDate = DateTime.Now;
                _AssetHistory.ModifiedDate = DateTime.Now;
                _AssetHistory.CreatedBy = vm.UserName;
                _AssetHistory.ModifiedBy = vm.UserName;
                _context.Add(_AssetHistory);
                await _context.SaveChangesAsync();

                return _AssetHistory;
            }
            catch (Exception) { throw; }
        }
        public DocumentHistory AddDocumentHistoryNoAsync(DocumentHistoryCRUDViewModel vm)
        {
            try
            {
                DocumentHistory _AssetHistory = new();
                _AssetHistory = vm;
                _AssetHistory.CreatedDate = DateTime.Now;
                _AssetHistory.ModifiedDate = DateTime.Now;
                _AssetHistory.CreatedBy = vm.UserName;
                _AssetHistory.ModifiedBy = vm.UserName;
                _context.Add(_AssetHistory);
                _context.SaveChanges();

                return _AssetHistory;
            }
            catch (Exception) { throw; }
        }

        public IQueryable<ItemDropdownListViewModel> GetddlEmployee()
        {
            return (from tblObj in _context.UserProfile.Where(x => x.Cancelled == false)
                    select new ItemDropdownListViewModel
                    {
                        Id = tblObj.UserProfileId,
                        Name = tblObj.FirstName + " " + tblObj.LastName,
                    }).OrderByDescending(x => x.Id);
        }
        public IQueryable<ItemDropdownListViewModel> GetddlEmailConfig()
        {
            return (from tblObj in _context.EmailConfig.Where(x => x.Cancelled == false)
                    select new ItemDropdownListViewModel
                    {
                        Id = tblObj.Id,
                        Name = tblObj.Email
                    }).OrderByDescending(x => x.Id);
        }
        public IQueryable<ItemDropdownListViewModel> GetddlUserEmail()
        {
            return (from tblObj in _context.UserProfile.Where(x => x.Cancelled == false)
                    select new ItemDropdownListViewModel
                    {
                        Id = tblObj.UserProfileId,
                        Name = tblObj.Email
                    }).OrderByDescending(x => x.Id);
        }
        public IQueryable<TEntity> DropdownData<TEntity>() where TEntity : class
        {
            var _DbSet = _context.Set<TEntity>();
            return _DbSet;
        }

        public Stream CopyStream(Stream _Stream, string _DestinationPath)
        {
            try
            {
                using var _FileStream = new FileStream(_DestinationPath, FileMode.Create, FileAccess.Write);
                _Stream.CopyTo(_FileStream);
                return _Stream;
            }
            catch (Exception) { throw; }

        }
        public async Task<List<ManageUserRolesViewModel>> GetManageRoleDetailsList(Int64 id)
        {
            var result = await (from tblObj in _context.ManageUserRolesDetails.Where(x => x.ManageRoleId == id)
                                select new Models.ManageUserRolesVM.ManageUserRolesViewModel
                                {
                                    ManageRoleDetailsId = tblObj.Id,
                                    RoleId = tblObj.RoleId,
                                    RoleName = tblObj.RoleName,
                                    IsAllowed = tblObj.IsAllowed,
                                }).OrderBy(x => x.RoleName).ToListAsync();
            return result;
        }
        public IQueryable<UserProfileCRUDViewModel> GetUserProfileDetails()
        {
            var result = (from vm in _context.UserProfile
                          join _ManageRole in _context.ManageUserRoles on vm.RoleId equals _ManageRole.Id
                          into _ManageRole
                          from objManageRole in _ManageRole.DefaultIfEmpty()
                          where vm.Cancelled == false
                          select new UserProfileCRUDViewModel
                          {
                              UserProfileId = vm.UserProfileId,
                              ApplicationUserId = vm.ApplicationUserId,
                              FirstName = vm.FirstName,
                              LastName = vm.LastName,
                              PhoneNumber = vm.PhoneNumber,
                              Email = vm.Email,
                              Address = vm.Address,
                              Country = vm.Country,
                              ProfilePicture = vm.ProfilePicture,
                              RoleId = vm.RoleId,
                              RoleIdDisplay = objManageRole.Name,
                              CreatedDate = vm.CreatedDate,
                              ModifiedDate = vm.ModifiedDate,
                              CreatedBy = vm.CreatedBy,
                              ModifiedBy = vm.ModifiedBy,
                              Cancelled = vm.Cancelled,
                          }).OrderByDescending(x => x.UserProfileId);
            return result;
        }
        public IEnumerable<T> GetTableData<T>(ApplicationDbContext dbContext) where T : class
        {
            return dbContext.Set<T>();
        }
        public bool IsAllowedFileExtension(string FilePath)
        {
            string[] _AllowedExtension = { ".docx", ".doc", ".pdf", ".txt" };
            var _GetWWWRootPath = GetWWWRootPath() + "/";
            var _FilePath = Path.Combine(_GetWWWRootPath + FilePath);
            string DBFileExtension = Path.GetExtension(_FilePath);
            for (int i = 0; i < _AllowedExtension.Count(); i++)
            {
                if (_AllowedExtension[i] == DBFileExtension)
                    return true;
            }
            return false;
        }
    }
}