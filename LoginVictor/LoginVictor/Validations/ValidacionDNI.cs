using System.ComponentModel.DataAnnotations;

namespace LoginVictor.Validations
{
    /// <summary>
    /// Atributo de validación personalizado para el DNI.
    /// Valida que el DNI tenga 8 dígitos seguidos de una letra correcta.
    /// </summary>
    public class ValidacionDNI : ValidationAttribute
    {
        /// <summary>
        /// Cadena de letras según la tabla de correspondencia, donde el índice representa el resto.
        /// </summary>
        private const string CadenaLetras = "TRWAGMYFPDXBNJZSQVHLCKE";

        /// <summary>
        /// Valida que el valor sea un DNI correcto.
        /// </summary>
        /// <param name="value">El valor a validar.</param>
        /// <returns>True si el DNI es válido; de lo contrario, false.</returns>
        public override bool IsValid(object value)
        {
            if (value is string dni)
            {
                // El DNI tiene que tener exactamente 9 carácteres (8 dígitos y una letra).
                if (dni.Length != 9)
                {
                    return false;
                }

                // Separo la parte numérica de la letra
                string parteNumerica = dni.Substring(0, 8);
                char parteLetra = dni[8];

                // Valido que la parteNumerica tenga solo números
                if (!int.TryParse(parteNumerica, out int numero))
                {
                    return false;
                }

                // Calculo el resto de dividir el número entre 23 y dependiendo del resto asocio la letra
                int resto = numero % 23;
                char letraCorrecta = CadenaLetras[resto];

                // Comparo la letra que le paso en mayúsculas con la letra que tiene que ser a ver si es correcta
                return char.ToUpper(parteLetra) == letraCorrecta;
            }
            return false;
        }

        /// <summary>
        /// Mensaje de error personalizado para la validación del DNI.
        /// </summary>
        /// <param name="name">El nombre del campo.</param>
        /// <returns>El mensaje de error.</returns>
        public override string FormatErrorMessage(string name)
        {
            return "El DNI introducido no tiene el formato válido (8 dígitos seguidos de una letra correcta) o no es un DNI";
        }
    }
}
