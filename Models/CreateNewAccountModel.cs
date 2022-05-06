using System.ComponentModel.DataAnnotations;

namespace OnlineBankingAPI.Models
{
    public class CreateNewAccountModel
    {
        [Required]
        [RegularExpression(@"^[0-9]{11}$", ErrorMessage = "Identity number must be 11 digits")]
        public string IdentityNumber { get; set; }
        public AccountType AccountType { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Pin must be 4 digits")]
        public string Pin { get; set; }

        [Compare("Pin", ErrorMessage = "Pins do not match")]
        public string ConfirmPin { get; set; }
    }
}
