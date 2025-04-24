﻿namespace BlazorAppVictor.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Perfil { get; set; } = string.Empty;

        public DateTime FechaRegistro { get; set; }
    }
}