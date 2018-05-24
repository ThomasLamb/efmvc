using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using MyFinance.Domain;
namespace MyFinance.Data
{
    public class MyFinanceContext : DbContext
    {
       // public MyFinanceContext() : base("MyFinance") { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public virtual void Commit()
        {
            base.SaveChanges();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
             modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .Map(m =>
            {
                m.ToTable("RoleMemberships");
                m.MapLeftKey("UserName");
                m.MapRightKey("RoleName");
            });
        }
    }

    public class MyFinanceContextInitializer : DropCreateDatabaseIfModelChanges<MyFinanceContext>
    {
        protected override void Seed(MyFinanceContext context)
        {
            var roles = new List<Role>{
                new Role{RoleName = "Administrator"},
                new Role{RoleName = "User"}               
            };

            roles.ForEach(r => context.Roles.Add(r));
        }
    }
}
