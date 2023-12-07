using System;
using System.Collections.Generic;
using FluentValidation;
using ClienteAPI.DTOs;


namespace ClienteAPI.Validacioness;
public class TipoCorreoDTOValidator : AbstractValidator<TipoCorreoDTO>
    {
        public TipoCorreoDTOValidator(){
            RuleFor(t => t.IdTipoCorreo).Empty();
            RuleFor(t => t.IdCli).NotEmpty().WithMessage("IdCli NO DEBE ESTAR VACIO");
            RuleFor(t => t.TipoCorreo1).NotEmpty().MaximumLength(50);
            RuleFor(t => t.DesTipoCorreo).NotEmpty().MaximumLength(50);
        }
    }
