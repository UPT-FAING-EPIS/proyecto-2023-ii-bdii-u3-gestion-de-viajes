using System;
using System.Collections.Generic;
using FluentValidation;
using ClienteAPI.DTOs;


namespace ClienteAPI.Validaciones
{
    
    public class ClienteValidator : AbstractValidator<ClienteCreateDTO>
    {
        public ClienteValidator(){
            /* RuleFor(t => t.IdCliente).Empty(); */
            RuleFor(t => t.Nombre).NotEmpty().MaximumLength(50).WithMessage("INTRODUCIR MAXIMO 5  CARACTERES.");
            RuleFor(t => t.ApellidoPaterno).NotEmpty().MaximumLength(50);
            RuleFor(t => t.ApellidoMaterno).NotEmpty().MaximumLength(50);
            /* RuleFor(t => t.Celular).NotEmpty().GreaterThan(0).WithMessage("INGRESAR UN NUMERO DE 9 DIGITOS"); */
            RuleFor(t => t.Celular)
                .GreaterThan(0)
                .WithMessage("El número de celular debe ser mayor que 0")
                .Must(celular => celular >= 100000000 && celular <= 999999999)
                .WithMessage("El número de celular debe tener exactamente 9 dígitos")
                .When(celular => celular != null) // Solo aplica la validación si el valor no es nulo
                .Must(celular => celular.ToString().Length == 9)
                .WithMessage("El número de celular debe tener exactamente 9 dígitos");
            RuleFor(t => t.Genero).NotEmpty().MaximumLength(30);
        }
    }

}
