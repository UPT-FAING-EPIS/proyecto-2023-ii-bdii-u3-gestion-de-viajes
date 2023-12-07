using System;
using System.Collections.Generic;

namespace ClienteAPI.Models;

public partial class TipoResidencium
{
    public byte IdResidencia { get; set; }

    public int IdCli { get; set; }

    public string DesTipResi { get; set; } = null!;

    public string Pais { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public string Provincia { get; set; } = null!;

    public virtual Cliente IdCliNavigation { get; set; } = null!;
}
