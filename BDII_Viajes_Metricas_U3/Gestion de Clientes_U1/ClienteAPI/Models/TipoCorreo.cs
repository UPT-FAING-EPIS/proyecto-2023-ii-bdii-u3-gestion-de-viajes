using System;
using System.Collections.Generic;

namespace ClienteAPI.Models;

public partial class TipoCorreo
{
    public byte IdTipoCorreo { get; set; }

    public int IdCli { get; set; }

    public string TipoCorreo1 { get; set; } = null!;

    public string DesTipoCorreo { get; set; } = null!;

    public virtual Cliente IdCliNavigation { get; set; } = null!;
}
