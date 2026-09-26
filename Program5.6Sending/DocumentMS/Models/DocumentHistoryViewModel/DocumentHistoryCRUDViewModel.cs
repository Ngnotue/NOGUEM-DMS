using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentMS.Models.DocumentHistoryViewModel
{
    public class DocumentHistoryCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        public Int64 DocumentId { get; set; }
        public Int64? AssignEmployeeId { get; set; }
        public string AssignEmployeeDisplay { get; set; }
        public string Action { get; set; }
        public string Note { get; set; }
        public string UserName { get; set; }
        public string CreatedDateDisplay { get; set; }


        public static implicit operator DocumentHistoryCRUDViewModel(DocumentHistory _DocumentHistory)
        {
            return new DocumentHistoryCRUDViewModel
            {
                Id = _DocumentHistory.Id,
                DocumentId = _DocumentHistory.DocumentId,
                AssignEmployeeId = _DocumentHistory.AssignEmployeeId,
                Action = _DocumentHistory.Action,
                Note = _DocumentHistory.Note,
                CreatedDate = _DocumentHistory.CreatedDate,
                ModifiedDate = _DocumentHistory.ModifiedDate,
                CreatedBy = _DocumentHistory.CreatedBy,
                ModifiedBy = _DocumentHistory.ModifiedBy,
                Cancelled = _DocumentHistory.Cancelled,
            };
        }

        public static implicit operator DocumentHistory(DocumentHistoryCRUDViewModel vm)
        {
            return new DocumentHistory
            {
                Id = vm.Id,
                DocumentId = vm.DocumentId,
                AssignEmployeeId = vm.AssignEmployeeId,
                Action = vm.Action,
                Note = vm.Note,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
