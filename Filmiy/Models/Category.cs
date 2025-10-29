using System.ComponentModel.DataAnnotations;

namespace Filmiy.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

       // public ICollection<Movie> Movies { get; set; }
    }
}
