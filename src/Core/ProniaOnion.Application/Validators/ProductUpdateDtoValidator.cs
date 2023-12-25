using FluentValidation;
using ProniaOnion.Application.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Validators
{
    internal class ProductUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is important").
               MaximumLength(50).WithMessage("Name must may contain maximum 100 characters")
               .MinimumLength(5).WithMessage("Name may contain at least 2 characters");
            RuleFor(x => x.SKU).NotEmpty().WithMessage("SKU is important").
                MaximumLength(10).WithMessage("SKU must may contain 10 characters");
            RuleFor(x => x.Price).NotEmpty()/*.Must(x=>x > 10 && x<999999.99m)*/.WithMessage("Price is important").LessThanOrEqualTo(999999.99m).GreaterThanOrEqualTo(10);
            RuleFor(x => x.Description).MaximumLength(1000).WithMessage("Description may contain 1000 characters");
            RuleFor(x => x.CategoryId).Must(c => c > 0);
            RuleFor(x => x.CategoryId).NotNull();
            RuleForEach(x => x.TagIds).Must(c => c > 0);
            RuleFor(x => x.TagIds).NotNull();
        }
    }
}
