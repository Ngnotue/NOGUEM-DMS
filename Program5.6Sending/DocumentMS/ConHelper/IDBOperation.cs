using DocumentMS.Models.DocumentHistoryViewModel;
using DocumentMS.Models.DocumentViewModel;
using DocumentMS.Models;

namespace DocumentMS.ConHelper
{
    public interface IDBOperation
    {
        Task AddDocumentHistory(AddDocumentHistoryViewModel vm);
        Task<List<DocumentFile>> AddMultipleFile(DocumentCRUDViewModel vm);
        Task<DocumentFile> AddDefaultFile(DocumentCRUDViewModel vm);
        Task<bool> AddDocumentFileContent(List<DocumentFile> listDocumentFile, string UserName);
    }
}
