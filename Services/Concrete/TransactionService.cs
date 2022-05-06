using Newtonsoft.Json;
using OnlineBankingAPI.DAL;
using OnlineBankingAPI.Models;
using OnlineBankingAPI.Services.Abstract;
using System;
using System.Linq;

namespace OnlineBankingAPI.Services.Concrete
{
    public class TransactionService : ITransactionService
    {
        private OnlineBankingDbContext _dbContext;
        IAccountService _accountService;
        private ILoggerService _logger;

        private static string classInfo = "Transaction";

        public TransactionService(OnlineBankingDbContext dbContext,  IAccountService accountService, ILoggerService logger)
        {
            _dbContext = dbContext;
            _accountService = accountService;
            _logger = logger;
        }


        public Response FindCustomerTransactionsByDate(string identityNumber, DateTime startDate, DateTime endDate)
        {
            Response response = new Response();

            var customer = _dbContext.Customers.Where(x => x.IdentityNumber == identityNumber).FirstOrDefault();
            if (customer == null)
            {
                response.IsSuccess = false;
                response.ResponseMessage = "Customer does not exist";
                return response;
            }

            var accountNumberList = _dbContext.Accounts.Where(x => x.CustomerId == customer.CustomerId).Select(y => y.AccountNumber).ToList();

            var transactions = _dbContext.Transactions.Where(x => x.TransactionDate >= startDate && x.TransactionDate <= endDate && accountNumberList.Contains(x.TransactionAccount)).ToList();
            response.IsSuccess = true;
            response.Data = transactions;
            response.ResponseMessage = "Transactions listed successfully";

            Log log = new Log()
            {
                LogDate = DateTime.Now,
                Class = classInfo,
                LogType = LogType.Info,
                Operation = OperationType.Read,
                Method = "FindTransactionsByDate"
            };
            _logger.LogInfo(log);

            return response;
        }

        public Response Makedeposit(string accountNumber, decimal amount, string pin)
        {
            Response response = new Response();
            Account account;
            Transaction transaction = new Transaction();

            var authUser = _accountService.Authenticate(accountNumber, pin);
            if (!authUser.IsSuccess)
            {
                response.IsSuccess = false;
                response.ResponseMessage = "Invalid credentials";
                return response;
            }

            try
            {

                var res = _accountService.GetAccountByAccountNumber(accountNumber);
                account = (Account)res.Data;
                account.CurrentAccountBalance += amount;

                if (_dbContext.Entry(account).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
                {
                    transaction.TransactionStatus = TransactionStatus.Success;
                    response.IsSuccess = true;
                    response.ResponseMessage = "Transaction successfull";
                }
                else
                {
                    transaction.TransactionStatus = TransactionStatus.Failed;
                    response.IsSuccess = false;
                    response.ResponseMessage = "Transaction failed";
                }

            }
            catch (Exception ex)
            {
                
            }

            transaction.TransactionType = TransactionType.Deposit;
            transaction.TransactionAccount = accountNumber;
            transaction.TransactionAmount = amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"NEW TRANSACTION FROM => {transaction.TransactionAccount} ON DATE => {transaction.TransactionDate} " +
                $"FOR AMOUNT => {transaction.TransactionAmount} TRANSACTION TYPE => {transaction.TransactionType} TRANSACTION STATUS => {transaction.TransactionStatus}";

            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();

            Log log = new Log()
            {
                LogDate = DateTime.Now,
                Class = classInfo,
                LogType = LogType.Info,
                Operation = OperationType.Create,
                Method = "Makedeposit",
                NewVersion = JsonConvert.SerializeObject(transaction)
            };
            _logger.LogInfo(log);

            return response;
        }

        public Response MakeWithdrawal(string accountNumber, decimal amount, string pin)
        {
            Response response = new Response();
            Account account;
            Transaction transaction = new Transaction();

            var authUser = _accountService.Authenticate(accountNumber, pin);
            if (!authUser.IsSuccess)
            {
                response.IsSuccess = false;
                response.ResponseMessage = "Invalid credentials";
                return response;
            }

            try
            {
                var res = _accountService.GetAccountByAccountNumber(accountNumber);
                account = (Account)res.Data;

                if (account.CurrentAccountBalance <= 0)
                {
                    response.IsSuccess = false;
                    response.ResponseMessage = "Current account balance doesn't have enough money";
                    return response;
                }

                account.CurrentAccountBalance -= amount;

                if (_dbContext.Entry(account).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
                {
                    transaction.TransactionStatus = TransactionStatus.Success;
                    response.IsSuccess = true;
                    response.ResponseMessage = "Transaction successfull";
                }
                else
                {
                    transaction.TransactionStatus = TransactionStatus.Failed;
                    response.IsSuccess = false;
                    response.ResponseMessage = "Transaction failed";
                }

            }
            catch (Exception ex)
            {

            }

            transaction.TransactionType = TransactionType.Withdrawal;
            transaction.TransactionAccount = accountNumber;
            transaction.TransactionAmount = amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"NEW TRANSACTION FROM => {transaction.TransactionAccount} ON DATE => {transaction.TransactionDate} " +
                $"FOR AMOUNT => {transaction.TransactionAmount} TRANSACTION TYPE => {transaction.TransactionType} TRANSACTION STATUS => {transaction.TransactionStatus}";

            response.Data = transaction;

            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();

            Log log = new Log()
            {
                LogDate = DateTime.Now,
                Class = classInfo,
                LogType = LogType.Info,
                Operation = OperationType.Create,
                Method = "MakeWithdrawal",
                NewVersion = JsonConvert.SerializeObject(transaction)
            };
            _logger.LogInfo(log);

            return response;
        }

        public Response GetAccountTransactions(string accountNumber, string pin)
        {
            Response response = new Response();
            Account account;
            Transaction transaction = new Transaction();

            var authUser = _accountService.Authenticate(accountNumber, pin);
            if (!authUser.IsSuccess)
            {
                response.IsSuccess = false;
                response.ResponseMessage = "Invalid credentials";
                return response;
            }

            account = (Account)authUser.Data;

            var transactionList = _dbContext.Transactions.Where(x => x.TransactionAccount == account.AccountNumber).ToList();
            response.IsSuccess = true;
            response.Data = transactionList;
            response.ResponseMessage = "Transactions listed successfully";

            Log log = new Log()
            {
                LogDate = DateTime.Now,
                Class = classInfo,
                LogType = LogType.Info,
                Operation = OperationType.Read,
                Method = "GetAccountTransactions"
            };
            _logger.LogInfo(log);
            return response;
        }
    }
}
