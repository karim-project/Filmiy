using System.ComponentModel.DataAnnotations;

namespace Filmiy.Models
{
    public class Actor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = null!;
        public string? Bio { get; set; }

        public string? Image { get; set; }

        public ICollection<MovieActor> MovieActors { get; set; } = new HashSet<MovieActor>();
    }
}
