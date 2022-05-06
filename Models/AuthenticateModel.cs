using System.ComponentModel.DataAnnotations;

namespace OnlineBankingAPI.Models
{
    public class AuthenticateModel
    {
        [Required]
        [RegularExpression(@"^[0][1-9]\d{9}$|^[1-9]\d{9}$", ErrorMessage = "Account number must be 10 digits")]
        public string AccountNumber { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Pin must be 4 digits")]
        public string Pin { get; set; }
    }
}
