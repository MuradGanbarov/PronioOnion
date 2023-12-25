
namespace ProniaOnion.Application.DTOs.Products
{
    public record ProductCreateDto(string Name,decimal Price,string SKU,string ? Description,int CategoryId,ICollection<int>? TagIds, ICollection<int>? ColorIds); //bura ICollection tipinden TagIds de elave olunmalidi
    
}
