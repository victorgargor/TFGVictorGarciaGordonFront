using LoginVictor.Datos;
using LoginVictor.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace LoginVictor.Controllers
{
    /// <summary>
    /// Controlador para la gestión de recibos.
    /// Permite agregar, listar, editar y eliminar recibos.
    /// Se validan reglas de negocio, como formato de fecha y restricciones según la cuota máxima del cliente.
    /// </summary>
    [ApiController]
    [Route("api/recibos")]
    public class RecibosController : ControllerBase
    {
        // Contexto de la base de datos inyectado a través del constructor
        private readonly ApplicationDbContext context;

        public RecibosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Agrega un nuevo recibo para un cliente.
        /// Se valida que el número de recibo sea único, que la fecha de emisión tenga el formato correcto,
        /// que el cliente exista y esté activo, y que el importe no exceda la cuota máxima (para clientes REGISTRADOS).
        /// </summary>
        /// <param name="recibo">Objeto Recibo con los datos a agregar.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPost]
        public async Task<ActionResult> AgregarRecibo(Recibo recibo)
        {
            // Verifico que no esté el número de recibo repetido
            if (await context.Recibos.AnyAsync(r => r.NumeroRecibo == recibo.NumeroRecibo))
            {
                return BadRequest("Ya existe un recibo con ese número.");
            }

            // Busco el cliente por su FK (ClienteId)
            var cliente = await context.Clientes.FindAsync(recibo.ClienteId);
            if (cliente == null)
            {
                return NotFound("No se encontró el cliente asociado al recibo.");
            }

            // Verifico que el cliente esté activo (no tenga FechaBaja)
            if (cliente.FechaBaja != null)
            {
                return BadRequest("No se pueden agregar recibos para clientes inactivos.");
            }

            // Valido que la fecha de emisión tenga el formato "yyyy/MM/dd HHmmss"
            if (!EsFormatoFechaReciboValido(recibo.FechaEmision))
            {
                return BadRequest("La fecha de emisión debe tener el formato 'yyyy/MM/dd HHmmss'.");
            }

            // Para clientes de tipo REGISTRADO, valido que el importe no supere la cuota máxima
            if (cliente.Tipo == TipoCliente.REGISTRADO && cliente.CuotaMaxima.HasValue && recibo.Importe > cliente.CuotaMaxima.Value)
            {
                return BadRequest("El importe del recibo excede la cuota máxima permitida para este cliente.");
            }

            // Agrego el recibo y se guardo los cambios en la base de datos
            context.Recibos.Add(recibo);
            await context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Obtiene todos los recibos asociados a un cliente, ordenados por fecha de emisión.
        /// </summary>
        /// <param name="clienteId">Id del cliente.</param>
        /// <returns>Lista de recibos del cliente.</returns>
        [HttpGet("cliente/{clienteId:int}")]
        public async Task<ActionResult<IEnumerable<Recibo>>> ObtenerRecibosPorCliente(int clienteId)
        {
            // Filtro los recibos por el ClienteId y los ordeno por FechaEmision
            var recibos = await context.Recibos
                .Where(r => r.ClienteId == clienteId)
                .OrderBy(r => r.FechaEmision)
                .ToListAsync();

            // En caso de que no se encuentren recibos o el cliente no tenga
            if (recibos == null || recibos.Count == 0)
            {
                return NotFound("No se encontraron recibos para el cliente especificado.");
            }

            // Devuelvo los recibos
            return Ok(recibos);
        }

        /// <summary>
        /// Lista todos los recibos registrados en el sistema, permitiendo ordenar por cliente o por fecha de emisión.
        /// </summary>
        /// <param name="orden">Criterio de ordenación: "cliente" o "fecha".</param>
        /// <returns>Lista de recibos.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recibo>>> ListarRecibos([FromQuery] string orden = "fecha")
        {
            List<Recibo> recibos;

            // Si se ordena por "cliente", se ordena por ClienteId
            if (orden.ToLower() == "cliente")
            {
                recibos = await context.Recibos.OrderBy(r => r.ClienteId).ToListAsync();
            }
            else
            {
                // Por defecto, se ordena por la Fecha de emisión
                recibos = await context.Recibos.OrderBy(r => r.FechaEmision).ToListAsync();
            }
            return Ok(recibos);
        }

        /// <summary>
        /// Obtiene un recibo único por su id
        /// </summary>
        /// <param name="id"> id del recibo a obtener</param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Recibo>> ObtenerRecibo(int id)
        {
            // Busco el recibo
            var recibo = await context.Recibos.FirstOrDefaultAsync(r => r.Id == id);

            // Si no encuentro el recibo
            if (recibo == null)
            {
                return NotFound("Recibo no encontrado.");
            }

            // Si se encuentra se devuelve
            return Ok(recibo);
        }


        /// <summary>
        /// Edita los datos de un recibo existente.
        /// Se valida el formato de la fecha, que el cliente asociado esté activo y que el importe cumpla las reglas de cuota.
        /// </summary>
        /// <param name="id">Id del recibo a editar.</param>
        /// <param name="reciboActualizado">Objeto Recibo con los nuevos datos.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> EditarRecibo(int id, Recibo reciboActualizado)
        {
            // Se busca el recibo existente por su Id
            var reciboExistente = await context.Recibos.FirstOrDefaultAsync(r => r.Id == id);
            if (reciboExistente == null)
            {
                return NotFound("Recibo no encontrado.");
            }
            // Valido el formato de la fecha de emisión
            if (!EsFormatoFechaReciboValido(reciboActualizado.FechaEmision))
            {
                return BadRequest("La fecha de emisión debe tener el formato 'yyyy/MM/dd HHmmss'.");
            }

            // Busco el cliente del recibo correspondiente
            var cliente = await context.Clientes.FindAsync(reciboExistente.ClienteId);

            // Sino se encuentra el recibo
            if (cliente == null)
            {
                return NotFound("Cliente asociado no encontrado.");
            }

            // Verifico que el cliente asociado esté activo
            if (cliente.FechaBaja != null)
            {
                return BadRequest("No se pueden editar recibos de clientes inactivos.");
            }

            // Valido que para clientes de tipo REGISTRADO el importe no supere la cuota máxima
            if (cliente.Tipo == TipoCliente.REGISTRADO && cliente.CuotaMaxima.HasValue && reciboActualizado.Importe > cliente.CuotaMaxima.Value)
            {
                return BadRequest("El importe del recibo excede la cuota máxima permitida para este cliente.");
            }

            // Actualizo los campos necesarios del recibo (todos menos el número de recibo)
            reciboExistente.Importe = reciboActualizado.Importe;
            reciboExistente.FechaEmision = reciboActualizado.FechaEmision;
            await context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Elimina un recibo por su Id.
        /// </summary>
        /// <param name="id">Id del recibo a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> EliminarRecibo(int id)
        {
            // Busco el recibo por su Id
            var recibo = await context.Recibos.FirstOrDefaultAsync(r => r.Id == id);
            if (recibo == null)
            {
                return NotFound("Recibo no encontrado.");
            }
            // Elimino el recibo del contexto, guardo en la base de datos y devuelvo un 200 OK
            context.Recibos.Remove(recibo);
            await context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Método privado que valida que la fecha de emisión tenga el formato "yyyy/MM/dd HHmmss".
        /// Se convierte la fecha a texto usando el formato y se intenta parsearla nuevamente.
        /// </summary>
        /// <param name="fecha">La fecha a validar.</param>
        /// <returns>True si el formato es correcto; de lo contrario, false.</returns>
        private bool EsFormatoFechaReciboValido(DateTime fecha)
        {
            string formatoEsperado = "yyyy/MM/dd HHmmss";
            string fechaComoTexto = fecha.ToString(formatoEsperado);
            return DateTime.TryParseExact(fechaComoTexto, formatoEsperado, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
    }
}
