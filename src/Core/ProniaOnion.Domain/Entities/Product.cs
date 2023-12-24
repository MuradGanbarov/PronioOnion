using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Domain.Entities
{
    public class Product : BaseNameableEntity
    {
        public decimal Price { get; set; }
        public string SKU { get; set; } = null!;
        public string? Description { get; set; }
        //relation's properties
        public string CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public ICollection<ProductColors>? ProductColors { get; set; } = null!;
        public ICollection<ProductTags>? ProductTags { get; set; }    
    }
}
