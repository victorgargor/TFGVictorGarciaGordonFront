using Microsoft.AspNetCore.Identity;

namespace EjerciciosVictorAPI.Entidades
{
    public class RolPermiso
    {
        public string RolId { get; set; } = null!;
        public IdentityRole Rol { get; set; } = null!;

        public Guid PermisoId { get; set; }
        public Permiso Permiso { get; set; } = null!;
    }
}
