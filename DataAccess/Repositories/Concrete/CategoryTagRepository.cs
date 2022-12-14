using Core.Entities;
using DataAccess.Contexts;
using DataAccess.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Concrete
{
    public class CategoryTagRepository : Repository<CategoryTag>, ICategoryTagRepository
    {
        public CategoryTagRepository(AppDbContext context) : base(context) { }
    }
}
