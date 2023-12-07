using System;
using System.Collections.Generic;

namespace ClienteAPI.DTOs
{
    public class ClienteCreateDTO
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = null!;

        public string ApellidoPaterno { get; set; } = null!;

        public string ApellidoMaterno { get; set; } = null!;

        public int Celular { get; set; }

        public string? Genero { get; set; }
    }
}
