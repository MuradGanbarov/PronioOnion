using FluentValidation;
using ProniaOnion.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")./*Matches(@"^[0-9A-Z]([-.\w]*[0-9A-Z])*$").*/
                MaximumLength(256).WithMessage("Email can contain maximum 256 characters");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required").
                MinimumLength(8).WithMessage("Password should contain minimum 8 characters").
                MaximumLength(256).WithMessage("Password can contain maximum 256 characters");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required").
                MinimumLength(4).WithMessage("Username should contain minimum 4 characters").
                MaximumLength(50).WithMessage("Username can contain maximum 50 characters");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").
                MinimumLength(2).WithMessage("Name should contain minimum 2 characters").
                MaximumLength(50).WithMessage("Name can contain maximum 50 characters");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Surname is required").
                MinimumLength(3).WithMessage("Surname should contain minimum 3 characters").
                MaximumLength(50).WithMessage("Surname can contain maximum 50 characters");
            RuleFor(x => x).Must(x => x.ConfirmPassword == x.Password).WithMessage("Password and ConfirmPassword should be equal");

        }
    }
}
