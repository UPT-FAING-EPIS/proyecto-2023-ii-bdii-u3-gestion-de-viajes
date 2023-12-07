using System;
using System.Collections.Generic;
using FluentValidation;
using ClienteAPI.DTOs;


namespace ClienteAPI.Validacioness;


public class TiposDocumentoDTOValidator : AbstractValidator<TiposDocumentoDTO>
{
    public TiposDocumentoDTOValidator(){
        RuleFor(t => t.IdTipoDocumento).Empty();
        RuleFor(t => t.IdCli).NotEmpty();
        RuleFor(t => t.DesTipoDocumento).NotEmpty().MaximumLength(40);
        RuleFor(t => t.NumDocumento).NotEmpty();
        RuleFor(t => t.FechaEmision).NotEmpty();
        RuleFor(t => t.FechaVencimiento).NotEmpty().WithMessage("SE EXCEDIO EL RANGO DE 30 CARACTERES");
    }
}