using System;

namespace OnlineBankingAPI.Models
{
    public class GetAccountModel
    {      
        public decimal CurrentAccountBalance { get; set; }

        public AccountType AccountType { get; set; }

        public string AccountNumber { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdateDate { get; set; }
    }
}
