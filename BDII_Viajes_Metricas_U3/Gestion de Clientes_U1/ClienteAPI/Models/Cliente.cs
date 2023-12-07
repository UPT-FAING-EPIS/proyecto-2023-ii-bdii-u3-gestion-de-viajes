using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClienteAPI.Models
{
    public partial class Cliente
    {
        public int IdCliente { get; set; }

        [Required]
        public string NomCliente { get; set; } = null!;

        [Required]
        public string ApePaterno { get; set; } = null!;

        [Required]
        public string ApeMaterno { get; set; } = null!;

        public int Numero { get; set; }

        public string? Genero { get; set; }


        public virtual ICollection<TipoCorreo?> TipoCorreos { get; set; } = new List<TipoCorreo?>();

        public virtual ICollection<TipoDireccion?> TipoDireccions { get; set; } = new List<TipoDireccion?>();

        public virtual ICollection<TipoResidencium?> TipoResidencia { get; set; } = new List<TipoResidencium?>();

        public virtual ICollection<TipoSeguro?> TipoSeguros { get; set; } = new List<TipoSeguro?>();

        public virtual ICollection<TiposDocumento?> TiposDocumentos { get; set; } = new List<TiposDocumento?>();
    }
}
