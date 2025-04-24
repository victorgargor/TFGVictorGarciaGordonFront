using LoginVictor.Datos;
using LoginVictor.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace LoginVictor.Controllers
{
    /// <summary>
    /// Controlador para la gestión de clientes.
    /// Incluye operaciones para crear, editar, eliminar, obtener y listar clientes.
    /// </summary>
    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : ControllerBase
    {
        /// <summary>
        /// Contexto de la base de datos utilizado para interactuar con la tabla de clientes.
        /// </summary>
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Constructor que inicializa el contexto de la base de datos.
        /// </summary>
        /// <param name="context">El contexto de la base de datos.</param>
        public ClientesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Lista todos los clientes, con filtros opcionales por estado (activos/inactivos) y ordenados por DNI o fecha de alta.
        /// </summary>
        /// <param name="orden">Orden para la lista de clientes, por defecto es "dni".</param>
        /// <param name="activos">Filtra por clientes activos o inactivos. Si es null, no se aplica ningún filtro.</param>
        /// <returns>Una lista de clientes ordenada según los parámetros proporcionados.</returns>
        [HttpGet]
        public async Task<IEnumerable<Cliente>> ListarClientes([FromQuery] string orden = "dni", [FromQuery] bool? activos = null)
        {
            // Inicia una consulta sobre los clientes.
            var query = context.Clientes.AsQueryable();

            // Filtro de clientes activos/inactivos si se pasa el parámetro "activos".
            if (activos.HasValue)
            {
                query = activos.Value
                    ? query.Where(c => c.FechaBaja == null)
                    : query.Where(c => c.FechaBaja != null);
            }

            // Ordena la lista de clientes según el parámetro "orden".
            return orden.ToLower() == "fecha"
                ? await query.OrderBy(c => c.FechaAlta).ToListAsync()
                : await query.OrderBy(c => c.DNI).ToListAsync();
        }

        /// <summary>
        /// Obtiene un cliente específico por su DNI, incluyendo la lista de recibos asociados.
        /// </summary>
        /// <param name="dni">El DNI del cliente que se desea obtener.</param>
        /// <returns>El cliente solicitado con su información, o un código de estado 404 si no se encuentra.</returns>
        [HttpGet("{dni}")]
        public async Task<ActionResult<Cliente>> ObtenerClientePorDNI(string dni)
        {
            // Incluye los recibos relacionados con el cliente, buscando por su DNI.
            var cliente = await context.Clientes.Include(c => c.Recibos)
                                                .FirstOrDefaultAsync(c => c.DNI == dni);

            // Si no lo encuentra devuelvo 404
            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        /// <summary>
        /// Crea un nuevo cliente en la base de datos, validando los datos proporcionados.
        /// </summary>
        /// <param name="cliente">El cliente a crear, con todos los datos necesarios.</param>
        /// <returns>Respuesta 200 OK si se crea correctamente o un error 400 si los datos son inválidos.</returns>
        [HttpPost]
        public async Task<ActionResult> CrearCliente(Cliente cliente)
        {
            // Verifico si ya existe un cliente con el mismo DNI.
            var existe = await context.Clientes.AnyAsync(c => c.DNI == cliente.DNI);
            if (existe)
            {
                return BadRequest("Ya existe un cliente con ese DNI."); // Si ya existe, devuelve un error 400.
            }

            // Valido la cuota máxima según el tipo de cliente (REGISTRADO o SOCIO).
            if (cliente.Tipo == TipoCliente.REGISTRADO)
            {
                if (cliente.CuotaMaxima is null || cliente.CuotaMaxima <= 0)
                {
                    return BadRequest("Los clientes REGISTRADOS deben tener una cuota máxima válida.");
                }
            }
            else if (cliente.Tipo == TipoCliente.SOCIO)
            {
                if (cliente.CuotaMaxima.HasValue)
                {
                    return BadRequest("Los SOCIOS no deben tener una cuota máxima.");
                }
            }

            // Agrego el cliente al contexto, guardo en la base de datos y devuelvo un 200 OK
            context.Add(cliente);
            await context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Edita los datos de un cliente existente en la base de datos.
        /// </summary>
        /// <param name="dni">El DNI del cliente a editar.</param>
        /// <param name="cliente">El cliente con los datos actualizados.</param>
        /// <returns>Respuesta 200 OK si se actualiza correctamente o un error 400 si los datos son inválidos.</returns>
        [HttpPut("{dni}")]
        public async Task<ActionResult> EditarCliente(string dni, Cliente cliente)
        {
            // Verifico si el DNI no coincide ya que no se puede cambiar
            if (dni != cliente.DNI)
            {
                return BadRequest("El DNI no puede ser modificado");
            }

            // Repito la validación de cuota máxima según el tipo de cliente.
            if (cliente.Tipo == TipoCliente.REGISTRADO)
            {
                if (cliente.CuotaMaxima is null || cliente.CuotaMaxima <= 0)
                {
                    return BadRequest("Los clientes REGISTRADOS deben tener una cuota máxima válida.");
                }
            }
            else if (cliente.Tipo == TipoCliente.SOCIO)
            {
                if (cliente.CuotaMaxima.HasValue)
                {
                    return BadRequest("Los SOCIOS no deben tener una cuota máxima.");
                }
            }

            // Actualizo el cliente en el contexto, guardo en la base de datos y devuelvo un 200 OK
            context.Update(cliente);
            await context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Elimina un cliente de la base de datos, si no tiene recibos asociados.
        /// </summary>
        /// <param name="dni">El DNI del cliente a eliminar.</param>
        /// <returns>Respuesta 200 OK si se elimina correctamente, o un error 400 si tiene recibos asociados.</returns>
        [HttpDelete("{dni}")]
        public async Task<ActionResult> EliminarCliente(string dni)
        {
            // Carga el cliente con sus recibos asociados.
            var cliente = await context.Clientes
                                       .Include(c => c.Recibos)
                                       .FirstOrDefaultAsync(c => c.DNI == dni);

            // Si no lo encuentra
            if (cliente == null)
            {
                return NotFound("Cliente no encontrado.");
            }

            // Verifico si el cliente tiene recibos asociados.
            if (cliente.Recibos != null && cliente.Recibos.Any())
            {
                // Si tiene recibos, devuelvo un error informando que no se puede eliminar.
                return BadRequest("El cliente tiene recibos asociados y no se puede eliminar. Si lo desea, puede darlo de baja.");
            }

            // Elimino el cliente del contexto, guardo en la base de datos y devuelvo un 200 OK con un mensaje
            context.Clientes.Remove(cliente);
            await context.SaveChangesAsync();

            return Ok("Cliente eliminado correctamente.");
        }

        /// <summary>
        /// Da de baja a un cliente, estableciendo la fecha de baja en la base de datos.
        /// </summary>
        /// <param name="dni">El DNI del cliente que se desea dar de baja.</param>
        /// <param name="fechaBaja">La fecha en la que el cliente será dado de baja.</param>
        /// <returns>Respuesta 200 OK si se da de baja correctamente, o un error 400 si la fecha no es válida.</returns>
        [HttpPut("baja/{dni}")]
        public async Task<ActionResult> DarBajaCliente(string dni, [FromQuery] string fechaBaja)
        {
            // Valido que el formato de la fecha de baja sea "yyyy/MM/dd HH:mm:ss"
            if (!DateTime.TryParseExact(fechaBaja, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaBajaConvertida))
            {
                return BadRequest("El formato de la fecha de baja es incorrecto. Debe ser 'yyyy/MM/dd HH:mm:ss'.");
            }

            // Hago que la fecha se trate como UTC
            if (fechaBajaConvertida.Kind == DateTimeKind.Unspecified)
            {
                fechaBajaConvertida = DateTime.SpecifyKind(fechaBajaConvertida, DateTimeKind.Utc);
            }

            // Busco el cliente para ver si existe
            var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.DNI == dni);
            if (cliente == null)
            {
                return NotFound("Cliente no encontrado.");
            }

            // Valido que no esté ya dado de baja
            if (cliente.FechaBaja != null)
            {
                return BadRequest("El cliente ya se encuentra dado de baja.");
            }

            // Valido que la fecha de baja no sea anterior a la fecha de alta.
            if (fechaBajaConvertida < cliente.FechaAlta)
            {
                return BadRequest($"La fecha de baja no puede ser anterior a la fecha de alta ({cliente.FechaAlta.ToString("yyyy/MM/dd HH:mm:ss")}).");
            }

            // Asigno la fecha de baja al cliente, actualizo el cliente, guardo en la base de datos y devuelvo un 200 OK con un mensaje
            cliente.FechaBaja = fechaBajaConvertida;
            context.Update(cliente);
            await context.SaveChangesAsync();

            return Ok("Cliente dado de baja.");
        }

        /// <summary>
        /// Reactiva a un cliente dado de baja, eliminando la fecha de baja.
        /// </summary>
        /// <param name="dni">El DNI del cliente a reactivar.</param>
        /// <returns>Respuesta 200 OK si el cliente se reactiva correctamente, o un error 404 si no se encuentra el cliente.</returns>
        [HttpPut("reactivar/{dni}")]
        public async Task<ActionResult> ReactivarCliente(string dni)
        {
            // Busco el cliente
            var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.DNI == dni);
            if (cliente == null)
            {
                return NotFound("Cliente no encontrado.");
            }

            // Compruebo que no esté activo ya
            if (cliente.FechaBaja == null)
            {
                return BadRequest("El cliente ya está activo.");
            }

            // Elimino la fecha de baja para reactivar al cliente, actualizo en el contexto, guardo en la base de datos y devuelvo un 200 0K con un mensaje
            cliente.FechaBaja = null;
            context.Update(cliente);
            await context.SaveChangesAsync();

            return Ok("Cliente reactivado.");
        }

        /// <summary>
        /// Realiza una búsqueda avanzada de clientes con diversos filtros.
        /// </summary>
        /// <param name="termino">Término para buscar en el DNI, nombre o apellidos.</param>
        /// <param name="fechaInicio">Fecha de inicio para filtrar los clientes por fecha de alta.</param>
        /// <param name="fechaFin">Fecha de fin para filtrar los clientes por fecha de alta.</param>
        /// <param name="orden">Orden para la lista de clientes (por defecto es "dni").</param>
        /// <param name="estado">Estado de los clientes (todos, activos, baja).</param>
        /// <returns>Una lista de clientes filtrados y ordenados según los criterios proporcionados.</returns>
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<Cliente>>> BuscarClientes(
            [FromQuery] string? termino,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            [FromQuery] string orden = "dni",
            [FromQuery] string estado = "todos")
        {
            // Inicio la consulta sobre los clientes
            var query = context.Clientes.AsQueryable();

            // Si hay término de búsqueda
            if (!string.IsNullOrWhiteSpace(termino))
            {
                // La función 'EF.Functions.ILike' permite realizar una búsqueda insensible a mayúsculas y minúsculas.
                // 'EF.Functions.Unaccent' se utiliza para eliminar acentos de las cadenas de texto y así hacer una búsqueda más flexible.
                query = query.Where(c =>
                    // Se aplica la búsqueda a los campos DNI, Nombre, Apellido1 y Apellido2 del cliente.
                    // El operador '%' se utiliza para indicar que estamos buscando una subcadena dentro del campo.
                    // Así, si el término de búsqueda es "Juan", se devolverán clientes cuyo DNI, Nombre o Apellido contengan "Juan".
                    EF.Functions.ILike(EF.Functions.Unaccent(c.DNI), $"%{termino}%") ||  // Búsqueda insensible a acentos en el DNI
                    EF.Functions.ILike(EF.Functions.Unaccent(c.Nombre), $"%{termino}%") || // Búsqueda insensible a acentos en el Nombre
                                                                                           // Se hace lo mismo para los Apellidos, considerando que uno de los apellidos puede ser nulo.
                    (c.Apellido1 != null && EF.Functions.ILike(EF.Functions.Unaccent(c.Apellido1), $"%{termino}%")) ||  // Apellido1 puede ser nulo, por eso la comprobación
                    (c.Apellido2 != null && EF.Functions.ILike(EF.Functions.Unaccent(c.Apellido2), $"%{termino}%"))); // Apellido2 también puede ser nulo
            }

            // Este bloque aplica filtros en las fechas de alta de los clientes, permitiendo que el usuario 
            // busque clientes cuya fecha de alta esté en un rango específico.

            if (fechaInicio.HasValue) // Si se ha proporcionado una fecha de inicio en el filtro.
            {
                var fi = fechaInicio.Value;  // Se obtiene el valor de la fecha de inicio.
                                             // Verifica si la fecha de inicio no tiene especificado el tipo de zona horaria (Kind).
                if (fi.Kind == DateTimeKind.Unspecified)
                {
                    fi = DateTime.SpecifyKind(fi, DateTimeKind.Utc);  // Si no tiene zona horaria, le asigno UTC UTC.
                }

                query = query.Where(c => c.FechaAlta >= fi);  // Filtra los clientes cuya fecha de alta sea mayor o igual a la fecha de inicio.
            }

            if (fechaFin.HasValue)
            {
                var ff = fechaFin.Value;

                if (ff.Kind == DateTimeKind.Unspecified)
                {
                    ff = DateTime.SpecifyKind(ff, DateTimeKind.Utc);
                }

                query = query.Where(c => c.FechaAlta <= ff);
            }

            // Filtro para mostrar solo clientes activos, dados de baja, o todos.
            switch (estado.ToLower())
            {
                case "activos":
                    query = query.Where(c => c.FechaBaja == null);
                    break;
                case "baja":
                    query = query.Where(c => c.FechaBaja != null);
                    break;
                    // "todos": sin filtro adicional.
            }

            // Ordena los resultados por fecha de alta o por DNI según el parámetro.
            query = orden.ToLower() == "fecha"
                ? query.OrderBy(c => c.FechaAlta)
                : query.OrderBy(c => c.DNI);

            // Ejecuto la consulta.
            var clientes = await query.ToListAsync();

            // Si no hay resultados devuelvo 404 con mensaje
            if (clientes.Count == 0)
            {
                return NotFound("No se encontraron clientes con esos criterios.");
            }

            // Devuelvo los clientes encontrados
            return Ok(clientes);
        }
    }
}
