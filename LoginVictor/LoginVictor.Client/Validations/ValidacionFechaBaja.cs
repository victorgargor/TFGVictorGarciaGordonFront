using BlazorAppVictor.Entidades;
using System.ComponentModel.DataAnnotations;

namespace BlazorAppVictor.Validations
{
    public class ValidacionFechaBaja : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var cliente = value as Cliente;

            // Sino hay cliente no se valida
            if (cliente == null) { return true; }

            // Si la FechaBaja es null es válida
            if (cliente.FechaBaja == null) { return true; }

            // Para validar que la FechaBaja no sea anterior a FechaAlta
            return cliente.FechaBaja >= cliente.FechaAlta;
        }

        public override string FormatErrorMessage(string name)
        {
            return "La fecha de baja no puede ser anterior a la fecha de alta.";
        }
    }
}
