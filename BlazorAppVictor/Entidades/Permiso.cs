using System.ComponentModel.DataAnnotations;

namespace EjerciciosVictorAPI.Entidades
{
    public class Permiso
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Nombre { get; set; } = null!;

        public List<RolPermiso> RolPermisos { get; set; } = new();
    }
}
