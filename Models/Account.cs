using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OnlineBankingAPI.Models
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        [ForeignKey("FK_Customer")]
        public int CustomerId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentAccountBalance { get; set; }

        public AccountType AccountType { get; set; }

        public string AccountNumber { get; set; }

        [JsonIgnore]
        public byte[] PinHash { get; set; }

        [JsonIgnore]
        public byte[] PinSalt { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdateDate { get; set; }


        Random random = new Random();

        public Account()
        {
            AccountNumber = Convert.ToString((long)Math.Floor(random.NextDouble() * 9_000_000_000L + 1_000_000_000L));
        }
    }

    public enum AccountType
    {
        Savings,
        Current,
        Corporate,
        Government,
    }
}
