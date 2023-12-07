using System;
using System.Collections.Generic;

namespace ClienteAPI.Models;

public partial class TiposDocumento
{
    public byte IdTipoDocumento { get; set; }

    public int IdCli { get; set; }

    public string DesTipoDocumento { get; set; } = null!;

    public int NumDocumento { get; set; }

    public DateTime FechaEmision { get; set; }

    public DateTime FechaVencimiento { get; set; }

    public byte[]? Imagen { get; set; }

    public virtual Cliente IdCliNavigation { get; set; } = null!;
}
