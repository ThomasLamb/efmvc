using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyFinance.Domain;
using MyFinance.Data.Infrastructure;
namespace MyFinance.Data
{
    public class CategoryRepository: RepositoryBase<Category>, ICategoryRepository
        {
        public CategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }           
        }
    public interface ICategoryRepository : IRepository<Category>
    {
    }
}