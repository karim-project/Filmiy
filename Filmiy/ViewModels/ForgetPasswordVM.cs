using System.ComponentModel.DataAnnotations;

namespace Filmiy.ViewModels
{
    public class ForgetPasswordVM
    {
        [Required]
        public string UserNameOREmail { get; set; } = string.Empty;
    }
}
