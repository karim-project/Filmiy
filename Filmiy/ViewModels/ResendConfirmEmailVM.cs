using System.ComponentModel.DataAnnotations;

namespace Filmiy.ViewModels
{
    public class ResendConfirmEmailVM
    {
        [Required]
        public string UserNameOREmail { get; set; } = string.Empty;
    }
}
