using Microsoft.JSInterop;

namespace BlazorAppVictor.Helpers
{
    /// <summary>
    /// Métodos de extensión para facilitar el uso del localStorage desde Blazor con IJSRuntime.
    /// </summary>
    public static class IJSRuntimeExtensionMethods
    {
        /// <summary>
        /// Guarda un valor en localStorage con la llave especificada.
        /// </summary>
        /// <param name="js">Instancia de IJSRuntime para ejecutar JS.</param>
        /// <param name="llave">Clave del item en localStorage.</param>
        /// <param name="contenido">Contenido que se desea almacenar.</param>
        public static ValueTask<object> GuardarEnLocalStorage(this IJSRuntime js,
            string llave, string contenido)
        {
            // Ejecuta localStorage.setItem(llave, contenido) en JavaScript
            return js.InvokeAsync<object>("localStorage.setItem", llave, contenido);
        }

        /// <summary>
        /// Obtiene un valor del localStorage según la llave especificada.
        /// </summary>
        /// <param name="js">Instancia de IJSRuntime.</param>
        /// <param name="llave">Clave del item que se desea obtener.</param>
        public static ValueTask<object> ObtenerDeLocalStorage(this IJSRuntime js,
            string llave)
        {
            // Ejecuta localStorage.getItem(llave) en JavaScript
            return js.InvokeAsync<object>("localStorage.getItem", llave);
        }

        /// <summary>
        /// Elimina un item del localStorage según la llave proporcionada.
        /// </summary>
        /// <param name="js">Instancia de IJSRuntime.</param>
        /// <param name="llave">Clave del item que se desea eliminar.</param>
        public static ValueTask<object> RemoverDelLocalStorage(this IJSRuntime js,
            string llave)
        {
            // Ejecuta localStorage.removeItem(llave) en JavaScript
            return js.InvokeAsync<object>("localStorage.removeItem", llave);
        }
    }
}
