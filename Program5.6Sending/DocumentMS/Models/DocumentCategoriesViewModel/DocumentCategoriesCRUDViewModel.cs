using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentMS.Models.DocumentCategoriesViewModel
{
    public class DocumentCategoriesCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
                public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }



        public static implicit operator DocumentCategoriesCRUDViewModel(DocumentCategories _DocumentCategories)
        {
            return new DocumentCategoriesCRUDViewModel
            {
                Id=_DocumentCategories.Id,
Name=_DocumentCategories.Name,
Description=_DocumentCategories.Description,
CreatedDate=_DocumentCategories.CreatedDate,
ModifiedDate=_DocumentCategories.ModifiedDate,
CreatedBy=_DocumentCategories.CreatedBy,
ModifiedBy=_DocumentCategories.ModifiedBy,
Cancelled=_DocumentCategories.Cancelled,

            };
        }

        public static implicit operator DocumentCategories(DocumentCategoriesCRUDViewModel vm)
        {
            return new DocumentCategories
            {
                Id=vm.Id,
Name=vm.Name,
Description=vm.Description,
CreatedDate=vm.CreatedDate,
ModifiedDate=vm.ModifiedDate,
CreatedBy=vm.CreatedBy,
ModifiedBy=vm.ModifiedBy,
Cancelled=vm.Cancelled,

            };
        }
    }
}
