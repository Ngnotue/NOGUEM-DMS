using System.ComponentModel.DataAnnotations;

namespace DocumentMS.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
