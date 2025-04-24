using System.Collections.Generic;
using LoginVictor.Entidades;
using Microsoft.EntityFrameworkCore;

namespace LoginVictor.Datos
{

    public class ApplicationDbContext : DbContext // Clase a partir de la cual configuro las tablas de mi BD
    {
        // Para realizar configuraciones de EntityFrameworkCore fuera de esta clase
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        // Para indicar que quiero que se cree una tabla en mi BD a partir de las propiedades de la clase Cliente
        public DbSet<Cliente> Clientes { get; set; }

        // Para indicar que quiero que se cree una tabla en mi BD a partir de las propiedades de la clase Recibo
        public DbSet<Recibo> Recibos { get; set; }
    }
}
