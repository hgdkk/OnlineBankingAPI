using Newtonsoft.Json;
using OnlineBankingAPI.DAL;
using OnlineBankingAPI.Models;
using OnlineBankingAPI.Services.Abstract;
using System;
using System.Linq;

namespace OnlineBankingAPI.Services.Concrete
{
    public class CustomerService : ICustomerService
    {
        private OnlineBankingDbContext _dbContext;
        private ILoggerService _logger;
        private static string classInfo = "Customer";

        public CustomerService(OnlineBankingDbContext dbContext, ILoggerService logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Response Create(Customer customer)
        {
            Response response = new Response();
            if (!_dbContext.Customers.Any(x => x.IdentityNumber == customer.IdentityNumber))
            {
                if (!_dbContext.Customers.Any(x => x.Email == customer.Email))
                {

                    customer.CreatedDate = DateTime.Now;
                    customer.LastUpdateDate = DateTime.Now;

                    _dbContext.Customers.Add(customer);
                    _dbContext.SaveChanges();

                    response.IsSuccess = true;
                    response.ResponseMessage = "Success";
                    response.Data = customer;

                    Log log = new Log()
                    {
                        LogDate = DateTime.Now,
                        Class = classInfo,
                        LogType = LogType.Info,
                        Operation = OperationType.Create,
                        Method = "Create",
                        NewVersion = JsonConvert.SerializeObject(customer)
                    };
                    _logger.LogInfo(log);
                }
                else
                {
                    response.IsSuccess = false;
                    response.ResponseMessage = "Customer already exists with this email";
                }
            }
            else
            {
                response.IsSuccess = false;
                response.ResponseMessage = "Customer already exists with this identity number";
            }

            return response;
        }

        public Response GetAllCustomers()
        {
            Response response = new Response();
            response.IsSuccess = true;
            response.ResponseMessage = "Success";
            response.Data = _dbContext.Customers.ToList();

            Log log = new Log()
            {
                LogDate = DateTime.Now,
                Class = classInfo,
                LogType = LogType.Info,
                Operation = OperationType.Read,
                Method = "GetAllCustomers"
            };
            _logger.LogInfo(log);

            return response;
        }

        public Response Update(Customer customer)
        {
            Response response = new Response();
            var customerToBeUpdated = _dbContext.Customers.Where(x => x.IdentityNumber == customer.IdentityNumber).FirstOrDefault();
            var oldCustomer = JsonConvert.SerializeObject(customerToBeUpdated);
            if (customerToBeUpdated == null)
            {
                response.IsSuccess = false;
                response.ResponseMessage = "Customer does not exist";
            }
            else
            {
                customerToBeUpdated.Phone = customer.Phone;
                customerToBeUpdated.Email = customer.Email;
                customerToBeUpdated.LastUpdateDate = DateTime.Now;
                _dbContext.Customers.Update(customerToBeUpdated);
                _dbContext.SaveChanges();

                response.IsSuccess = true;
                response.ResponseMessage = "Success";
                response.Data = customerToBeUpdated;

                Log log = new Log()
                {
                    LogDate = DateTime.Now,
                    Class = classInfo,
                    LogType = LogType.Info,
                    Operation = OperationType.Update,
                    Method = "Update",
                    NewVersion = JsonConvert.SerializeObject(customerToBeUpdated),
                    OldVersion = oldCustomer
                };
                _logger.LogInfo(log);
            }

            return response;
        }

    }
}
