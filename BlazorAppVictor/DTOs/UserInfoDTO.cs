using System.ComponentModel.DataAnnotations;

namespace BlazorAppVictor.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class UserInfoDTO
    {
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [RegularExpression(
            @"^(?=.{6,}$)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).*$",
            ErrorMessage = "La contraseña debe tener al menos 6 caracteres, " +
                           "incluyendo una mayúscula, una minúscula y un carácter especial.")]
        public string Password { get; set; }
    }

}
