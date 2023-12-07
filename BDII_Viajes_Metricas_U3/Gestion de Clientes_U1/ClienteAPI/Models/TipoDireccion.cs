using System;
using System.Collections.Generic;

namespace ClienteAPI.Models;

public partial class TipoDireccion
{
    public byte IdDireccion { get; set; }

    public int IdCli { get; set; }

    public string TipoDireccion1 { get; set; } = null!;

    public string DesTipoDireccion { get; set; } = null!;

    public string Calle { get; set; } = null!;

    public string? Referencia { get; set; }

    public virtual Cliente IdCliNavigation { get; set; } = null!;
}
