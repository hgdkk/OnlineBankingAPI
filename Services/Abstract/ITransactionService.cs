using OnlineBankingAPI.Models;
using System;

namespace OnlineBankingAPI.Services.Abstract
{
    public interface ITransactionService
    {
        Response FindCustomerTransactionsByDate(string identityNumber, DateTime startDate, DateTime endDate);
        Response Makedeposit(string accountNumber, decimal amount, string pin);
        Response MakeWithdrawal(string accountNumber, decimal amount, string pin);

        Response GetAccountTransactions(string accountNumber, string pin);
    }
}
