using System.ComponentModel.DataAnnotations;

namespace OnlineBankingAPI.Models
{
    public class CreateNewCustomerModel
    {
        [Required]
        [RegularExpression(@"^[0-9]{11}$", ErrorMessage = "Identity number must be 11 digits")]
        public string IdentityNumber { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
