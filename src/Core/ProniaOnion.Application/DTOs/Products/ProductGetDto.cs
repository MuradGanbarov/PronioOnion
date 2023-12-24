using ProniaOnion.Application.DTOs.Categories;
using ProniaOnion.Application.DTOs.Tags;
using ProniaOnion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.DTOs.Products
{
    public record  ProductGetDto(int id, decimal Price, string SKU, string? Description,int CategoryId,IncludeCategoryDto Category,int TagId,  ICollection<IncludeTagDto>? Tags);
    
}
