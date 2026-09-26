using System.ComponentModel.DataAnnotations;

namespace DocumentMS.JWTConfiguration.DTOs.Requests
{
    public class TokenRequest
    {
        [Required]
        public string Token { get; set; }

         [Required]
        public string RefreshToken { get; set; }
    }
}