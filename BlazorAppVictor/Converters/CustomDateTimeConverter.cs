using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorAppVictor.Converters
{
    /// <summary>
    /// Convertidor JSON personalizado para <see cref="DateTime"/> que utiliza el formato "yyyy/MM/dd HHmmss".
    /// </summary>
    /// <remarks>
    /// Este convertidor permite serializar y deserializar objetos <see cref="DateTime"/> utilizando el formato "yyyy/MM/dd HHmmss".
    /// Se puede aplicar a nivel de propiedad mediante el atributo <see cref="JsonConverterAttribute"/>.
    /// </remarks>
    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        /// <summary>
        /// Formato de fecha utilizado para la serialización y deserialización.
        /// </summary>
        private readonly string _dateFormat;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CustomDateTimeConverter"/> con el formato predeterminado "yyyy/MM/dd HHmmss".
        /// </summary>
        public CustomDateTimeConverter() : this("yyyy/MM/dd HHmmss")
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CustomDateTimeConverter"/> con un formato de fecha especificado.
        /// </summary>
        /// <param name="dateFormat">El formato de fecha a utilizar, por ejemplo, "yyyy/MM/dd HHmmss".</param>
        public CustomDateTimeConverter(string dateFormat)
        {
            _dateFormat = dateFormat;
        }

        /// <summary>
        /// Lee y convierte el valor JSON en un objeto <see cref="DateTime"/> utilizando el formato especificado.
        /// </summary>
        /// <param name="reader">El lector de JSON que contiene el valor a convertir.</param>
        /// <param name="typeToConvert">El tipo de objeto a convertir. Se espera que sea <see cref="DateTime"/>.</param>
        /// <param name="options">Opciones de serialización actuales.</param>
        /// <returns>El objeto <see cref="DateTime"/> convertido.</returns>
        /// <exception cref="JsonException">
        /// Se lanza si el valor JSON no se puede convertir al formato especificado.
        /// </exception>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Obtener la cadena de fecha desde el JSON.
            string? dateString = reader.GetString();

            // Intentar convertir la cadena a DateTime utilizando el formato especificado.
            if (!string.IsNullOrEmpty(dateString) &&
                DateTime.TryParseExact(dateString, _dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                // Especificar que el DateTime es UTC para que PostgreSQL lo acepte.
                return DateTime.SpecifyKind(date, DateTimeKind.Utc);
            }

            // Si la conversión falla, lanzar una excepción indicando el formato esperado.
            throw new JsonException($"Formato de fecha incorrecto. Se espera el formato {_dateFormat}");
        }


        /// <summary>
        /// Escribe un objeto <see cref="DateTime"/> como un valor JSON utilizando el formato especificado.
        /// </summary>
        /// <param name="writer">El escritor de JSON al que se escribirá el valor.</param>
        /// <param name="value">El objeto <see cref="DateTime"/> a convertir en una cadena.</param>
        /// <param name="options">Opciones de serialización actuales.</param>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Convertir el DateTime a cadena utilizando el formato especificado y escribirlo en el JSON.
            writer.WriteStringValue(value.ToString(_dateFormat));
        }
    }
}
