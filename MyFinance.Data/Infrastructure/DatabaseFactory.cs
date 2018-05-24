using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFinance.Data.Infrastructure
{
public class DatabaseFactory : Disposable, IDatabaseFactory
{
    private MyFinanceContext dataContext;
    public MyFinanceContext Get()
    {
        return dataContext ?? (dataContext = new MyFinanceContext());
    }
    protected override void DisposeCore()
    {
        if (dataContext != null)
            dataContext.Dispose();
    }
}
}
