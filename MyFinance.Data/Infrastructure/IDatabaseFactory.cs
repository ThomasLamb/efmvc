using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFinance.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        MyFinanceContext Get();
    }
}
