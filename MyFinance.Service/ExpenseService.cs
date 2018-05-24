using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyFinance.Domain;
using MyFinance.Data.Infrastructure;
using MyFinance.Data;

namespace MyFinance.Service
{
     public interface IExpenseService
    {
        IEnumerable<Expense> GetExpenses(DateTime startDate, DateTime ednDate);
        Expense GetExpense(int id);        
        void CreateExpense(Expense expense);
        void DeleteExpense(int id);
        void SaveExpense();
    }
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository expenseRepository;       
        private readonly IUnitOfWork unitOfWork;
        public ExpenseService(IExpenseRepository expenseRepository, IUnitOfWork unitOfWork)
        {         
            this.expenseRepository = expenseRepository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Expense> GetExpenses(DateTime startDate, DateTime endDate)
        {
            var expenses = expenseRepository.GetMany(exp => exp.Date >= startDate && exp.Date <= endDate);
            return expenses;
        }
        public void CreateExpense(Expense expense)
        {
            expenseRepository.Add(expense);
            SaveExpense();
        }
        public Expense GetExpense(int id)
        {
            var expense = expenseRepository.GetById(id);
            return expense;
        }
        public void DeleteExpense(int id)
        {
            var expense = expenseRepository.GetById(id);
            expenseRepository.Delete(expense);
            SaveExpense();
        }
        public void SaveExpense()
        {
            unitOfWork.Commit();
        }
    }
}
