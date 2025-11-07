using System.ComponentModel.DataAnnotations;

namespace Filmiy.Models
{
    public class Cinema
    {
       

        public int Id { get; set; }

        [Required(ErrorMessage = "Cinema name is required.")]
        [StringLength(200, ErrorMessage = "Cinema name cannot exceed 200 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters.")]
        public string Location { get; set; } = null!;

       
        public int NumberOfHalls { get; set; }

        
        public string? Description { get; set; }

      
        public string? Image { get; set; }

      
        public ICollection<Movie> Movies { get; set; } = new HashSet<Movie>();
    }
}

