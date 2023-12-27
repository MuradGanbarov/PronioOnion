using FluentValidation;
using ProniaOnion.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.UserNameOrEmail).NotEmpty().WithMessage("Username/Email is required");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required").
                MinimumLength(8).WithMessage("Password should contain minimum 8 characters").
                MaximumLength(256).WithMessage("Password can contain maximum 256 characters");

        }
    }
}
