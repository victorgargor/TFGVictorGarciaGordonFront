namespace BlazorAppVictor.DTOs
{
    /// <summary>
    /// DTO que representa los parámetros de paginación utilizados en consultas que retornan múltiples resultados.
    /// </summary>
    public class PaginacionDTO
    {
        /// <summary>
        /// Número de la página solicitada (por defecto es 1).
        /// </summary>
        public int Pagina { get; set; } = 1;

        /// <summary>
        /// Cantidad de registros que se desean obtener por página (por defecto es 5).
        /// </summary>
        public int CantidadRegistros { get; set; } = 5;
    }
}
