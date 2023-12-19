using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string CreatedBY { get; set; } = null!;


        public BaseEntity()
        {
            CreatedAt = DateTime.Now;
            CreatedBY = "murad.ganbarov";
        }


    }
}
