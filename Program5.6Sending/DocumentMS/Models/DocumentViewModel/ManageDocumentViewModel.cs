using System.Collections.Generic;

namespace DocumentMS.Models.DocumentViewModel
{
    public class ManageDocumentViewModel : EntityBase
    {
        public DocumentCRUDViewModel DocumentCRUDViewModel { get; set; }
        public List<Comment> listComment { get; set; }
    }
}

