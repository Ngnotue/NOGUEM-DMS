using DocumentMS.Data;
using DocumentMS.Models;
using DocumentMS.Models.CommonViewModel;
using DocumentMS.Models.DocumentHistoryViewModel;
using DocumentMS.Models.DocumentViewModel;
using DocumentMS.Models.UserProfileViewModel;
using DocumentMS.Models.ManageUserRolesVM;
using UAParser;

namespace DocumentMS.Services
{
    public interface ICommon
    {
        string UploadedFile(IFormFile ProfilePicture);
        string UploadedFile(IFormFile _IFormFile, string CreatedDirName);
        string GetServerFileDir();
        string GetWWWRootPath();
        string GetContentPath(string DBFilePath);
        Task<SMTPEmailSetting> GetSMTPEmailSetting();
        Task<SendGridSetting> GetSendGridEmailSetting();
        UserProfile GetByUserProfile(Int64 id);
        UserProfileCRUDViewModel GetByUserProfileInfo(Int64 id);
        bool InsertLoginHistory(LoginHistory _LoginHistory, ClientInfo _ClientInfo);
        public IQueryable<DocumentCRUDViewModel> GetDocumentGridItem();
        IQueryable<DocumentCRUDViewModel> GetDocumentList();
        IQueryable<DocumentFileCRUDViewModel> GetDocumentFileList();
        Tuple<byte[], string> GetDownloadDetails(Int64 id);
        IQueryable<DocumentHistoryCRUDViewModel> GetDocumentHistoryList();
        Task<DocumentHistory> AddDocumentHistory(DocumentHistoryCRUDViewModel vm);
        DocumentHistory AddDocumentHistoryNoAsync(DocumentHistoryCRUDViewModel vm);
        IQueryable<ItemDropdownListViewModel> GetddlEmployee();
        IQueryable<ItemDropdownListViewModel> GetddlEmailConfig();
        IQueryable<ItemDropdownListViewModel> GetddlUserEmail();
        Stream CopyStream(Stream _Stream, string _DestinationPath);
        IQueryable<TEntity> DropdownData<TEntity>() where TEntity : class;
        Task<List<ManageUserRolesViewModel>> GetManageRoleDetailsList(Int64 id);
        IQueryable<UserProfileCRUDViewModel> GetUserProfileDetails();
        IEnumerable<T> GetTableData<T>(ApplicationDbContext dbContext) where T : class;
        bool IsAllowedFileExtension(string FilePath);
        // Extension

    }
}
