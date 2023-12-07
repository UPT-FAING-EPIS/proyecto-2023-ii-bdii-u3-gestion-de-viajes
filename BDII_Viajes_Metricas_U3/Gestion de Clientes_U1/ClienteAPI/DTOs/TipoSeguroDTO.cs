using System;
using System.Collections.Generic;

namespace ClienteAPI.DTOs;

public partial class TipoSeguroDTO
{
    public byte IdSeguro { get; set; }

    public int IdCli { get; set; }

    public string TipSeguro { get; set; } = null!;

    public string Poliza { get; set; } = null!;

    public decimal Cobertura { get; set; }

    public byte[]? DocumentoSeguro { get; set; }
}
