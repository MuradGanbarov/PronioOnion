using FluentValidation;
using ProniaOnion.Application.DTOs.Categories;


namespace ProniaOnion.Application.Validators
{
    internal class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50).MinimumLength(5); //match sadece bir ornekcun idi
            
        }
    }
}
