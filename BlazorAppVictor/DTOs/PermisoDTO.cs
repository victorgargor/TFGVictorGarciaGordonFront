namespace EjerciciosVictorAPI.DTOs
{
    public class PermisoDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); 
        public string Nombre { get; set; } = null!;
    }
}
