using System;
using System.Collections.Generic;
using FluentValidation;
using ClienteAPI.DTOs;


namespace ClienteAPI.Validacioness;

public class TipoResidenciumDTOValidator : AbstractValidator<TipoResidenciumDTO>
{
    public TipoResidenciumDTOValidator(){
        RuleFor(t => t.IdResidencia).Empty();
        RuleFor(t => t.IdCli).NotEmpty();
        RuleFor(t => t.DesTipResi).NotEmpty().MaximumLength(40);
        RuleFor(t => t.Pais).NotEmpty().MaximumLength(50);
        RuleFor(t => t.Ciudad).NotEmpty().MaximumLength(15);
        RuleFor(t => t.Provincia).NotEmpty().MaximumLength(30).WithMessage("SE EXCEDIO EL RANGO DE 30 CARACTERES");
    }
}