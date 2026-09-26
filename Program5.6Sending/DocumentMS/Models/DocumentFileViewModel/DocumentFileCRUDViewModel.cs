using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentMS.Models.DocumentViewModel
{
    public class DocumentFileCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        public Int64 DocumentId { get; set; }
        public string Name { get; set; }
        [Display(Name = "File Path")]
        public string FilePath { get; set; }
        [Display(Name = "Content Type")]
        public string ContentType { get; set; }
        public double Length { get; set; }
        [Display(Name = "Is File Save In DB")]
        public bool IsFileSaveInDB { get; set; }
        public byte[] DocByte { get; set; }
        
        public static implicit operator DocumentFileCRUDViewModel(DocumentFile _DocumentFile)
        {
            return new DocumentFileCRUDViewModel
            {
                Id = _DocumentFile.Id,
                DocumentId = _DocumentFile.DocumentId,
                Name = _DocumentFile.Name,
                FilePath = _DocumentFile.FilePath,
                ContentType = _DocumentFile.ContentType,
                Length = _DocumentFile.Length,
                IsFileSaveInDB = _DocumentFile.IsFileSaveInDB,
                DocByte = _DocumentFile.DocByte,
                
                CreatedDate = _DocumentFile.CreatedDate,
                ModifiedDate = _DocumentFile.ModifiedDate,
                CreatedBy = _DocumentFile.CreatedBy,
                ModifiedBy = _DocumentFile.ModifiedBy,
                Cancelled = _DocumentFile.Cancelled,
            };
        }
        public static implicit operator DocumentFile(DocumentFileCRUDViewModel vm)
        {
            return new DocumentFile
            {
                Id = vm.Id,
                DocumentId = vm.DocumentId,
                Name = vm.Name,
                FilePath = vm.FilePath,
                ContentType = vm.ContentType,
                Length = vm.Length,
                IsFileSaveInDB = vm.IsFileSaveInDB,
                DocByte = vm.DocByte,
                
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
