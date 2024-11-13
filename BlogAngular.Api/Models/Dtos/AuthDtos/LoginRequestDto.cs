using System.ComponentModel.DataAnnotations;

namespace BlogAngular.Api.Models.Dtos.AuthDtos
{
    public class LoginRequestDto
    {
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
