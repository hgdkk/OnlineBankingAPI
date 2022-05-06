using OnlineBankingAPI.Models;

namespace OnlineBankingAPI.Services.Abstract
{
    public interface IAccountService
    {
        Response Authenticate(string AccountNumber, string Pin);
        Response GetAllAccounts();
        Response Create(string identityNumber, Account account, string Pin, string ConfirmPin);
        Response Update(Account account, string Pin = null);
        Response Delete(AuthenticateModel model);
        Response GetAccountById(int Id);
        Response GetAccountByAccountNumber(string AccountNumber);
        Response GetCustomerAccounts(string identityNumber);
    }
}
