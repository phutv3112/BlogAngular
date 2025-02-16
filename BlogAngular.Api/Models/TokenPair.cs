using System.ComponentModel.DataAnnotations;

namespace BlogAngular.Api.Models
{
    public class TokenPair
    {
        public string AccessToken { get; set; }
        [StringLength(36)]
        public string RefreshToken { get; set; }
    }
}
