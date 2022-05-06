using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBankingAPI.Models
{
    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        public string TransactionReference { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TransactionAmount { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public bool IsSuccess => TransactionStatus.Equals(TransactionStatus.Success);
        public string TransactionAccount { get; set; }
        public string TransactionParticulars { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }

        public Transaction()
        {
            TransactionReference = $"{Guid.NewGuid().ToString().Replace("-", "").Substring(1, 27)}";
        }
    }

    public enum TransactionStatus
    {
        Failed,
        Success,
        Error
    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal
    }

}
