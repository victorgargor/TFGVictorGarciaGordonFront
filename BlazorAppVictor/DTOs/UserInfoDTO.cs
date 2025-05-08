using System.ComponentModel.DataAnnotations;

namespace BlazorAppVictor.DTOs
{
    /// <summary>
    /// DTO que representa la información de un usuario para registro o autenticación.
    /// </summary>
    public class UserInfoDTO
    {
        /// <summary>
        /// Dirección de correo electrónico del usuario.
        /// Debe tener un formato válido y es obligatoria.
        /// </summary>
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string Email { get; set; }

        /// <summary>
        /// Contraseña del usuario.
        /// Debe tener al menos 6 caracteres, una mayúscula, una minúscula y un carácter especial.
        /// </summary>
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [RegularExpression(
            @"^(?=.{6,}$)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).*$",
            ErrorMessage = "La contraseña debe tener al menos 6 caracteres, " +
                           "incluyendo una mayúscula, una minúscula y un carácter especial.")]
        public string Password { get; set; }
    }
}
