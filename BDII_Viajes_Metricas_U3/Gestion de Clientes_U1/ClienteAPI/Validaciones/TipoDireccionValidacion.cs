using System;
using System.Collections.Generic;
using FluentValidation;
using ClienteAPI.DTOs;

namespace ClienteAPI.Validacioness;


public class TipoDireccionDTOValidator : AbstractValidator<TipoDireccionDTO>
{
    public TipoDireccionDTOValidator(){
        RuleFor(t => t.IdDireccion).Empty();
        RuleFor(t => t.IdCli).NotEmpty();
        RuleFor(t => t.TipoDireccion1).NotEmpty().MaximumLength(40);
        RuleFor(t => t.DesTipoDireccion).NotEmpty().MaximumLength(50);
        RuleFor(t => t.Calle).NotEmpty().MaximumLength(15);
        RuleFor(t => t.Referencia).NotEmpty().MaximumLength(20);
    }
}
