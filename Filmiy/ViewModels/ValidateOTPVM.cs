using System.ComponentModel.DataAnnotations;

namespace Filmiy.ViewModels
{
    public class ValidateOTPVM
    {
        [Required]
        public string OTP {  get; set; } = string.Empty;

        public string ApplicationUserId { get; set; } = string.Empty;
    }
}
