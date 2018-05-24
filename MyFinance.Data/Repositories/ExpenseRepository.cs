using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyFinance.Domain;
using MyFinance.Data.Infrastructure;

namespace MyFinance.Data
{
    public class ExpenseRepository : RepositoryBase<Expense>, IExpenseRepository
        {
        public ExpenseRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }           
        }
    public interface IExpenseRepository : IRepository<Expense>
    {
    }
}
