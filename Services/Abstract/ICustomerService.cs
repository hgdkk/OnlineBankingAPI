using OnlineBankingAPI.Models;

namespace OnlineBankingAPI.Services.Abstract
{
    public interface ICustomerService
    {
        Response GetAllCustomers();
        Response Create(Customer custemer);

        Response Update(Customer customer);
        
    }
}
