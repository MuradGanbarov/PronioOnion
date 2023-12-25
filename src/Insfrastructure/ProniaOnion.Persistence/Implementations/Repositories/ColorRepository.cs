using ProniaOnion.Application.Abstraction.Repositories;
using ProniaOnion.Domain.Entities;
using ProniaOnion.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Persistence.Implementations.Repositories
{
    public class ColorRepository : Repository<Color>,IColorRepository
    {
        public ColorRepository(AppDbContext context) : base(context) { }
       
    }
}
