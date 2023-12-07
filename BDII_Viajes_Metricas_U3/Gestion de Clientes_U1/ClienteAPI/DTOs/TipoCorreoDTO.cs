using System;
using System.Collections.Generic;

namespace ClienteAPI.DTOs;

public partial class TipoCorreoDTO
{
    public byte IdTipoCorreo { get; set; }

    public int IdCli { get; set; }

    public string TipoCorreo1 { get; set; } = null!;

    public string DesTipoCorreo { get; set; } = null!;
}
