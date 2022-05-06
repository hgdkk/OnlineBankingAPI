using System.ComponentModel.DataAnnotations;

namespace OnlineBankingAPI.Models
{
    public class UpdateAccountModel
    {
        [Required]
        public string AccountNumber { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Pin must be 4 digits")]
        public string Pin { get; set; }

        [Required]
        [Compare("Pin", ErrorMessage = "Pins do not match")]
        public string ConfirmPin { get; set; }

    }
}
