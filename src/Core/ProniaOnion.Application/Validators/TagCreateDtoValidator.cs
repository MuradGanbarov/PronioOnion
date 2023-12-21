using FluentValidation;
using ProniaOnion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Validators
{
    internal class TagCreateDtoValidator : AbstractValidator<Tag>
    {
        public TagCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is important").
                MaximumLength(50).WithMessage("Name must may contain maximum 100 characters")
                .MinimumLength(5).WithMessage("Name may contain at least 2 characters");
            
        }
    }
}
