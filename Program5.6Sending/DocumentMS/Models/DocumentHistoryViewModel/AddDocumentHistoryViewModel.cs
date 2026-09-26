using System;

namespace DocumentMS.Models.DocumentHistoryViewModel
{
    public class AddDocumentHistoryViewModel
    {
        public Int64 DocumentId { get; set; }
        public Int64? AssignEmployeeId { get; set; }
        public string Action { get; set; }
        public string UserName { get; set; }

        public static implicit operator DocumentHistoryCRUDViewModel(AddDocumentHistoryViewModel vm)
        {
            return new DocumentHistoryCRUDViewModel
            {
                DocumentId = vm.DocumentId,
                AssignEmployeeId = vm.AssignEmployeeId,
                Action = vm.Action,
                UserName = vm.UserName
            };
        }

    }
}
