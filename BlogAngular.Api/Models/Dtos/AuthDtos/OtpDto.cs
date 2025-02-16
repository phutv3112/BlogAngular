using System.ComponentModel.DataAnnotations;

namespace BlogAngular.Api.Models.Dtos.AuthDtos
{
    public class OtpDto
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; } = default!;
        [StringLength(8)]
        [Required]
        public string Otp { get; set; } = default!;
    }
}
