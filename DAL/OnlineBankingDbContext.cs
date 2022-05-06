using Microsoft.EntityFrameworkCore;
using OnlineBankingAPI.Models;

namespace OnlineBankingAPI.DAL
{
    public class OnlineBankingDbContext : DbContext
    {
        public OnlineBankingDbContext(DbContextOptions<OnlineBankingDbContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Log> Logs { get; set; }
    }
}
