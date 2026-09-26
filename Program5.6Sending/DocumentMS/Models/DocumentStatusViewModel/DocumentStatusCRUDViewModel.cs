using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentMS.Models.DocumentStatusViewModel
{
    public class DocumentStatusCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public static implicit operator DocumentStatusCRUDViewModel(DocumentStatus _DocumentStatus)
        {
            return new DocumentStatusCRUDViewModel
            {
                Id = _DocumentStatus.Id,
                Name = _DocumentStatus.Name,
                Description = _DocumentStatus.Description,
                CreatedDate = _DocumentStatus.CreatedDate,
                ModifiedDate = _DocumentStatus.ModifiedDate,
                CreatedBy = _DocumentStatus.CreatedBy,
                ModifiedBy = _DocumentStatus.ModifiedBy,
                Cancelled = _DocumentStatus.Cancelled,
            };
        }

        public static implicit operator DocumentStatus(DocumentStatusCRUDViewModel vm)
        {
            return new DocumentStatus
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
