using ProniaOnion.Application.DTOs.Categories;
using ProniaOnion.Application.DTOs.Colors;
using ProniaOnion.Application.DTOs.Tags;
namespace ProniaOnion.Application.DTOs.Products
{
    public record ProductGetDto()
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
        public string SKU { get; init; }
        public string Description { get; init; }
        public int CategoryId { get; init; }
        public IncludeCategoryDto Category { get; init; }
        public ICollection<IncludeColorDto> Colors { get; set; } = null!;
        public ICollection<IncludeTagDto> Tags { get; set; } = null!;

    }
        
}

