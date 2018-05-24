using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyFinance.Domain;
using MyFinance.Data.Infrastructure;

namespace MyFinance.Data
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
       
    }
    public interface IRoleRepository : IRepository<Role>
    {
       
    }
}
