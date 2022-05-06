using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using OnlineBankingAPI.DAL;
using OnlineBankingAPI.Models;
using OnlineBankingAPI.Services.Abstract;
using System;
using System.Linq;
using System.Text;
namespace OnlineBankingAPI.Services.Concrete
{
    public class AccountService : IAccountService
    {
        private OnlineBankingDbContext _dbContext;
        private ILoggerService _logger;
        private static string classInfo = "Account";
        public AccountService(OnlineBankingDbContext dbContext, ILoggerService logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Response Authenticate(string accountNumber, string pin)
        {
            Response response = new Response();
            var account = _dbContext.Accounts.Where(x => x.AccountNumber == accountNumber).SingleOrDefault();
            if (account == null)
            {
                response.IsSuccess = false;
                response.ResponseMessage = "Account does not exist";
                return response;
            }

            if (!VerifyPinHash(pin, account.PinHash, account.PinSalt))
            {
                response.IsSuccess = false;
                response.ResponseMessage = "Unauthenticated";
            }
            else
            {
                response.IsSuccess = true;
                response.ResponseMessage = "Authenticated";
                response.Data = account;

                Log log = new Log()
                {
                    LogDate = DateTime.Now,
                    Class = classInfo,
                    LogType = LogType.Info,
                    Operation = OperationType.Read,
                    Method = "Authenticate"
                };
                _logger.LogInfo(log);
            }

            return response;
        }

        private static bool VerifyPinHash(string pin, byte[] pinHash, byte[] pinSalt)
        {
            if (string.IsNullOrEmpty(pin)) throw new ArgumentNullException("Pin");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
                var computedPinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
                for (int i = 0; i < computedPinHash.Length; i++)
                {
                    if (computedPinHash[i] != pinHash[i]) return false;
                }
            }

            return true;
        }

        public Response Create(string identityNumber, Account account, string pin, string confirmPin)
        {
            Response response = new Response();
            if (!pin.Equals(confirmPin))
            {
                response.IsSuccess = false;
                response.ResponseMessage = "Pins do not match";
                return response;
            }

            var customer = _dbContext.Customers.Where(x => x.IdentityNumber == identityNumber).FirstOrDefault();
            if (customer == null)
            {
                response.IsSuccess = false;
                response.ResponseMessage = "Customer does not exist";
            }
            else
            {
                byte[] pinHash, pinSalt;
                CreatePinHash(pin, out pinHash, out pinSalt);

                account.CustomerId = customer.CustomerId;
                account.PinHash = pinHash;
                account.PinSalt = pinSalt;
                account.CreatedDate = DateTime.Now;
                account.LastUpdateDate = DateTime.Now;

                _dbContext.Accounts.Add(account);
                _dbContext.SaveChanges();

                response.IsSuccess = true;
                response.ResponseMessage = "Success";
                response.Data = account;


                Log log = new Log()
                {
                    LogDate = DateTime.Now,
                    Class = classInfo,
                    LogType = LogType.Info,
                    Operation = OperationType.Create,
                    Method = "Create",
                    NewVersion = JsonConvert.SerializeObject(account)
                };
                _logger.LogInfo(log);
            }

            return response;
        }

        private static void CreatePinHash(string pin, out byte[] pinhash, out byte[] pinSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }

        public Response Delete(AuthenticateModel model)
        {
            Response response = new Response();
            var account = _dbContext.Accounts.Where(x => x.AccountNumber == model.AccountNumber).SingleOrDefault();
            if (account == null)
            {
                response.IsSuccess = false;
                response.ResponseMessage = "Account does not exist";
                return response;
            }

            if (!VerifyPinHash(model.Pin, account.PinHash, account.PinSalt))
            {
                response.IsSuccess = false;
                response.ResponseMessage = "Invalid credentials";
                return response;
            }

            _dbContext.Accounts.Remove(account);
            _dbContext.SaveChanges();
            response.IsSuccess = true;
            response.ResponseMessage = "Success";

            Log log = new Log()
            {
                LogDate = DateTime.Now,
                Class = classInfo,
                LogType = LogType.Info,
                Operation = OperationType.Delete,
                Method = "Delete",
                OldVersion = JsonConvert.SerializeObject(account)
            };
            _logger.LogInfo(log);

            return response;
        }

        public Response GetAllAccounts()
        {
            Response response = new Response();
            response.IsSuccess = true;
            response.ResponseMessage = "Success";
            response.Data = _dbContext.Accounts.ToList();

            Log log = new Log()
            {
                LogDate = DateTime.Now,
                Class = classInfo,
                LogType = LogType.Info,
                Operation = OperationType.Read,
                Method = "GetAllAccounts"
            };
            _logger.LogInfo(log);

            return response;
        }

        public Response GetAccountByAccountNumber(string accountNumber)
        {
            Response response = new Response();

            var account = _dbContext.Accounts.Where(x => x.AccountNumber == accountNumber).FirstOrDefault();
            if (account == null)
            {
                response.IsSuccess = false;
                response.ResponseMessage = "Account does not exist";
            }
            else
            {
                response.IsSuccess = true;
                response.ResponseMessage = "Success";
                response.Data = account;

                Log log = new Log()
                {
                    LogDate = DateTime.Now,
                    Class = classInfo,
                    LogType = LogType.Info,
                    Operation = OperationType.Read,
                    Method = "GetAccountByAccountNumber"
                };
                _logger.LogInfo(log);
            }

            return response;
        }

        public Response GetAccountById(int Id)
        {
            Response response = new Response();

            var account = _dbContext.Accounts.Where(x => x.AccountId == Id).FirstOrDefault();
            if (account == null)
            {
                response.IsSuccess = false;
                response.ResponseMessage = "Account does not exist";
            }
            else
            {
                response.IsSuccess = true;
                response.ResponseMessage = "Success";
                response.Data = account;

                Log log = new Log()
                {
                    LogDate = DateTime.Now,
                    Class = classInfo,
                    LogType = LogType.Info,
                    Operation = OperationType.Read,
                    Method = "GetAccountById"
                };
                _logger.LogInfo(log);
            }

            return response;
        }

        public Response Update(Account account, string pin = null)
        {
            Response response = new Response();
            var accountToBeUpdated = _dbContext.Accounts.Where(x => x.AccountNumber == account.AccountNumber).FirstOrDefault();
            var oldAccount = JsonConvert.SerializeObject(accountToBeUpdated);
            if (accountToBeUpdated == null)
            {
                response.IsSuccess = false;
                response.ResponseMessage = "Account does not exist";
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(pin))
                {
                    byte[] pinHash, pinSalt;
                    CreatePinHash(pin, out pinHash, out pinSalt);
                    accountToBeUpdated.PinHash = pinHash;
                    accountToBeUpdated.PinSalt = pinSalt;
                }

                accountToBeUpdated.LastUpdateDate = DateTime.Now;

                _dbContext.Accounts.Update(accountToBeUpdated);
                _dbContext.SaveChanges();

                response.IsSuccess = true;
                response.ResponseMessage = "Success";
                response.Data = accountToBeUpdated;

                Log log = new Log()
                {
                    LogDate = DateTime.Now,
                    Class = classInfo,
                    LogType = LogType.Info,
                    Operation = OperationType.Update,
                    Method = "Update",
                    NewVersion = JsonConvert.SerializeObject(accountToBeUpdated),
                    OldVersion = oldAccount
                };
                _logger.LogInfo(log);
            }

            return response;
        }

        public Response GetCustomerAccounts(string identityNumber)
        {
            Response response = new Response();
            var customer = _dbContext.Customers.Where(x => x.IdentityNumber == identityNumber).FirstOrDefault();
            if (customer == null)
            {
                response.IsSuccess = false;
                response.ResponseMessage = "Customer does not exist";
                return response;
            }

            var accountList = _dbContext.Accounts.Where(x => x.CustomerId == customer.CustomerId).ToList();

            response.IsSuccess = true;
            response.Data = accountList;
            response.ResponseMessage = "Accounts listed successfully";

            Log log = new Log()
            {
                LogDate = DateTime.Now,
                Class = classInfo,
                LogType = LogType.Info,
                Operation = OperationType.Read,
                Method = "GetCustomerAccounts"
            };
            _logger.LogInfo(log);

            return response;
        }
 
    }
}
