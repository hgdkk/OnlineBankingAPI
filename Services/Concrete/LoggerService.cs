using OnlineBankingAPI.DAL;
using OnlineBankingAPI.Models;
using OnlineBankingAPI.Services.Abstract;
namespace OnlineBankingAPI.Services.Concrete
{
    public class LoggerService : ILoggerService
    {
        private OnlineBankingDbContext _dbContext;
        public LoggerService(OnlineBankingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void LogInfo(Log log)
        {            
            _dbContext.Logs.Add(log);
            _dbContext.SaveChanges();
        }
    }
}
