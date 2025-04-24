using System.ComponentModel.DataAnnotations;

namespace BlazorAppVictor.DTOs
{
    public class UserInfoDTO
    {
        public string Nombre { get; set; } = null!;
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
