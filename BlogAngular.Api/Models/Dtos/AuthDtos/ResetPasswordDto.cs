using System.ComponentModel.DataAnnotations;

namespace BlogAngular.Api.Models.Dtos.AuthDtos
{
    public class ResetPasswordDto
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
