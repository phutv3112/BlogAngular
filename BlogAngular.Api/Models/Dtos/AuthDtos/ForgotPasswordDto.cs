using System.ComponentModel.DataAnnotations;

namespace BlogAngular.Api.Models.Dtos.AuthDtos
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
