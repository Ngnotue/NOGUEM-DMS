using System.ComponentModel.DataAnnotations;

namespace DocumentMS.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
