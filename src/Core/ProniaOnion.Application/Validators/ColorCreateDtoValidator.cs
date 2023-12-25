using FluentValidation;
using ProniaOnion.Application.DTOs.Colors;


namespace ProniaOnion.Application.Validators
{
    internal class ColorCreateDtoValidator : AbstractValidator<ColorCreateDto>
    {
        public ColorCreateDtoValidator()
        {
            RuleFor(c=>c.Name).NotEmpty().MinimumLength(5).MaximumLength(50);
        }
    }
}
